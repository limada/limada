using Limada.Tests.Model;
using Limaki.Graphs;
using Limaki.Tests;
using Limaki.UnitTest;
//using Limaki.Data.db4o;
using NUnit.Framework;
using Limada.Model;
using Limaki.Data;
using Limada.Tests.Basic;
using Limaki.Tests.Graph.Model;
using Limada.Schemata;
using Id = System.Int64;

namespace Limada.Tests.ThingGraphs.SchemaGraph {
    public class SchemaGraphTestBase : ThingGraphTestBase {
        public override IThingGraph Graph {
            get {
                if (!(base.Graph is SchemaThingGraph)) {
                    base.Graph = new SchemaThingGraph(base.Graph);
                    SchemaFacade.MakeMarkersUnique(base.Graph);
                }

                return base.Graph;
            }
        }
    }

    public class SchemaGraphTest : SchemaGraphTestBase {
        public virtual void TestFindRoots(IThing described, IThing description, ILink descriptionLink) {
            foreach (IThing item in Graph.FindRoots(described)) {
                Assert.IsFalse(item.Equals(descriptionLink), descriptionLink.ToString());
                Assert.IsFalse(item.Equals(description), description.ToString());
            } 
        }

        public virtual void TestDescription(IThing described, IThing description, ILink descriptionLink) {
            Assert.IsTrue (Graph.Contains (described));
            //Assert.IsTrue(Graph.Contains(description));
            Assert.IsFalse(Graph.Contains(descriptionLink));

            foreach(ILink link in Graph.Edges(described)) {
                Assert.IsFalse (link.Equals (descriptionLink));
            }
            foreach (ILink link in Graph.Edges(description)) {
                Assert.IsFalse(link.Equals(descriptionLink));
            }
            
            SchemaThingGraph schemaGraph = Graph as SchemaThingGraph;
            IThing thing = schemaGraph.ThingToDisplay (described);
            Assert.AreEqual (description,thing);

            thing = schemaGraph.DescribedThing (description);
            Assert.AreEqual(described, thing);

            Walker<IThing, ILink> walker = new Walker<IThing, ILink>(Graph);
            foreach (LevelItem<IThing> item in walker.DeepWalk(described, 0)) {
                Assert.IsFalse(item.Node.Equals(descriptionLink));
                Assert.IsFalse(item.Node.Equals(description));
            }



        }

        [Test]
        public virtual void StandardGraphTest() {
            BasicThingGraphTest graphTest = new BasicThingGraphTest();
            graphTest.DoDetail = false; //this.DoDetail;

            graphTest.Graph = this.Graph;
            graphTest.Setup();
            graphTest.AllTests();
            graphTest.TearDown();

            this.Close();
            ReportSummary();
        }

        [Test]
        public virtual void DescriptionTest() {
            var factory = new DescriptionSampleFactory ();
            factory.Graph = this.Graph;
            factory.Populate ();

            var graph = this.Graph as SchemaThingGraph;
            graph.Initialize ();


            TestDescription (factory.Nodes[1], factory.Nodes[2], factory.Edges[1]);
            TestDescription(factory.Nodes[3], factory.Nodes[4], factory.Edges[2]);
            
            
            TestFindRoots(factory.Nodes[1], factory.Nodes[2], factory.Edges[1]);
            ReportSummary();
        }

        [Test]
        public virtual void FindRootsTest() {
            if (Graph != null) {
                foreach (IThing item in Graph.FindRoots(null)) {
                    if (!Graph.IsMarker(item))
                        ;
                }
            }
            ReportSummary();
        }


        [Test]
        public virtual void DocumentSchemaTest() {
            var factory = new DigidocSampleFactory();
            factory.Graph = this.Graph;
            factory.Populate();
            ((SchemaThingGraph)Graph).Initialize();

            TestDescription(factory.Nodes[1], factory.Nodes[2], factory.Edges[1]);
            TestDescription(factory.Nodes[3], factory.Nodes[4], factory.Edges[3]);
            ReportSummary();
        }

        
    }
}
