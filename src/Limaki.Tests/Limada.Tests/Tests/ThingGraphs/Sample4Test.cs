using Limada.Model;
using Limaki.Tests;
using System;
using Limada.Data;
using Limaki.Data;
using Limaki.Common;
using Limaki;
using Limada.Usecases;
using NUnit.Framework;
using Limada.Schemata;
using System.Linq;
using Limaki.Model.Content;
using System.Collections.Generic;
using Limaki.Graphs.Extensions;
using Limaki.Common.IOC;
using Limaki.Contents.IO;
using System.IO;
using Limaki.Graphs;

namespace Limada.Tests.ThingGraphs {
    [TestFixture]
    public class Sample4Test : DomainTest {
        protected IThingFactory Factory {
            get { return Registry.Factory.Create<IThingFactory>(); }
        }

        public ThingGraphIo ThingGraphProvider { get; set; }

        public virtual SchemaThingGraph OpenFile(string fileName) {
            if (ThingGraphProvider != null) {
                ThingGraphProvider.Close(GraphContent);
                ThingGraphProvider = null;
            }
            if (ThingGraphProvider == null) {
                var fileManager = new ThingGraphIoManager();
                var file = Iori.FromFileName(fileName);
                var provider = fileManager.GetSinkIO(file, IoMode.Read);
                if (provider != null) {
                    ThingGraphProvider = provider as ThingGraphIo;
                    GraphContent = ThingGraphProvider.Open(file);
                    return new SchemaThingGraph(GraphContent.Data);
                }
            } else {
                throw new Exception("File " + fileName + " could not be opened");
            }

            return null;
        }


        public virtual void Close() {
            if (ThingGraphProvider != null) {
                ThingGraphProvider.Close(GraphContent);
            }
        }


        public virtual void OnFlush(IThingGraph graph) {
            if (ThingGraphProvider != null) {
                if (ThingGraphProvider.IoMode == IoMode.Write)
                    ThingGraphProvider.Flush(GraphContent);
            }
        }
        
        [TestFixtureTearDown]
        public override void TearDown() {
            Close();
            base.TearDown();
        }

        public string SampleFile {
            get {
                if (true)
                    return TestLocations.Sample4_pib;
                else
                    return TestLocations.AdressExample_pib;
            }
        }

        public IEnumerable<IThing> FindRoot(IThingGraph source, bool doAutoView) {
            source = source.Unwrap() as IThingGraph;
            var result = new List<IThing>();
            var topic = source.GetById(TopicSchema.Topics.Id);
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
                return source.FindRoots(null).Where(item=>!source.IsMarker(item));
            }
            return result;
        }


        [Test]
        public virtual void WalkThrouFirstLevelTest() {
            var graph = OpenFile(SampleFile);
            
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

            var graph = OpenFile(SampleFile);
            new ThingGraphMaintenance ().CleanWrongDocuments (graph, true);
        }

        [Test]
        public virtual void SearchSomeoneTest() {
            var graph = OpenFile(SampleFile);
            // this does not work; find out how to search for nulls
            var someones = graph.GetByData(null);
            {
                foreach (var someone1 in someones)
                {
                    var disp1 = graph.ThingToDisplay(someone1);
                    ReportDetail(string.Format("{0}", disp1.Data ?? "<null>"));
                }
            }
            var someone = graph.GetByData(TestLocations.Sample4_Persons, false)
                .Where(e => e.Data.ToString().ToLower() == TestLocations.Sample4_Persons)
                .FirstOrDefault();

            // another notation of the stuff above:
            var so2 = (from so in graph.GetByData(TestLocations.Sample4_Persons, false)
                       where so.Data.ToString().ToLower() == TestLocations.Sample4_Persons
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
            var id = TestLocations.Sample4_Node1;
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

        public ThingGraphContent GraphContent { get; set; }
    }
}