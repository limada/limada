/*
 * Limaki 
 * Version 0.071
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

using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common.Collections;
using Limaki.Graphs;
using Limaki.Widgets;

namespace Limaki.Widgets.Layout {
    /// <summary>
    /// Provides methods to expand and collapse subgraphs of items
    /// uses Layout to set appropriate commands into the Scene.Commands-queue
    /// </summary>
    public class FoldingControler {
        Scene _scene = null;
        public virtual Scene Scene {
            get { return _scene; }
            set { _scene = value; }
        }

        private ILayout<Scene,IWidget> _layout = null;
        public ILayout<Scene, IWidget> Layout {
            get { return _layout; }
            set { _layout = value; }
        }

        public virtual void Clear() {
            _scene = null;
            view.Clear();
            this._data = null;
            this.graphView = null;
            createdWidgets.Clear();
        }

        private IGraph<IWidget, IEdgeWidget> _data = null;
        /// <summary>
        /// the unfiltered source data, should contain the whole graph
        /// sets _data only if _data is not the graphView
        /// that means, _data should always contain the original Data
        /// this is a Workaround!!!
        /// this behavaior shoult be raplaced with
        /// something like: bool IsViewActive
        /// </summary>
        public IGraph<IWidget, IEdgeWidget> Data {
            get { return _data; }
            set {
                if (Scene.Graph != graphView) {
                    _data = value;
                    if (_data is IFactoryListener<IWidget>) {
                        ((IFactoryListener<IWidget>)_data).CreateListener +=
                            delegate(IWidget w) {
                                if (!createdWidgets.Contains(w))
                                    createdWidgets.Add(w);
                            };
                    }
                }
            }
        }

        /// <summary>
        /// the GraphView of the unfolded widgets 
        /// </summary>
        private GraphView<IWidget, IEdgeWidget> graphView = null;

        /// <summary>
        /// the Graph containing the unfolded widgets
        /// </summary>
        private IGraph<IWidget, IEdgeWidget> view = new Graph<IWidget, IEdgeWidget>();

        /// <summary>
        /// a list containing the widgets which are newly created in this session
        /// </summary>
        public ICollection<IWidget> createdWidgets = new Set<IWidget>();


        protected virtual IEnumerable<TItem> Nodes<TItem, TEdge>(IEnumerable<TEdge> source)
            where TEdge : IEdge<TItem>, TItem {
            Set<TItem> done = new Set<TItem>();
            foreach (TEdge edge in source) {
                if (!done.Contains(edge.Root)) {
                    done.Add(edge.Root);
                    if (!(edge.Root is TEdge))
                        yield return edge.Root;
                }
                if (!done.Contains(edge.Leaf)) {
                    done.Add(edge.Leaf);
                    if (!(edge.Leaf is TEdge))
                        yield return edge.Leaf;
                }
            }
        }



        public bool IsFiltered = false;
        public virtual void ApplyFilter() {
            if (true){//(graphView==null || graphView.Data != this.Data) {
                graphView = new GraphView<IWidget, IEdgeWidget>();  
            }
            graphView.Data = this.Data;
            graphView.View = view;
            if (Scene.Graph != graphView) {
                Scene.Graph = graphView;
            }
            IsFiltered = true;
        }

        public virtual void UpdateDone(IWidget curr) {
            Scene.Focused = curr;
            Scene.Selected.Add(curr);
        }

        public virtual void Expand() {
            IWidget curr = null;
            ViewBuilder viewBuilder = new ViewBuilder(this.view);

            if (Scene.Focused != null) {
                this.Data = Scene.Graph;
                curr = Scene.Focused;

                foreach (IWidget widget in Scene.Selected.Elements) {
                    viewBuilder.AddExpanded(widget, this.Data);
                }

                ApplyFilter();
                UpdateAdded(curr, viewBuilder);
                UpdateDone (curr);
            }
        }

        public virtual void UpdateAdded(IWidget curr, ViewBuilder viewBuilder) {
            // add all items of newFilter into filter
            Stack<IWidget> changeStack = new Stack<IWidget>(viewBuilder.Changes);
            while (changeStack.Count > 0) {
                IWidget widget = changeStack.Pop();

                view.Add(widget);

                if (createdWidgets.Contains(widget)) {
                    Layout.Invoke(widget);
                    Layout.Justify(widget);
                    createdWidgets.Remove(widget);
                }

                // look if we have invisible, but valid edges
                foreach (IEdgeWidget edge in Data.Fork(widget)) {
                    if (viewBuilder.Contains(edge.Root) && viewBuilder.Contains(edge.Leaf)) {
                        if (!viewBuilder.Changes.Contains(edge)) {
                            changeStack.Push(edge);
                            viewBuilder.Changes.Add(edge);
                        }
                    }
                }
            }


            LayoutProperties prop = new LayoutProperties();
            prop.Layout = this.Layout;

            Arranger<Scene, IWidget, IEdgeWidget> arranger =
                new Arranger<Scene, IWidget, IEdgeWidget>(this.Scene, this.Layout);

            arranger.Arrange(curr, viewBuilder.Changes);

            if (graphView != null)
                foreach (IEdgeWidget edge in arranger.AffectedEdges) {
                    Scene.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
                }

        }


        void NeverRemove(ViewBuilder viewBuilder, IEnumerable<IWidget> widgets) {
            foreach (IWidget selected in widgets) {
                viewBuilder.NeverRemove(selected);
                viewBuilder.View.Add(selected);
                if (selected is IEdgeWidget) {
                    foreach (IEdgeWidget edge in viewBuilder.View.Vein((IEdgeWidget)selected)) {
                        viewBuilder.NeverRemove(edge);
                        viewBuilder.View.Add(edge);
                        viewBuilder.NeverRemove(edge.Root);
                        viewBuilder.View.Add(edge.Root);
                        viewBuilder.NeverRemove(edge.Leaf);
                        viewBuilder.View.Add(edge.Leaf);
                    }
                }
            }
        }
        public virtual void Collapse() {
            IWidget curr = null;
            ViewBuilder viewBuilder = new ViewBuilder(this.view);
            if (Scene.Focused != null) {
                    
                this.Data = Scene.Graph;

                curr = Scene.Focused;

                NeverRemove (viewBuilder, Scene.Selected.Elements);

                foreach (IWidget widget in Scene.Selected.Elements) {
                    viewBuilder.RemoveCollapsed(widget, viewBuilder.View);
                }
                
                Stack<IWidget> siblings = new Stack<IWidget>(viewBuilder.Changes);
                while (siblings.Count !=0){
                    IEdgeWidget edge = siblings.Pop () as IEdgeWidget;
                    if (edge != null) {
                        if (viewBuilder.Changes.Contains(edge.Root) || viewBuilder.Changes.Contains(edge.Leaf) ) {
                            viewBuilder.Remove (edge);
                        }
                    }
                }

                ApplyFilter();

                UpdateRemoved(curr, viewBuilder);

                viewBuilder.RemoveOrphans (viewBuilder.View);
                UpdateRemoved(curr, viewBuilder);

                UpdateDone(curr);
            }
        }

        public virtual void CollapseToFocused() {
            IWidget curr = null;
            ViewBuilder viewBuilder = new ViewBuilder(this.view);
            if (Scene.Focused != null) {
                this.Data = Scene.Graph;

                curr = Scene.Focused;

                NeverRemove(viewBuilder, Scene.Selected.Elements);

                foreach(IWidget widget in Scene.Graph) {
                    viewBuilder.RemoveCollapsed(widget, Scene.Graph);
                    viewBuilder.Remove (widget);
                }

                UpdateRemoved(curr, viewBuilder);
                ApplyFilter();
                UpdateDone(curr);
            }
        }
        
        public virtual void UpdateRemoved(IWidget curr, ViewBuilder viewBuilder) {
            Stack<IWidget> changeStack = new Stack<IWidget>(viewBuilder.Changes);
            while (changeStack.Count > 0) {
                IWidget widget = changeStack.Pop();
                viewBuilder.View.Remove(widget);
                Scene.Commands.Add(new RemoveBoundsCommand(widget, this.Scene));
            }
            viewBuilder.Changes.Clear();
        }

        public virtual void ShowAllData() {
            if (Scene.Focused != null) {
                
                IWidget curr = Scene.Focused;
                if (Scene.Graph == this.graphView) {
                    
                    Scene.Graph = this.graphView.Data;

                    IsFiltered = false;
                    if (curr != null) {
                        Scene.Focused = curr;
                        Scene.Selected.Add (curr);
                    }
                    this.Layout.Invoke ();
                    
                }
            }
        }
    }
}