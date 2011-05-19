using Limada.Model;
using Limaki.Tests;
using System;
using Limada.Data;
using Limaki.Data;
using Limaki.Common;
using Limaki;
using Limada.UseCases;
using NUnit.Framework;
using Limada.Schemata;
using System.Linq;
using Limaki.Model.Streams;
using System.Collections.Generic;
using Limaki.Graphs.Extensions;
using Limaki.Common.IOC;


namespace Limada.Tests.ThingGraphs {
    [TestFixture]
    public class ReportingTest : DomainTest {
        protected IThingFactory Factory {
            get { return Registry.Factory.Create<IThingFactory>(); }
        }

        public IThingGraphProvider ThingGraphProvider { get; set; }

        public virtual SchemaThingGraph OpenFile(string fileName) {
            if (ThingGraphProvider != null) {
                ThingGraphProvider.Close();
                ThingGraphProvider = null;
            }
            if (ThingGraphProvider == null) {
                var fileManager = new FileManagerBase();
                var file = DataBaseInfo.FromFileName(fileName);
                var provider = fileManager.GetProvider(file);
                if (provider != null) {
                    provider.Open(file);
                    ThingGraphProvider = provider;
                    return new SchemaThingGraph(ThingGraphProvider.Data);
                }
            } else {
                throw new Exception("File " + fileName + " could not be opened");
            }

            return null;
        }


        public virtual void Close() {
            if (ThingGraphProvider != null) {
                ThingGraphProvider.Close();
            }
        }


        public virtual void OnFlush(IThingGraph graph) {
            if (ThingGraphProvider != null) {
                if (ThingGraphProvider.Saveable)
                    ThingGraphProvider.Save();
            }
        }
        
        [TestFixtureTearDown]
        public override void TearDown() {
            Close();
            base.TearDown();
        }


        public IEnumerable<IThing> FindRoot(IThingGraph source, bool doAutoView) {
            source = (source as SchemaThingGraph).Source as IThingGraph;
            var result = new List<IThing>();
            IThing topic = source.GetById(TopicSchema.Topics.Id);
            if (topic != null && (source.Edges(topic).Count > 0)) {
                if (doAutoView) {
                    try {
                        var autoView = (
                            from link in source.Edges(topic)
                            where link.Marker.Id == TopicSchema.AutoViewMarker.Id
                            select source.Adjacent(link, topic)).
                            FirstOrDefault();

                    } catch (Exception e) { }
                }
                if (topic != null) {
                    result.Add(topic);
                }
            } else {
                return GraphPairExtension<IThing, ILink>.FindRoots(source, null).Where(item=>!source.IsMarker(item));
            }
            return result;
        }


        [Test]
        public virtual void WalkThrouFirstLevelTest() {
            var graph = OpenFile(@"S:\temp\sample_4.pib");//(@"E:\testdata\txbProjekt\ADRESSEXAMPLE.pib");
            
            foreach (var thing in FindRoot(graph, true)) {
                var disp = graph.ThingToDisplay(thing);
                ReportDetail("Root:\t"+string.Format("{0}", disp.Data ?? "<null>"));
                var walker = new Walker<IThing, ILink>(graph);

                ReportDetail("********* Things:");
                foreach (var sub in walker.DeepWalk(thing, 0)
                    .Where(i => i.Node!=thing && !(i.Node is ILink))
                    ) {
                    disp = graph.ThingToDisplay(sub.Node);
                    var tabs = "".PadLeft(sub.Level, '\t');
                    ReportDetail(tabs+string.Format("{0}", disp.Data ?? "<null>"));
                }

                walker = new Walker<IThing, ILink>(graph);
                ReportDetail("********* Link.Leafs:");
                foreach (var sub in walker.DeepWalk(thing, 0)
                    .Where(i => (i.Node is ILink))
                    ) {
                    disp = graph.ThingToDisplay((sub.Node as ILink).Leaf);
                    var marker = graph.ThingToDisplay((sub.Node as ILink).Marker) ?? CommonSchema.EmptyMarker;
                    var tabs = "".PadLeft(sub.Level,'\t');
                    ReportDetail(tabs+string.Format("[{0}]:{1}", marker.Data,disp.Data ?? "<null>"));
                }
            }

        }
        [Test]
        public virtual void CleanWrongDocumentsTest() {
            // search for all StringThings where link.Marker == Document && text = null or empty
            // get the Title of things
            // set text of target to title.text
            // remove title
            // set rootlinks.where(marker==Document) to  marker = CommonSchema.Commonmarker
            
            var graph = OpenFile(@"S:\temp\sample_4.pib"); //(@"E:\testdata\txbProjekt\ADRESSEXAMPLE.pib");
            graph.Add(CommonSchema.CommonMarker);
            var nullStringThings = graph.GetByData(null);
            var act = true;
            foreach (var nullStringThing in nullStringThings) {
                var edges = graph.Edges(nullStringThing).ToArray();
                var disp = graph.ThingToDisplay(nullStringThing);
                
                if (disp != nullStringThing) {
                    ReportDetail(string.Format("+\t{0}\t[{1}]", nullStringThing.Data ?? "<null>", nullStringThing.Id.ToString("X")));
                    var titleLink = edges.FirstOrDefault(l => l.Marker.Id == DocumentSchema.DocumentTitle.Id);
                    if (titleLink != null && titleLink.Leaf == disp) {
                        ReportDetail(string.Format("\t-\t{0}\t[{1}]", disp.Data ?? "<null>", disp.Id.ToString("X")));
                        if (true) {
                            nullStringThing.Data = disp.Data;
                            graph.Add(nullStringThing);
                            graph.Remove(titleLink);
                            var dispEdges = graph.Edges(disp).ToArray();
                            foreach (var link in dispEdges) {
                                graph.ChangeEdge(link, nullStringThing, link.Root == disp);
                                graph.Add(link);
                            }
                            graph.Remove(disp);
                        }
                    }
                    var documentLink = edges.FirstOrDefault(l => l.Marker.Id == DocumentSchema.Document.Id);
                    if(documentLink!=null) {
                        ReportDetail(string.Format("\t<>\t{0}\t{1}", DocumentSchema.Document.Data, CommonSchema.CommonMarker.Data));
                        if (act) {
                            documentLink.Marker = CommonSchema.CommonMarker;
                            graph.Add(documentLink);
                        }
                    }
                } else {
                    //if (edges.Count() == 0)
                    //    ReportDetail(string.Format("--\t{0}\t[{1}]", disp.Data ?? "<null>", disp.Id.ToString("X")));
                    //else
                    //    ReportDetail(string.Format("-\t{0}\t[{1}]", disp.Data ?? "<null>", disp.Id.ToString("X")));

                }
            }
            var dbGraph = (graph.Source as DbGraph<IThing, ILink>);
            if(dbGraph!=null) {
                dbGraph.Flush();
                dbGraph.Close();
            }
        }

        [Test]
        public virtual void SearchSomeoneTest() {
            var graph = OpenFile(@"S:\temp\sample_4.pib"); //(@"E:\testdata\txbProjekt\ADRESSEXAMPLE.pib");
            // this does not work; find out how to search for nulls
            var someones = graph.GetByData(null);
            {
                foreach (var someone1 in someones)
                {
                    var disp1 = graph.ThingToDisplay(someone1);
                    ReportDetail(string.Format("{0}", disp1.Data ?? "<null>"));
                }
            }
            var someone = graph.GetByData("personen", false)
                .Where(e => e.Data.ToString().ToLower() == "personen")
                .FirstOrDefault();

            // another notation of the stuff above:
            var so2 = (from so in graph.GetByData("personen", false)
                     where so.Data.ToString().ToLower() == "personen"
                     select so).FirstOrDefault();

            ReportDetail("****** Exact result:");
            var disp = graph.ThingToDisplay(someone);
            ReportDetail(string.Format("{0}", disp.Data ?? "<null>"));

            ReportDetail("****** all leafs of exact result:");
            var links = graph.Edges(someone).Where(e=>e.Root==someone);
            foreach (var link in links) {
                
                var marker = graph.ThingToDisplay(link.Marker) ?? CommonSchema.EmptyMarker;
                disp = graph.ThingToDisplay(link.Leaf);
                ReportDetail(string.Format("[{0}]:{1}", marker.Data, disp.Data ?? "<null>"));
            }
            var id = unchecked((long)0x0D0A3C7B7F6A4D22);
            var emptything = graph.GetById(id);
            if(emptything != null)
            {
                var walker = new Walker<IThing, ILink>(graph);
                ReportDetail("********* walk emtyphting:");
                foreach (var sub in walker.Walk(emptything, 0)) {

                    var tabs = "".PadLeft(sub.Level, '\t');
                    if (sub.Node is ILink) {
                        var marker = graph.ThingToDisplay((sub.Node as ILink).Marker) ?? CommonSchema.EmptyMarker;
                        disp = graph.ThingToDisplay((sub.Node as ILink).Leaf);
                        ReportDetail(tabs + string.Format("[{0}]:{1}", marker.Data, disp.Data ?? "<null>"));
                    } else {
                        disp = graph.ThingToDisplay(sub.Node);
                        ReportDetail(tabs + string.Format("{0}", disp.Data ?? "<null>"));
                    }
                }
            }
        }
    }
}