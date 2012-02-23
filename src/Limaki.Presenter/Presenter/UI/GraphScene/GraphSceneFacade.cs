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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using System.Linq;
using Limaki.Presenter.Layout;
using Xwt;

namespace Limaki.Presenter.UI.GraphScene {
    /// <summary>
    /// Provides a simplified interface to
    /// a scene, a layout and the scenes graph
    /// where scene.graph is a GraphView
    /// </summary>
    public class GraphSceneFacade<TItem, TEdge> 
    where TItem:class
    where TEdge:TItem,IEdge<TItem> {
        public GraphSceneFacade(Get<IGraphScene<TItem, TEdge>> sceneHandler, IGraphLayout<TItem, TEdge> layout) {
            this.SceneHandler = sceneHandler;
            this.Layout = layout;
        }

        Get<IGraphScene<TItem, TEdge>> SceneHandler = null;
        IGraphScene<TItem, TEdge> _scene = null;
        public virtual IGraphScene<TItem, TEdge> Scene {
            get {
                IGraphScene<TItem, TEdge> scene = _scene;

                if (SceneHandler != null)
                    scene = SceneHandler();

                if (scene != _scene) {
                    Clear();
                    _scene = scene;
                }
                return _scene;
            }
        }


        private IGraph<TItem, TEdge> _data = null;
        /// <summary>
        /// the data-graph of the GraphView
        /// </summary>
        public IGraph<TItem, TEdge> Data {
            get {
                var data = _data;
                IGraphScene<TItem, TEdge> scene = this.Scene;
                if (scene.Graph is GraphView<TItem, TEdge>) {
                    _data = ((GraphView<TItem, TEdge>)scene.Graph).Two;
                } else {
                    _data = scene.Graph;
                }
                if (data != _data) {
                    if (_data is IFactoryListener<TItem>) {
                        ((IFactoryListener<TItem>)_data).ItemCreated +=
                            delegate(TItem w) {
                                if (!CreatedItems.Contains(w))
                                    CreatedItems.Add(w);
                            };
                    }
                }
                return _data;
            }
        }

        private IGraph<TItem, TEdge> _view = null;
        /// <summary>
        /// the Graph containing the filtered items
        /// graphView.View will be set to view
        /// </summary>
        protected IGraph<TItem, TEdge> View {
            get {
                if (this.Scene.Graph is GraphView<TItem, TEdge>) {
                    _view = ((GraphView<TItem, TEdge>)this.Scene.Graph).One;
                } else if (_view == null) {
                    _view = new Graph<TItem, TEdge>();

                }
                return _view;
            }
        }

        private GraphView<TItem, TEdge> _graphView = null;
        /// <summary>
        /// the GraphView contains:
        /// View: the filtered graph
        /// Data: the original, full, unfiltered graph 
        /// </summary>
        protected GraphView<TItem, TEdge> graphView {
            get {
                if (this.Scene.Graph is GraphView<TItem, TEdge>) {
                    _graphView = (GraphView<TItem, TEdge>)this.Scene.Graph;
                    IsFiltered = true;
                } else if (_graphView == null) {
                    _graphView = new GraphView<TItem, TEdge>(this.Data, this.View);
                }

                return _graphView;
            }
        }

        private IGraphLayout<TItem, TEdge> _layout = null;
        public IGraphLayout<TItem, TEdge> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        public virtual void Clear() {
            this._scene = null;
            this._view = null;
            this._data = null;
            this._graphView = null;
            CreatedItems.Clear();
        }

        /// <summary>
        /// a list containing the visuals which are newly created in this session
        /// </summary>
        public ICollection<TItem> CreatedItems = new Set<TItem>();

        public bool IsFiltered = false;
        public virtual void ApplyFilter() {
            if (Scene.Graph != graphView) {
                TItem curr = Scene.Focused;
                Scene.Graph = graphView;
                Scene.Focused = curr;
            }
            IsFiltered = true;
        }

        public virtual void RestoreFocused(TItem curr) {
            Scene.Focused = curr;
            Scene.Selected.Add(curr);
        }

        public virtual Func<TItem, string> OrderBy { get; set; }
        protected virtual Arranger<TItem, TEdge> CreateArranger(IGraphScene<TItem, TEdge> data) {
            var result = new Arranger<TItem, TEdge>(data, this.Layout);
            if (OrderBy != null)
                result.OrderBy = this.OrderBy;
            return result;
        }

        /// <summary>
        /// adds item to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pt"></param>
        public virtual void Add(TItem item, Point pt) {
            IGraphScene<TItem, TEdge> scene = this.Scene;
            if (scene == null || item == null)
                return;
            ApplyFilter();
            TItem curr = scene.Focused;

            var arranger = CreateArranger (scene);

            var affected = new GraphViewFacade<TItem, TEdge> (this.graphView).Add (item);

            arranger.Arrange(item, affected, pt);

            arranger.Commit();

            RestoreFocused(curr);

        }

        /// <summary>
        /// adds items to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pt"></param>
        public virtual void Add(IEnumerable<TItem> elements, bool justify, bool arrange) {
            IGraphScene<TItem, TEdge> scene = this.Scene;
            ApplyFilter();
            TItem curr = scene.Focused;

            var arranger = CreateArranger(scene);

            var affected = new GraphViewFacade<TItem, TEdge>(this.graphView).Add(elements);

            if (arrange)
                arranger.ArrangeItems(affected, justify, (Point)Layout.Border);
            else
                arranger.ArrangeEdges(affected, justify);

            arranger.Commit();

            RestoreFocused(curr);

        }
        public virtual void AddRaw(IEnumerable<TItem> elements) {
            IGraphScene<TItem, TEdge> scene = this.Scene;
            ApplyFilter();
            TItem curr = scene.Focused;

            var affected = new GraphViewFacade<TItem, TEdge>(this.graphView).Add(elements);

        }

        public virtual void Expand(bool deep) {
            IGraphScene<TItem, TEdge> scene = this.Scene;
            if (scene.Selected.Count > 0) {
                ApplyFilter();
                

                TItem curr = scene.Focused;
                var arranger = CreateArranger(scene);

                var affected = new GraphViewFacade<TItem, TEdge> 
                    (this.graphView).Expand (scene.Selected.Elements, deep);

                arranger.Arrange (scene.Selected.Elements, affected, deep);
                
                arranger.Commit ();

                RestoreFocused(curr);
                
            }
        }

        public bool RemoveOrhpans { get; set; }
        public virtual void Collapse() {
            if (Scene.Selected.Count > 0) {
                ApplyFilter();
                var scene = this.Scene;
                TItem curr = scene.Focused;
                var affected = new GraphViewFacade<TItem, TEdge> (this.graphView){RemoveOrphans = this.RemoveOrhpans}
                    .Collapse (scene.Selected.Elements);
                UpdateRemoved(affected);

                RestoreFocused(curr);

            }
        }

        public virtual void CollapseToFocused() {
            if (Scene.Focused != null) {
                ApplyFilter();
                TItem curr = Scene.Focused;

                UpdateRemoved(new GraphViewFacade<TItem, TEdge>(this.graphView)
                    .CollapseToFocused(Scene.Selected.Elements));

                RestoreFocused(curr);
            }
        }
        
        public virtual void Hide() {
            if (Scene.Selected.Count > 0) {
                ApplyFilter();
                
                UpdateRemoved(new GraphViewFacade<TItem, TEdge>(this.graphView)
                    .Hide(Scene.Selected.Elements));

                Scene.Selected.Clear ();
                Scene.Focused = null;
                Scene.Hovered = null;


            }
        }

        public virtual void UpdateRemoved(ICollection<TItem> removed) {
            foreach(TItem remove in removed) {
                Scene.Requests.Add(new RemoveBoundsCommand<TItem,TEdge>(remove, this.Scene));
                if (this.Scene.Hovered == remove) {
                    this.Scene.Hovered = null;
                }
                if (this.Scene.Focused == remove) {
                    this.Scene.Focused = null;
                }
                if (this.Scene.Selected.Contains(remove)) {
                    this.Scene.Selected.Remove (remove);
                }
            }
        }

        public virtual void ShowAllData() {
            ApplyFilter();
            var scene = this.Scene;
            TItem curr = scene.Focused;
            
            var roots = new Queue<TItem>(
                this.Data.FindRoots (curr));
            
            new GraphViewFacade<TItem, TEdge> (this.graphView).Expand (roots, true);

            var arranger = CreateArranger(scene);

            arranger.ArrangeDeepWalk(roots,true,(Point)Layout.Distance);

            arranger.Commit ();

            RestoreFocused(curr);

        }

        public virtual bool IsExpanded(GraphView<TItem, TEdge> graphView, TItem target) {
            var walker = new Walker<TItem, TEdge>(graphView.Two);
            foreach (var item in walker.ExpandWalk(target, 0)) {
                if (!graphView.One.Contains(item.Node)) {
                    return false;
                }
            }
            return true;
        }

        public virtual void Toggle() {
            var focused = Scene.Focused;
            if(focused==null) {
                focused = Scene.Selected.Elements.FirstOrDefault ();
            }
            if (focused != null && Scene.Graph == this.graphView) {
                if (IsExpanded(this.graphView, focused)) {
                    Collapse();
                } else {
                    Expand(false);
                }
            }
        }
    }
}