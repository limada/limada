using System.Collections.Generic;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.View.Visuals;
using NUnit.Framework;
using Limaki.Graphs;
using System.Linq;

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
                yield return Mock.SampleFactory.Nodes[1]; // Person
                yield return Mock.SampleFactory.Nodes[2]; // John
                yield return Mock.SampleFactory.Nodes[3]; // City
                yield return Mock.SampleFactory.Nodes[4]; // Boston
                yield return Mock.SampleFactory.Nodes[5]; // Go
                yield return Mock.SampleFactory.Nodes[6]; // Bus
                yield return Mock.SampleFactory.Edges[1]; // [Person->John]
                yield return Mock.SampleFactory.Edges[2]; // [City->Boston]
                yield return Mock.SampleFactory.Edges[3]; // [[Person->John]->Go]
                yield return Mock.SampleFactory.Edges[4]; // [[[Person->John]->Go]->[City->Boston]]
                yield return Mock.SampleFactory.Edges[5]; // [[[[Person->John]->Go]->[City->Boston]]->Bus]
            }
        }

        public virtual IEnumerable<IVisual> PersonExpanded {
            get {

                yield return Mock.SampleFactory.Nodes[1]; // Person
                yield return Mock.SampleFactory.Nodes[2]; // John
                yield return Mock.SampleFactory.Nodes[5]; // Go
                yield return Mock.SampleFactory.Edges[1]; // [Person->John]
                yield return Mock.SampleFactory.Edges[3]; //[[Person->John]->Go]        

            }
        }

        [Test]
        public override void FullExpandTest() {
            base.FullExpandTest();
        }

        [Test]
        public void FullExandAndPerson() {
            this.FullExpandTest();
            this.Person();
        }

        [Test]
        public void Person() {
            Mock.Scene.Selected.Clear();
            Mock.SetFocused (Mock.SampleFactory.Nodes[1]); // Person
            Mock.SceneFacade.CollapseToFocused();

            Assert.AreEqual (Mock.Scene.Graph.Count, 1);
            Mock.AreEquivalent (new IVisual[] { Mock.SampleFactory.Nodes[1] }, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.Expand (false);

            Mock.AreEquivalent (PersonExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.SetFocused (Mock.SampleFactory.Nodes[5]); // Go
            Mock.SceneFacade.Expand (false);

            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            Mock.SceneFacade.RemoveOrphans = true;
            Mock.SceneFacade.Collapse();
            Mock.AreEquivalent (PersonExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

            this.ReportSummary();

        }

        bool edgeWithPerson = false;

        [Test]
        [Ignore ("expanding edges currently is not supported")]
        public void PersonJohnEdge() {
            if (edgeWithPerson) {
                Person ();
                Mock.Scene.Selected.Clear ();
            } else {
                Mock.Scene.ClearView ();
                var person = Mock.SampleFactory.Nodes[1];
                Mock.SetFocused (person); // Person

                Mock.SceneFacade.Expand (false);
                Mock.CommandsPerform ();
            }

            Mock.SetFocused (Mock.SampleFactory.Edges[1]); // [Person->John]
            Mock.SceneFacade.Expand (false);
            Mock.CommandsPerform ();

            Mock.ProveLocationNotZero (FullExpanded.ToArray ());
            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);

        }

        [Test]
        [Ignore ("expanding edges currently is not supported")]
        public void PersonJohnGoEdge() {

            if (edgeWithPerson) {
                Person ();
                Mock.Scene.Selected.Clear ();
            } else {
                Mock.Scene.ClearView ();
                Mock.SetFocused (Mock.SampleFactory.Nodes[1]); // Person

                Mock.SceneFacade.Expand (false);
                Mock.CommandsPerform ();
            }

            Mock.Scene.Selected.Clear();
            Mock.SetFocused (Mock.SampleFactory.Edges[3]); // [[Person->John]->Go]
            Mock.SceneFacade.Expand (false);
            Mock.CommandsPerform ();

            Mock.ReportScene(Mock.Scene);
            Mock.ProveLocationNotZero (FullExpanded.ToArray());
            Mock.AreEquivalent (FullExpanded, Mock.Scene.Graph);
            Mock.ProveShapes (Mock.Scene);


        }
        }
}