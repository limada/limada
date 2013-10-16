/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.Layout;
using Xwt;

namespace Limaki.View.UI.GraphScene {
    /// <summary>
    /// Provides a simplified interface to
    /// a scene, a layout and the scenes graph
    /// where scene.graph is a SubGraph
    /// </summary>
    public class GraphSceneFacade<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public GraphSceneFacade (Func<IGraphScene<TItem, TEdge>> sceneHandler, IGraphSceneLayout<TItem, TEdge> layout) {
            this.SceneHandler = sceneHandler;
            this.Layout = layout;
        }

        Func<IGraphScene<TItem, TEdge>> SceneHandler = null;
        IGraphScene<TItem, TEdge> _scene = null;
        public virtual IGraphScene<TItem, TEdge> Scene {
            get {
                IGraphScene<TItem, TEdge> scene = _scene;

                if (SceneHandler != null)
                    scene = SceneHandler ();

                if (scene != _scene) {
                    Clear ();
                    _scene = scene;
                }
                return _scene;
            }
        }


        private IGraph<TItem, TEdge> _graph = null;
        /// <summary>
        /// the data-graph of the SubGraph
        /// </summary>
        public IGraph<TItem, TEdge> Graph {
            get {
                var graph = _graph;
                var scene = this.Scene;
                if (scene.Graph is SubGraph<TItem, TEdge>) {
                    _graph = ((SubGraph<TItem, TEdge>) scene.Graph).Source;
                } else {
                    _graph = scene.Graph;
                }
                if (graph != _graph) {
                    if (_graph is IFactoryListener<TItem>) {
                        ((IFactoryListener<TItem>) _graph).ItemCreated += w => {
                            if (!CreatedItems.Contains (w))
                                CreatedItems.Add (w);
                        };
                    }
                }
                return _graph;
            }
        }

        private IGraph<TItem, TEdge> _view = null;
        /// <summary>
        /// the Graph containing the filtered items
        /// SubGraph.View will be set to view
        /// </summary>
        protected IGraph<TItem, TEdge> View {
            get {
                if (this.Scene.Graph is SubGraph<TItem, TEdge>) {
                    _view = ((SubGraph<TItem, TEdge>) this.Scene.Graph).Sink;
                } else if (_view == null) {
                    _view = new Graph<TItem, TEdge> ();
                }
                return _view;
            }
        }

        private SubGraph<TItem, TEdge> _subGraph = null;
        /// <summary>
        /// the SubGraph contains:
        /// View: the filtered graph
        /// Data: the original, full, unfiltered graph 
        /// </summary>
        protected SubGraph<TItem, TEdge> SubGraph {
            get {
                if (this.Scene.Graph is SubGraph<TItem, TEdge>) {
                    _subGraph = (SubGraph<TItem, TEdge>) this.Scene.Graph;
                    IsFiltered = true;
                } else if (_subGraph == null) {
                    _subGraph = new SubGraph<TItem, TEdge> (this.Graph, this.View);
                }

                return _subGraph;
            }
        }

        public IGraphSceneLayout<TItem, TEdge> Layout { get; set; }

        public virtual void Clear () {
            this._scene = null;
            this._view = null;
            this._graph = null;
            this._subGraph = null;
            CreatedItems.Clear ();
        }

        /// <summary>
        /// a list containing the visuals which are newly created in this session
        /// </summary>
        public ICollection<TItem> CreatedItems = new Set<TItem> ();

        public bool IsFiltered = false;
        public virtual void ApplyFilter () {
            if (Scene.Graph != SubGraph) {
                TItem curr = Scene.Focused;
                Scene.Graph = SubGraph;
                Scene.Focused = curr;
            }
            IsFiltered = true;
        }

        public virtual void RestoreFocused (TItem curr) {
            Scene.Focused = curr;
            Scene.Selected.Add (curr);
        }

        public virtual DataComparer<TItem> OrderBy { get; set; }

        protected virtual Aligner<TItem, TEdge> CreateAligner (IGraphScene<TItem, TEdge> data) {
            var result = new Aligner<TItem, TEdge>(data, this.Layout);
            return result;
        }

        /// <summary>
        /// adds item to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pt"></param>
        public virtual void Add (TItem item, Point pt) {
            IGraphScene<TItem, TEdge> scene = this.Scene;
            if (scene == null || item == null)
                return;
            ApplyFilter ();
            var curr = scene.Focused;

            var affected = new SubGraphWorker<TItem, TEdge> (this.SubGraph).Add (item);

            var aligner = CreateAligner(scene);
            aligner.Locator.Justify(item);
            var options = Layout.Options();
            var bounds = aligner.NearestNextFreeSpace(pt, aligner.Locator.GetSize(item), new TItem[]{item}, true, options.Dimension, options.Distance);
            aligner.Locator.SetLocation(item, bounds.Location);
            aligner.AffectedEdges(new TItem[]{item});
            aligner.Commit ();

            RestoreFocused (curr);

        }

        /// <summary>
        /// adds items to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pt"></param>
        public virtual void Add (IEnumerable<TItem> elements, bool justify, bool arrange) {
            IGraphScene<TItem, TEdge> scene = this.Scene;
            ApplyFilter();
            TItem curr = scene.Focused;

            var affected = new SubGraphWorker<TItem, TEdge>(this.SubGraph)
                .Add(elements);

            if (justify || arrange) {
                var aligner = CreateAligner(scene);
                var options = Layout.Options();
                if (arrange) {
                    options.Collisions = Collisions.NextFree | Collisions.Toggle;
                    aligner.Columns(affected, options);

                } else if (justify) {
                    aligner.Justify(affected);
                }
                aligner.Commit();
            }

            RestoreFocused(curr);

        }

        public virtual void AddRaw (IEnumerable<TItem> elements) {
            IGraphScene<TItem, TEdge> scene = this.Scene;
            ApplyFilter ();
            TItem curr = scene.Focused;

            var affected = new SubGraphWorker<TItem, TEdge> (this.SubGraph)
                .Add (elements);

        }

        public class LevelItemComparer<T>:Comparer<LevelItem<T>> {
            public virtual DataComparer<T> OrderBy { get; set; }
            public override int Compare (LevelItem<T> x, LevelItem<T> y) {
                if(x.Level==y.Level) {
                    return OrderBy.Compare(x.Node, y.Node);
                } else {
                    return x.Level.CompareTo(y.Level);
                }
            }
        }
        public virtual void Expand (bool deep) {
            var scene = this.Scene;
            if (scene.Selected.Count > 0) {
                ApplyFilter ();
                var roots = scene.Selected.Elements;
                
                TItem curr = scene.Focused;
                var affected = new SubGraphWorker<TItem, TEdge>
                    (this.SubGraph).Expand(roots, deep);

                var aligner = CreateAligner(scene);
                var options = Layout.Options();

                var walker = new Walker<TItem, TEdge>(this.SubGraph);
                
                roots.ForEach(root => {
                    affected.Add(root);
                    var walk = (deep ? walker.DeepWalk(root, 1) : walker.ExpandWalk(root, 1))
                            .Where(l => !(l.Node is TEdge) && affected.Contains(l.Node))
                            ;// && !root.Equals(l.Node));
                    walk = walk.ToArray();
                    if (OrderBy != null)
                        walk = walk.OrderBy(l => l, new LevelItemComparer<TItem>{OrderBy = this.OrderBy}).ToArray();
                    
                    var bounds = new Rectangle(aligner.Locator.GetLocation(root), aligner.Locator.GetSize(root));
                    options.Collisions = Collisions.None;
                    var cols = aligner.MeasureWalk(walk, ref bounds, options);
                    aligner.DequeColumn(cols, ref bounds, options);
                    options.Collisions = Collisions.NextFree | Collisions.PerColumn | Collisions.Toggle;
                    aligner.LocateColumns(cols, ref bounds, options);
                });

                aligner.Commit();

                RestoreFocused (curr);

            }
        }

        public bool RemoveOrhpans { get; set; }

        public virtual void Collapse () {
            if (Scene.Selected.Count > 0) {
                ApplyFilter ();
                var scene = this.Scene;
                TItem curr = scene.Focused;
                var affected = new SubGraphWorker<TItem, TEdge> (this.SubGraph) { RemoveOrphans = this.RemoveOrhpans }
                    .Collapse (scene.Selected.Elements);
                UpdateRemoved (affected);

                RestoreFocused (curr);

            }
        }

        public virtual void CollapseToFocused () {
            if (Scene.Focused != null) {
                ApplyFilter ();
                TItem curr = Scene.Focused;

                UpdateRemoved (new SubGraphWorker<TItem, TEdge> (this.SubGraph)
                    .CollapseToFocused (Scene.Selected.Elements));

                RestoreFocused (curr);
            }
        }

        public virtual void Hide () {
            if (Scene.Selected.Count > 0) {
                ApplyFilter ();

                UpdateRemoved (new SubGraphWorker<TItem, TEdge> (this.SubGraph)
                    .Hide (Scene.Selected.Elements));

                Scene.Selected.Clear ();
                Scene.Focused = default(TItem);
                Scene.Hovered = default(TItem);


            }
        }

        public virtual void UpdateRemoved (ICollection<TItem> removed) {
            foreach (TItem remove in removed) {
                Scene.Requests.Add (new RemoveBoundsCommand<TItem, TEdge> (remove, this.Scene));
                if (remove.Equals(this.Scene.Hovered)) {
                    this.Scene.Hovered = default(TItem);
                }
                if (remove.Equals(this.Scene.Focused)) {
                    this.Scene.Focused = default(TItem);
                }
                if (this.Scene.Selected.Contains (remove)) {
                    this.Scene.Selected.Remove (remove);
                }
            }
        }

        public virtual void ShowAllData () {
            ApplyFilter ();
            var scene = this.Scene;
            TItem curr = scene.Focused;

            var roots = new Queue<TItem> (
                this.Graph.FindRoots (curr));

            new SubGraphWorker<TItem, TEdge> (this.SubGraph).Expand (roots, true);

            var aligner = CreateAligner(scene);
            
            var walker = new Walker<TItem, TEdge>(this.SubGraph);
            var options = Layout.Options();
            var pos = new Point(Layout.Border.Width, Layout.Border.Height);

            roots.ForEach(root => {
                var walk = walker.DeepWalk(root, 1).Where(l => !(l.Node is TEdge));
                if (OrderBy != null)
                    walk = walk.OrderBy(l => l, new LevelItemComparer<TItem> { OrderBy = this.OrderBy });
                
                var bounds = new Rectangle(pos, Size.Zero);
                aligner.Columns(walk, ref bounds, options);
                pos = new Point(pos.X, pos.Y + bounds.Size.Height + options.Distance.Height);
            });

            aligner.Commit();

            RestoreFocused (curr);

        }

        public virtual bool IsExpanded (SubGraph<TItem, TEdge> subGraph, TItem target) {
            var walker = new Walker<TItem, TEdge> (subGraph.Source);
            foreach (var item in walker.ExpandWalk (target, 0)) {
                if (!subGraph.Sink.Contains (item.Node)) {
                    return false;
                }
            }
            return true;
        }

        public virtual void Toggle () {
            var focused = Scene.Focused;
            if (focused == null) {
                focused = Scene.Selected.Elements.FirstOrDefault ();
            }
            if (focused != null && Scene.Graph == this.SubGraph) {
                if (IsExpanded (this.SubGraph, focused)) {
                    Collapse ();
                } else {
                    Expand (false);
                }
            }
        }
    }
}