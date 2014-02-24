using System.Collections.Generic;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;
using Limaki.Graphs;

namespace Limaki.Tests.View.Visuals {

    public class GCJohnBostonSceneTest<TItem, TEdge> : SceneFacadeTest<TItem, TEdge, GCJohnBostonGraphFactory<TItem, TEdge>>
        where TEdge : IEdge<TItem>, TItem {

        public IEnumerable<IVisual> JohnGoBostonNodes {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // Person
                yield return Mock.SampleFactory.Nodes[2]; // John
                yield return Mock.SampleFactory.Nodes[3]; // City
                yield return Mock.SampleFactory.Nodes[4]; // Boston
                yield return Mock.SampleFactory.Nodes[5]; // Go
            }
        }

        public IEnumerable<IVisual> JohnGoBoston {
            get {
                yield return Mock.SampleFactory.Nodes[1]; // Person
                yield return Mock.SampleFactory.Nodes[2]; // John
                yield return Mock.SampleFactory.Nodes[3]; // City
                yield return Mock.SampleFactory.Nodes[4]; // Boston
                yield return Mock.SampleFactory.Nodes[5]; // Go
                yield return Mock.SampleFactory.Edges[1]; // [Person->John]
                yield return Mock.SampleFactory.Edges[2]; // [City->Boston]
                yield return Mock.SampleFactory.Edges[3]; // [[Person->John]->Go]
                yield return Mock.SampleFactory.Edges[4]; // [[[Person->John]->Go]->[City->Boston]]
            }
        }

        public override IEnumerable<IVisual> FullExpanded {
            get {
                return new IVisual[] {
                    Mock.SampleFactory.Nodes[1], // Person
                    Mock.SampleFactory.Nodes[2], // John
                    Mock.SampleFactory.Nodes[3], // City
                    Mock.SampleFactory.Nodes[4], // Boston
                    Mock.SampleFactory.Nodes[5], // Go
                    Mock.SampleFactory.Nodes[6], // Bus
                    Mock.SampleFactory.Edges[1], // [Person->John]
                    Mock.SampleFactory.Edges[2], // [City->Boston]
                    Mock.SampleFactory.Edges[3], // [[Person->John]->Go]
                    Mock.SampleFactory.Edges[4], // [[[Person->John]->Go]->[City->Boston]]
                    Mock.SampleFactory.Edges[5], // [[[[Person->John]->Go]->[City->Boston]]->Bus]
                };
            }
        }

        [Test]
        public override void FullExpandTest () {
            base.FullExpandTest ();
        }

        [Test]
        public void FullExandAndPerson () {
            this.FullExpandTest ();
            this.Person ();
        }

        [Test]
        public void Person () {
            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[1]); // Person
            Mock.SceneFacade.CollapseToFocused ();

            Assert.AreEqual (Mock.Scene.Graph.Count, 1);
            Mock.AreEquivalent (new IVisual[] { Mock.SampleFactory.Nodes[1] }, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);

            var personExpanded = new IVisual[] {
                Mock.SampleFactory.Nodes[1], // Person
                Mock.SampleFactory.Nodes[2], // John
                Mock.SampleFactory.Nodes[5], // Go
                Mock.SampleFactory.Edges[1], // [Person->John]
                Mock.SampleFactory.Edges[3], // [[Person->John]->Go]
            };

            Mock.AreEquivalent (personExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Nodes[5]); // Go
            Mock.SceneFacade.Expand (false);

            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.RemoveOrphans = true;
            Mock.SceneFacade.Collapse ();
            Mock.AreEquivalent (personExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.SampleFactory.Edges[1]); // [Person->John]
            Mock.SceneFacade.Expand (false);

            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            this.ReportSummary ();

        }
    }
}