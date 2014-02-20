using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.Tests.Graph.Wrappers;
using Limaki.View.Layout;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.View.Visuals {

    public class SceneFacadeTestWrapper<TFactory> : SceneFacadeTestWrapper<IGraphEntity, IGraphEdge, TFactory>
        where TFactory : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new () {

        public SceneFacadeTestWrapper (SceneFacadeTest<IGraphEntity, IGraphEdge, TFactory> test):base(test){ }
        }

    public class SceneFacadeTestWrapper<IGraphEntity, IGraphEdge,TFactory>
        where IGraphEdge : IEdge<IGraphEntity>, IGraphEntity
        where TFactory : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new () {

        /// <summary>
        /// sets Scene.Focus to item and 
        /// Expands(deep) it
        /// item is added if not in view
        /// </summary>
        /// <param name="item"></param>
        /// <param name="deep"></param>
        public void Expand (IVisual item, bool deep) {
            Scene.Selected.Clear ();
            SetFocused (item);
            SceneFacade.Expand (deep);
            Test.CommandsPerform ();
        }

        public IVisual ChangeLink (IVisualEdge edge, IVisual item, bool root) {

            var newItem = item;
            var oldItem = root ? edge.Root : edge.Leaf;

            Scene.ChangeEdge (edge, newItem, root);
            Scene.Graph.OnGraphChanged (edge, GraphEventType.Update);

            Scene.Requests.Add (new LayoutCommand<IVisual> (edge, LayoutActionType.Justify));
            foreach (var twig in Scene.Twig (edge)) {
                Scene.Requests.Add (new LayoutCommand<IVisual> (twig, LayoutActionType.Justify));
            }

            Test.CommandsPerform ();

            return oldItem;
        }

        public void ProoveChangedLink (IVisualEdge edge, IVisual newItem, IVisual oldItem, bool root) {
            ProoveChangedLink (edge, newItem, oldItem, root, true);
        }

        public void ProoveChangedLink (IVisualEdge edge, IVisual newItem, IVisual oldItem, bool root, bool inView) {
            
            Assert.IsNotNull (edge);
            Assert.AreSame (root ? edge.Root : edge.Leaf, newItem);
            Assert.AreNotSame (root ? edge.Root : edge.Leaf, oldItem);

            if (inView) {
                Assert.IsTrue (View.Edges (newItem).Contains (edge));
                Assert.IsFalse (View.Edges (oldItem).Contains (edge));
            } else {
                Assert.IsTrue (Graph.Edges (newItem).Contains (edge));
                Assert.IsFalse (Graph.Edges (oldItem).Contains (edge));
            }
        }

        /// <summary>
        /// sets Scene.Focus to item and 
        /// calls Layout.Perform and Layout.AdjustSize
        /// item is added if not in view
        /// </summary>
        public void SetFocused (IVisual item) {
            Mock.SetFocused (item);
        }

        public void EnsureShape (IVisual item) {
            Mock.EnsureShape (item);
        }

        /// <summary>
        /// make a new link
        /// add it to Scene
        /// call View.OnGraphChanged 
        /// </summary>
        /// <param name="root"></param>
        /// <param name="leaf"></param>
        /// <returns></returns>
        public IVisualEdge AddEdge (IVisual root, IVisual leaf) {

            var sourceEdge = new VisualEdge<string> ("", root, leaf);
            sourceEdge.Data = GraphExtensions.EdgeString<IVisual, IVisualEdge> (sourceEdge);

            Scene.Add (sourceEdge);
            Scene.Graph.OnGraphChanged (sourceEdge, GraphEventType.Add);
            return sourceEdge;
        }

        public void RemoveEdge (IVisualEdge edge) {
            this.Scene.Graph.OnGraphChanged (edge, GraphEventType.Remove);
            this.Scene.Remove (edge);
        }

        public void ProveContains (IGraph<IVisual, IVisualEdge> graph, params IVisual[] visuals) {
            foreach (var item in visuals)
                if (item is IVisualEdge)
                    Assert.IsTrue (graph.Contains ((IVisualEdge) item));
                else
                    Assert.IsTrue (graph.Contains (item));
        }

        public void ProveNotContains (IGraph<IVisual, IVisualEdge> graph, params IVisual[] visuals) {
            foreach (var item in visuals)
                if (item is IVisualEdge)
                    Assert.IsFalse (graph.Contains ((IVisualEdge) item));
                else
                    Assert.IsFalse (graph.Contains (item));
        }

        /// <summary>
        /// tests if View.Sink contains visuals
        /// </summary>
        /// <param name="visuals"></param>
        public void ProveViewContains (params IVisual[] visuals) {
            ProveContains (this.View.Sink, visuals);
        }

        public void ProveViewNotContains (params IVisual[] visuals) {
            ProveNotContains (this.View.Sink, visuals);
        }

        public SceneFacadeTestWrapper (SceneFacadeTest<IGraphEntity, IGraphEdge, TFactory> test) {

            this.Mock = test.Mock;
            this.Test = test;

            Reset ();
        }

        public void Reset() {
            this.Graph =
                Mock.Scene.Graph.RootSource ().Source
                as IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>;

            this.View =
                Mock.Scene.Graph
                as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;

            this.Nodes = Mock.Factory.Nodes;
            this.Edges = Mock.Factory.Edges;

            this.Scene = Mock.Scene;

            this.SceneFacade = Mock.SceneFacade;
            this.Display = Mock.Display;
        }

        public IGraphScene<IVisual, IVisualEdge> Scene { get; protected set; }

        public GraphSceneFacade<IVisual, IVisualEdge> SceneFacade { get; protected set; }

        public IGraphSceneDisplay<IVisual, IVisualEdge> Display { get; protected set; }

        public IList<IVisual> Nodes { get; protected set; }

        public IList<IVisualEdge> Edges { get; protected set; }

        /// <summary>
        /// Scene.Graph as <see cref="IGraphPair{IVisual, IVisual, IVisualEdge, IVisualEdge}"/>
        /// </summary>
        public IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> View { get; protected set; }

        /// <summary>
        /// Scene.Graph.RootSource().Source as <see cref="IGraphPair{IVisual, IGraphEntity, IVisualEdge, IGraphEdge}"/>
        /// </summary>
        public IGraphPair<IVisual, IGraphEntity, IVisualEdge, IGraphEdge> Graph { get; protected set; }

        public SceneFacadeTest<IGraphEntity, IGraphEdge, TFactory> Test { get; protected set; }

        public TestSceneMock<IGraphEntity, IGraphEdge, TFactory> Mock { get; protected set; }

        public T TestAs<T> () where T : SceneFacadeTest<IGraphEntity, IGraphEdge, TFactory> {
            return this.Test as T;
        }

        
    }
}