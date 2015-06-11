/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 20012-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Model;
using Limaki.Tests.Graph.Model;
using Limaki.View;
using Limaki.View.GraphScene;
using System;
using System.Linq;
using System.Collections.Generic;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend.Viz;
using Xwt;
using NUnit.Framework;
using Limaki.Common.Collections;
using System.IO;

namespace Limaki.Tests.View.Visuals {

    public class SceneTestEnvironment<TItem, TEdge, TFactory> : SceneTestEnvironment<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem
        where TFactory : ISampleGraphFactory<TItem, TEdge>, new () {
        public SceneTestEnvironment () : base (new SampleSceneFactory<TItem, TEdge, TFactory> ()) { }


    }

    public class SceneTestEnvironment<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public SceneTestEnvironment(ISampleGraphSceneFactory sampleFactory) {
            this.SampleFactory = sampleFactory;
        }

        public virtual ISampleGraphSceneFactory SampleFactory { get; set; }

        protected IGraphScene<IVisual, IVisualEdge> _scene;
        public virtual IGraphScene<IVisual, IVisualEdge> Scene {
            get {
                EnsureScene();
                return _scene;
            }
            set {
                _scene = value;
                if (_display != null) {
                    _display.Data = value;
                }
            }
        }

        public void EnsureScene () {
            if (_scene == null) {
                var s = this.SampleFactory.NewScene ();
                var g = new SubGraph<IVisual, IVisualEdge> (
                    s.Graph,
                    new VisualGraph ());
                _scene = new Scene ();
                _scene.Graph = g;
                if (_display != null) {
                    _display.Data = _scene;
                }
                _sceneFacade = null;
            }
        }

        IGraphSceneDisplay<IVisual, IVisualEdge> _display = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display {
            get { return _display ?? (_display = new VisualsDisplay { Data = this.Scene }); }
            set { _display = value; }
        }

        protected GraphSceneFacade<IVisual, IVisualEdge> _sceneFacade;
        public virtual GraphSceneFacade<IVisual, IVisualEdge> SceneFacade {
            get {
                return _sceneFacade ?? (_sceneFacade =
                    new GraphSceneFacade<IVisual, IVisualEdge> (() => this.Scene, Display.Layout) {
                        RemoveOrphans = false
                    });
            }
        }

        public virtual void Clear () {
            _scene = null;
            _sceneFacade = null;
            _display = null;
        }

        #region accessor facade

        public IList<IVisual> Nodes { get { EnsureScene (); return SampleFactory.Nodes; } }

        public IList<IVisualEdge> Edges { get { EnsureScene (); return SampleFactory.Edges; } }

        /// <summary>
        /// Scene.Graph as <see cref="IGraphPair{IVisual, IVisual, IVisualEdge, IVisualEdge}"/>
        /// </summary>
        public IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> View {
            get { return Scene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>; }
        }

        /// <summary>
        /// Scene.Graph.RootSource().Source as <see cref="IGraphPair{IVisual, IGraphEntity, IVisualEdge, IGraphEdge}"/>
        /// </summary>
        public IGraphPair<IVisual, TItem, IVisualEdge, TEdge> Source {
            get { return Scene.Graph.RootSource ().Source as IGraphPair<IVisual, TItem, IVisualEdge, TEdge>; }
        }

        #endregion

        #region method facade

        /// <summary>
        /// sets Scene.Focus to item and 
        /// calls Layout.Perform and Layout.AdjustSize
        /// item is added if not in view
        /// </summary>
        public void SetFocused (IVisual item) {
            this.Scene.Selected.Clear ();
            this.Scene.Focused = item;
            EnsureShape (item);
            this.Scene.AddBounds (item);
        }

        public void EnsureShape (IVisual item) {
            this.Display.Layout.Perform (item);
            this.Display.Layout.AdjustSize (item);
        }

        public void CommandsPerform () {
            this.Display.Perform ();
            this.Scene.Requests.Clear ();
        }

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
            CommandsPerform ();
        }

        public IVisual ChangeLink (IVisualEdge edge, IVisual item, bool root) {

            var newItem = item;
            var oldItem = root ? edge.Root : edge.Leaf;

            Scene.ChangeEdge (edge, newItem, root);
            Scene.Graph.OnGraphChange (edge, GraphEventType.Update);

            Scene.Requests.Add (new LayoutCommand<IVisual> (edge, LayoutActionType.Justify));
            foreach (var twig in Scene.Twig (edge)) {
                Scene.Requests.Add (new LayoutCommand<IVisual> (twig, LayoutActionType.Justify));
            }

            CommandsPerform ();

            return oldItem;
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
            Scene.Graph.OnGraphChange (sourceEdge, GraphEventType.Add);
            return sourceEdge;
        }

        public void RemoveEdge (IVisualEdge edge) {
            this.Scene.Graph.OnGraphChange (edge, GraphEventType.Remove);
            this.Scene.Remove (edge);
        }

        public static SceneTestEnvironment<TItem, TEdge>[] Create<TFactory> (int count) where TFactory : ISampleGraphFactory<TItem, TEdge>, new () {
            var tests = new SceneTestEnvironment<TItem, TEdge, TFactory>[count];
            var i = 0;
            tests.ForEach (t => {
                var test = new SceneTestEnvironment<TItem, TEdge, TFactory>();
                tests[i++] = test;
            });
            return tests;
        }

        #endregion

        #region Proves

        public void AreEquivalent (IEnumerable<IVisual> visuals, IGraph<IVisual, IVisualEdge> graph) {

            foreach (var visual in visuals) {
                string s = "graph.Contains( " + visual.Data.ToString () + " )";
                if (visual is IVisualEdge) {
                    Assert.IsTrue (graph.Contains ((IVisualEdge)visual), s);
                } else {
                    Assert.IsTrue (graph.Contains (visual), s);
                }
            }

            var visualsCollection = visuals.ToArray();
            foreach (var visual in graph) {
                var s = "visuals.Contains( " + visual.Data.ToString () + " )";
                Assert.IsTrue (visualsCollection.Contains (visual), s);
            }
        }

        public void ProveShapes () {
            ProveShapes (this.Scene);
        }

        public void ProveShapes (IGraphScene<IVisual, IVisualEdge> scene) {
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

        public void ProveChangedEdge (IVisualEdge edge, IVisual newItem, IVisual oldItem, bool root) {
            ProveChangedEdge (edge, newItem, oldItem, root, true);
        }

        public void ProveEdgesContainRootLeaf (IGraph<IVisual, IVisualEdge> graph) {
            foreach (var edge in graph.Edges ()) {
                if (edge.Root is IVisualEdge)
                    Assert.IsTrue (graph.Contains ((IVisualEdge)edge.Root));
                else
                    Assert.IsTrue (graph.Contains (edge.Root));

                if (edge.Leaf is IVisualEdge)
                    Assert.IsTrue (graph.Contains ((IVisualEdge)edge.Leaf));
                else
                    Assert.IsTrue (graph.Contains (edge.Leaf));
            }

        }

        public void ProveLocationNotZero (params IVisual[] visuals) {
            foreach (var item in visuals) {
                Assert.AreNotEqual (item.Location, Point.Zero,"{0} has a zero location",item.Data);
            }
        }

        public void ProveChangedEdge (IVisualEdge edge, IVisual newItem, IVisual oldItem, bool root, bool inView) {

            Assert.IsNotNull (edge);
            Assert.AreSame (root ? edge.Root : edge.Leaf, newItem);
            Assert.AreNotSame (root ? edge.Root : edge.Leaf, oldItem);

            if (inView) {
                Assert.IsTrue (View.Edges (newItem).Contains (edge));
                Assert.IsFalse (View.Edges (oldItem).Contains (edge));
            } else {
                Assert.IsTrue (Source.Edges (newItem).Contains (edge));
                Assert.IsFalse (Source.Edges (oldItem).Contains (edge));
            }
        }

        public void ProveContains<TItem, TEdge> (IGraph<TItem, TEdge> graph, params TItem[] items)
        where TEdge : IEdge<TItem> {
            foreach (var item in items) {
                var m = string.Format ("contains {0}", item);
                if (item is TEdge)
                    Assert.IsTrue (graph.Contains ((TEdge) (object) item), m);
                else
                    Assert.IsTrue (graph.Contains (item), m);
            }
        }

        public void ProveNotContains<TItem, TEdge> (IGraph<TItem, TEdge> graph, params TItem[] items)
         where TEdge : IEdge<TItem> {

            foreach (var item in items) {
                var m = string.Format ("contains {0}", item);
                if (item is TEdge)
                    Assert.IsFalse (graph.Contains ((TEdge) (object) item), m);
                else
                    Assert.IsFalse (graph.Contains (item), m);
            }
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

        IContextWriter _reportPainter = null;
        public virtual IContextWriter ReportPainter { get { return _reportPainter ?? (_reportPainter = Registry.Factory.Create<IContextWriter> ()); } }
        public virtual void ReportScene(IGraphScene<IVisual,IVisualEdge> scene) {

            var engine = ReportPainter.Switch();

            var worker = new GraphSceneContextVisualizer<IVisual, IVisualEdge> () {
                Folder = this.SceneFacade,
                //Layout = this.Display.Layout,
                //StyleSheet = this.Display.StyleSheet
            };
            worker.Compose (scene, new VisualsRenderer ());
            worker.Modeller.Perform ();
            worker.Modeller.Finish ();
            ReportPainter.PushPaint (ctx => worker.Painter.Paint (ctx));
            using (var file = File.Create (this.GetType().Name+".html")) {
                ReportPainter.Write (file);
            }
            ReportPainter.Restore (engine);
        }
        #endregion

       
    }
}