using System;
using Limada.Model;
using Limada.Schemata;
using Limada.Tests.Model;
using Limaki.Graphs;
using NUnit.Framework;
using Id = System.Int64;

namespace Limada.Tests.ThingGraphs.SchemaGraph {
    public class SchemaGraphPerformanceTest : SchemaGraphTestBase {
        public int Count = 50;
        
        private Id _rootId = 0;
        protected long rootId {
            get {return _rootId;}
            set { _rootId = value; }
        }


        Id testMarkerId = 0x203806532CE53439;
        IThing TestMarker {
            get {
                IThing result = Graph.GetById (testMarkerId);
                if (result == null) {
                    var thing = Factory.CreateItem(testMarkerId,"performancemarker");
                    result = thing;
                }
                return result;
            }
        }

        protected IThing GetRoot() {
            IThing root = null;
            if (rootId == 0) {
                IThing topic = Graph.GetById(TopicSchema.Topics.Id);
                if (topic == null)
                    return null;
                foreach (ILink link in Graph.Edges(topic)) {
                    if (link.Marker.Id == testMarkerId) {
                        return link.Leaf;
                    }
                }
            } else {
                root = Graph.GetById(rootId);
            }
            return root;
        }

        [Test]
        public void WriteDescriptionTest() {
            
            ReportDetail ("Writing");
            var factory = new DescriptionSampleFactory();
            factory.Graph = this.Graph;
            factory.Count = Count;
            factory.TestMarker = this.TestMarker;
            factory.Populate();

            this.rootId = factory.Root.Id;
            factory=null;

            Close ();
            ReportSummary("Writes");


        }
        
        [Test]
        public void ReadDescriptionTest() {
            IThing root = GetRoot ();
            if (root == null)
                WriteDescriptionTest ();

            root = GetRoot ();

            ReportDetail("Reading");
            this.Tickers.Start();
            var schemaGraph = this.Graph as SchemaThingGraph;
            var view = new SubGraph<IThing, ILink>(this.Graph, new Graph<IThing, ILink>());
            var facade = new SubGraphWorker<IThing, ILink>(view);

            view.Add(root);

            int iCount = 0;
            foreach (IThing thing in facade.Expand(new IThing[] { root }, false)) {
                if (!(thing is ILink)) {
                    var thingToDisplay = schemaGraph.ThingToDisplay(thing);
                    iCount++;
                }
            }
            ReportSummary("Reads \tCount \t" + iCount);
        }
    }
}