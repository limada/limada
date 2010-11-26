/*
 * Limaki 
 * Version 0.08
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

namespace Limaki.Graphs.Extensions {
    /// <summary>
    /// extends some operations to a GraphView
    /// such as Expand, Collapse, Add
    /// each operation is a Unit of Work and gives back a collection of changed items
    /// </summary>
    public class GraphViewFacade<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        public GraphViewFacade(GraphView<TItem, TEdge> view) {
            this._graphView = view;
        }
        public IGraph<TItem, TEdge> Data {
            get { return graphView.Two; }
        }

        protected IGraph<TItem, TEdge> View {
            get { return graphView.One; }
        }

        private GraphView<TItem, TEdge> _graphView = null;
        /// <summary>
        /// the GraphView contains:
        /// View: the filtered graph
        /// Data: the original, full, unfiltered graph 
        /// </summary>
        protected GraphView<TItem, TEdge> graphView {
            get { return _graphView; }
        }

        /// <summary>
        /// Adds data to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ICollection<TItem> Add(TItem item) {
            ViewBuilder<TItem, TEdge> viewBuilder =
                new ViewBuilder<TItem, TEdge>(this.View);

            if (item is TEdge) {
                if (!Data.Contains((TEdge)item))
                    Data.Add((TEdge)item);
            } else if (!Data.Contains(item))
                Data.Add(item);

            viewBuilder.Add(item);

            CommitAdd(viewBuilder);

            return viewBuilder.Changes;
        }

        /// <summary>
        /// Adds data to view and data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ICollection<TItem> Add(IEnumerable<TItem> elements) {
            ViewBuilder<TItem, TEdge> viewBuilder =
                new ViewBuilder<TItem, TEdge>(this.View);

            foreach (TItem item in elements) {
                if (item is TEdge) {
                    if (!Data.Contains((TEdge)item))
                        Data.Add((TEdge)item);
                } else if (!Data.Contains(item))
                    Data.Add(item);
                
                viewBuilder.Add(item);
            }

            CommitAdd(viewBuilder);

            return viewBuilder.Changes;
        }

        public virtual ICollection<TItem> Expand(IEnumerable<TItem> elements, bool deep) {

            ViewBuilder<TItem, TEdge> viewBuilder =
                new ViewBuilder<TItem, TEdge>(this.View);

            foreach (TItem item in elements) {
                if (deep)
                    viewBuilder.AddDeepExpanded(item, this.Data);
                else
                    viewBuilder.AddExpanded(item, this.Data);
            }

            CommitAdd(viewBuilder);

            return viewBuilder.Changes;
        }


        /// <summary>
        /// add all items of viewBuilder into view
        /// add invisible, but valid edges into view:
        /// </summary>
        /// <param name="viewBuilder"></param>
        protected virtual void CommitAdd(ViewBuilder<TItem, TEdge> viewBuilder) {
            Stack<TItem> changeStack = new Stack<TItem>(viewBuilder.Changes);
            while (changeStack.Count > 0) {
                TItem item = changeStack.Pop();

                View.Add(item);

                // look if we have invisible, but valid edges
                if (Data is IBaseGraphPair<TItem, TEdge>) {
                    var data = (IBaseGraphPair<TItem, TEdge>)Data;
                    foreach (TEdge edge in data.ComplementEdges(item, View)) {
                        if (!viewBuilder.Changes.Contains(edge)) {
                            changeStack.Push(edge);
                            viewBuilder.Changes.Add(edge);
                        }
                    }
                }
                else 
                foreach (TEdge edge in Data.Fork(item)) {
                    if (viewBuilder.Contains(edge.Root) && viewBuilder.Contains(edge.Leaf)) {
                        if (!viewBuilder.Changes.Contains(edge)) {
                            changeStack.Push(edge);
                            viewBuilder.Changes.Add(edge);
                        }
                    }
                }
            }
        }

        protected void NeverRemove(ViewBuilder<TItem, TEdge> viewBuilder, IEnumerable<TItem> items) {
            foreach (TItem selected in items) {
                viewBuilder.NeverRemove(selected);
                viewBuilder.View.Add(selected);
                if (selected is TEdge) {
                    foreach (TEdge edge in viewBuilder.View.Vein((TEdge)selected)) {
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


        protected virtual void RevoveEdgesOfChanged(ViewBuilder<TItem, TEdge> viewBuilder) {
            Stack<TItem> changeStack = new Stack<TItem>(viewBuilder.Changes);
            while (changeStack.Count != 0) {
                TItem item = changeStack.Pop ();
                if (item is TEdge) {
                    TEdge edge = (TEdge)item;
                    if (viewBuilder.Changes.Contains(edge.Root) || viewBuilder.Changes.Contains(edge.Leaf)) {
                        viewBuilder.Remove(edge);
                    }
                }
            }          
        }

        public ICollection<TItem> Collapse( IEnumerable<TItem> elements ) {

            ViewBuilder<TItem, TEdge> viewBuilder =
                new ViewBuilder<TItem, TEdge>(this.View);

            NeverRemove(viewBuilder, elements);


            foreach (TItem item in elements) {
                viewBuilder.RemoveCollapsed(item, this.View);
            }

            RevoveEdgesOfChanged (viewBuilder);

            CommitRemove(viewBuilder);
            List<TItem> changes = new List<TItem>(viewBuilder.Changes);
            viewBuilder.Changes.Clear ();
            
            viewBuilder.RemoveOrphans(this.View);
            CommitRemove(viewBuilder);
            changes.AddRange (viewBuilder.Changes);

            viewBuilder.Changes.Clear();
            return changes;

        }

        public virtual ICollection<TItem> CollapseToFocused(IEnumerable<TItem> elements) {
            ViewBuilder<TItem, TEdge> viewBuilder =
                new ViewBuilder<TItem, TEdge>(this.View);

            NeverRemove(viewBuilder, elements);

            foreach (TItem item in View) {
                viewBuilder.RemoveCollapsed(item, View);
                viewBuilder.Remove(item);
            }

            CommitRemove(viewBuilder);

            return viewBuilder.Changes;
        }

        public ICollection<TItem> Hide(IEnumerable<TItem> elements) {

            ViewBuilder<TItem, TEdge> viewBuilder =
                new ViewBuilder<TItem, TEdge>(this.View);

            foreach (TItem item in elements) {
                viewBuilder.Remove(item);
                foreach (TEdge edge in this.View.Twig(item)) {
                    viewBuilder.Remove(edge);
                }
            }

            RevoveEdgesOfChanged(viewBuilder);

            CommitRemove(viewBuilder);
            //List<TItem> changes = new List<TItem>(viewBuilder.Changes);
            //viewBuilder.Changes.Clear();

            //viewBuilder.RemoveOrphans(this.View);
            //CommitRemove(viewBuilder);
            //changes.AddRange(viewBuilder.Changes);

            //viewBuilder.Changes.Clear();
            return viewBuilder.Changes;

        }

        protected virtual void CommitRemove(ViewBuilder<TItem, TEdge> viewBuilder) {
            foreach(TItem item in viewBuilder.Changes) {
                viewBuilder.View.Remove(item);
            }
        }

    }
}