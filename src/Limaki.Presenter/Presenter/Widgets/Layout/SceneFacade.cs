/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
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
using Limaki.Presenter.Layout;
using Limaki.Presenter.UI;
using Limaki.Widgets;


namespace Limaki.Presenter.Widgets.Layout {
    /// <summary>
    /// Provides a simplified interface to
    /// a scene, a layout and the scenes graph
    /// where scene.graph is a GraphView
    /// </summary>
    public class SceneFacade {
        public SceneFacade(Get<IGraphScene<IWidget, IEdgeWidget>> sceneHandler, IGraphLayout<IWidget, IEdgeWidget> layout) {
            this.SceneHandler = sceneHandler;
            this.Layout = layout;
        }

        Get<IGraphScene<IWidget, IEdgeWidget>> SceneHandler = null;
        IGraphScene<IWidget, IEdgeWidget> _scene = null;
        public virtual IGraphScene<IWidget, IEdgeWidget> Scene {
            get {
                var scene = _scene;

                if (SceneHandler != null)
                    scene = SceneHandler();

                if (scene != _scene) {
                    Clear();
                    _scene = scene;
                }
                return _scene;
            }
        }


        private IGraph<IWidget, IEdgeWidget> _data = null;
        /// <summary>
        /// the data-graph of the GraphView
        /// </summary>
        public IGraph<IWidget, IEdgeWidget> Data {
            get {
                var data = _data;
                var scene = this.Scene;
                if (scene.Graph is GraphView<IWidget, IEdgeWidget>) {
                    _data = ((GraphView<IWidget, IEdgeWidget>)scene.Graph).Two;
                } else {
                    _data = scene.Graph;
                }
                if (data != _data) {
                    if (_data is IFactoryListener<IWidget>) {
                        ((IFactoryListener<IWidget>)_data).ItemCreated +=
                            delegate(IWidget w) {
                                if (!createdWidgets.Contains(w))
                                    createdWidgets.Add(w);
                            };
                    }
                }
                return _data;
            }
        }

        private IGraph<IWidget, IEdgeWidget> _view = null;
        /// <summary>
        /// the Graph containing the filtered widgets
        /// graphView.View will be set to view
        /// </summary>
        protected IGraph<IWidget, IEdgeWidget> View {
            get {
                if (this.Scene.Graph is GraphView<IWidget, IEdgeWidget>) {
                    _view = ((GraphView<IWidget, IEdgeWidget>)this.Scene.Graph).One;
                } else if (_view == null) {
                    _view = new Graph<IWidget, IEdgeWidget>();

                }
                return _view;
            }
        }

        private GraphView<IWidget, IEdgeWidget> _graphView = null;
        /// <summary>
        /// the GraphView contains:
        /// View: the filtered graph
        /// Data: the original, full, unfiltered graph 
        /// </summary>
        protected GraphView<IWidget, IEdgeWidget> graphView {
            get {
                if (this.Scene.Graph is GraphView<IWidget, IEdgeWidget>) {
                    _graphView = (GraphView<IWidget, IEdgeWidget>)this.Scene.Graph;
                    IsFiltered = true;
                } else if (_graphView == null) {
                    _graphView = new GraphView<IWidget, IEdgeWidget>(this.Data, this.View);
                }

                return _graphView;
            }
        }

        private IGraphLayout<IWidget, IEdgeWidget> _layout = null;
        public IGraphLayout<IWidget, IEdgeWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        public virtual void Clear() {
            this._scene = null;
            this._view = null;
            this._data = null;
            this._graphView = null;
            createdWidgets.Clear();
        }

        /// <summary>
        /// a list containing the widgets which are newly created in this session
        /// </summary>
        public ICollection<IWidget> createdWidgets = new Set<IWidget>();

        public bool IsFiltered = false;
        public virtual void ApplyFilter() {
            if (Scene.Graph != graphView) {
                IWidget curr = Scene.Focused;
                Scene.Graph = graphView;
                Scene.Focused = curr;
            }
            IsFiltered = true;
        }

        public virtual void RestoreFocused(IWidget curr) {
            Scene.Focused = curr;
            Scene.Selected.Add(curr);
        }

        protected virtual Arranger<IWidget, IEdgeWidget> CreateArranger(IGraphScene<IWidget, IEdgeWidget> data) {
            return new Arranger<IWidget, IEdgeWidget> (data, this.Layout);
        }

        /// <summary>
        /// adds item to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pt"></param>
        public virtual void Add(IWidget item, PointI pt) {
            var scene = this.Scene;
            if (scene == null || item == null)
                return;
            ApplyFilter();
            IWidget curr = scene.Focused;
            
            var arranger = CreateArranger(scene);

            var affected = new GraphViewFacade<IWidget, IEdgeWidget> 
                            (this.graphView).Add (item);

            arranger.Arrange(item, affected, pt);

            arranger.Commit();

            RestoreFocused(curr);

        }

        /// <summary>
        /// adds items to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <param name="pt"></param>
        public virtual void Add(IEnumerable<IWidget> elements, bool justify, bool arrange) {
            var scene = this.Scene;
            ApplyFilter();
            IWidget curr = scene.Focused;

            var arranger = CreateArranger(scene);

            var affected = new GraphViewFacade<IWidget, IEdgeWidget>
                                (this.graphView).Add(elements);

            if (arrange)
                arranger.ArrangeDeepWalk(affected,justify,(PointI)Layout.Distance);
            else
                arranger.ArrangeEdges(affected, justify);

            arranger.Commit();

            RestoreFocused(curr);

        }

        public virtual void Expand(bool deep) {
            var scene = this.Scene;
            if (scene.Selected.Count > 0) {
                ApplyFilter();
                

                IWidget curr = scene.Focused;
                var arranger = CreateArranger(scene);

                var affected = new GraphViewFacade<IWidget, IEdgeWidget> 
                    (this.graphView).Expand (scene.Selected.Elements, deep);

                arranger.Arrange (scene.Selected.Elements, affected, deep);
                
                arranger.Commit ();

                RestoreFocused(curr);
            }
        }

        public virtual void Collapse() {
            if (Scene.Selected.Count > 0) {
                ApplyFilter();
                IWidget curr = Scene.Focused;

                UpdateRemoved(new GraphViewFacade<IWidget, IEdgeWidget>(this.graphView)
                    .Collapse(Scene.Selected.Elements));

                RestoreFocused(curr);

            }
        }

        public virtual void CollapseToFocused() {
            if (Scene.Focused != null) {
                ApplyFilter();
                IWidget curr = Scene.Focused;

                UpdateRemoved(new GraphViewFacade<IWidget, IEdgeWidget>(this.graphView)
                    .CollapseToFocused(Scene.Selected.Elements));

                RestoreFocused(curr);
            }
        }
        
        public virtual void Hide() {
            if (Scene.Selected.Count > 0) {
                ApplyFilter();
                
                UpdateRemoved(new GraphViewFacade<IWidget, IEdgeWidget>(this.graphView)
                    .Hide(Scene.Selected.Elements));

                Scene.Selected.Clear ();
                Scene.Focused = null;
                Scene.Hovered = null;


            }
        }

        public virtual void UpdateRemoved(ICollection<IWidget> removed) {
            foreach(IWidget widget in removed) {
                Scene.Requests.Add(
                    new RemoveBoundsCommand<IWidget,IEdgeWidget>(
                        widget, this.Scene));
                if (this.Scene.Hovered == widget) {
                    this.Scene.Hovered = null;
                }
                if (this.Scene.Focused == widget) {
                    this.Scene.Focused = null;
                }
                if (this.Scene.Selected.Contains(widget)) {
                    this.Scene.Selected.Remove (widget);
                }
            }
        }

        public virtual void ShowAllData() {
            ApplyFilter();
            IWidget curr = Scene.Focused;
            
            var roots = new Queue<IWidget>(
                new GraphPairFacade<IWidget, IEdgeWidget> ().FindRoots (this.Data, curr));
            
            new GraphViewFacade<IWidget, IEdgeWidget> (this.graphView).Expand (roots, true);

            var arranger = CreateArranger(this.Scene);

            arranger.ArrangeDeepWalk(roots,true,(PointI)Layout.Distance);

            arranger.Commit ();

            RestoreFocused(curr);

        }

        public virtual bool IsExpanded(GraphView<IWidget, IEdgeWidget> graphView, IWidget widget) {
            var walker = new Walker<IWidget, IEdgeWidget>(graphView.Two);
            foreach (var item in walker.ExpandWalk(widget, 0)) {
                if (!graphView.One.Contains(item.Node)) {
                    return false;
                }
            }
            return true;
        }

        public virtual void Toggle() {
            if (Scene.Focused != null && Scene.Graph == this.graphView) {
                if (IsExpanded(this.graphView, Scene.Focused)) {
                    Collapse();
                } else {
                    Expand(false);
                }
            }
        }
    }
}