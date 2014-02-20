using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.View.Visuals;
using Limaki.View.UI.GraphScene;
using Limaki.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Wrappers {

    public abstract class SceneFacadeTest<TFactory> : SceneFacadeTest<IGraphEntity, IGraphEdge, TFactory>
        where TFactory : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new () {
    }

    public abstract class SceneFacadeTest<TItem, TEdge, TFactory> : DomainTest
        where TEdge : IEdge<TItem>, TItem
        where TFactory : ISampleGraphFactory<TItem, TEdge>, new () {

        public void AreEquivalent (IEnumerable<IVisual> visuals, IGraph<IVisual, IVisualEdge> graph) {

            foreach (var visual in visuals) {
                string s = "graph.Contains( " + visual.Data.ToString () + " )";
                if (visual is IVisualEdge) {
                    Assert.IsTrue (graph.Contains ((IVisualEdge)visual), s);
                } else {
                    Assert.IsTrue (graph.Contains (visual), s);
                }
            }
            var visualsCollection = new List<IVisual> (visuals);
            foreach (var visual in graph) {
                var s = "visuals.Contains( " + visual.Data.ToString () + " )";
                Assert.IsTrue (visualsCollection.Contains (visual), s);
            }
        }

        public bool AlwaysInvoke = false;
        bool invoked = false;

        public void CommandsPerform () {
            Mock.Display.Perform ();
            Mock.Scene.Requests.Clear ();
        }

        public void ProoveShapes (IGraphScene<IVisual, IVisualEdge> scene) {
            CommandsPerform ();
            var indexList = new Set<IVisual> ();
            foreach (var visual in scene.SpatialIndex.Query ()) {
                if (!indexList.Contains (visual)) {
                    indexList.Add (visual);
                } else {
                    Assert.Fail (visual + " two times in SpatialIndex");
                }
                bool found = false;
                if (visual is IVisualEdge)
                    found = scene.Contains ((IVisualEdge)visual);
                else
                    found = scene.Contains (visual);

                Assert.IsTrue (found,
                               "to much items in SpatialIndex: ! scene.Contains ( " + visual.ToString () + " ) of Spatialindex");
            }

            foreach (var visual in scene.Graph) {
                if (visual.Shape != null)
                    Assert.IsTrue (indexList.Contains (visual),
                                   "to less items in SpatialIndex: ! SpatialIndex.Contains ( " + visual.ToString () + " ) of scene.Graph");
            }
        }

        protected TestSceneMock<TItem, TEdge, TFactory> _mock = null;
        public virtual TestSceneMock<TItem, TEdge, TFactory> Mock {
            get {
                if (_mock == null) {
                    _mock = new TestSceneMock<TItem, TEdge, TFactory> ();
                }
                return _mock;
            }
            set { _mock = value; }
        }

        public abstract IEnumerable<IVisual> FullExpanded { get; }

        public virtual IEnumerable<IVisual> FirstNode {
            get { yield return Mock.Factory.Nodes[1]; }
        }

        [Test]
        public void InvokeTest () {
            CommandsPerform ();
            Mock.Display.Reset ();
            ProoveShapes (Mock.Scene);
        }

        [Test]
        public virtual void FullExpandTest () {
            CommandsPerform ();
            Mock.Reset ();
            Mock.Scene.Selected.Clear ();
            Mock.SetFocused (Mock.Factory.Nodes[1]);
            Mock.SceneFacade.Expand (true);
            AreEquivalent (FullExpanded, Mock.Scene.Graph);
            ProoveShapes (Mock.Scene);
            this.ReportSummary ();
        }

        public SceneFacadeTestWrapper<TItem, TEdge, TFactory> Wrap () {
            return new SceneFacadeTestWrapper<TItem, TEdge, TFactory> (this);
        }

        public SceneFacadeTestWrapper<TItem, TEdge, TFactory>[] Wraps (int count) {
            var tests = new SceneFacadeTestWrapper<TItem, TEdge, TFactory>[count];
            var i = 0;
            tests.ForEach (t => {
                var test = Activator.CreateInstance (this.GetType ()) as
                           SceneFacadeTest<TItem, TEdge, TFactory>;
                tests[i++] = test.Wrap ();
            });
            return tests;
        }
    }
}