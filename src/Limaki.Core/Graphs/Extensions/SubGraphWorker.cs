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


using System.Collections.Generic;

namespace Limaki.Graphs.Extensions {
    /// <summary>
    /// extends some operations to a SubGraph
    /// such as Expand, Collapse, Add
    /// each operation is a Unit of Work and gives back a collection of changed items
    /// </summary>
    public class SubGraphWorker<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        public SubGraphWorker(SubGraph<TItem, TEdge> subGraph) {
            this.SubGraph = subGraph;
            this.RemoveOrphans = true;
        }
        public IGraph<TItem, TEdge> Source {
            get { return SubGraph.Source; }
        }

        protected IGraph<TItem, TEdge> Sink {
            get { return SubGraph.Sink; }
        }

        public bool RemoveOrphans { get; set; }

        /// <summary>
        /// the SubGraph contains:
        /// Sink: the filtered graph
        /// Source: the original, full, unfiltered graph 
        /// </summary>
        protected SubGraph<TItem, TEdge> SubGraph { get; set; }

        /// <summary>
        /// Adds data to SubGraph and data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ICollection<TItem> Add(TItem item) {
            var worker = new WalkerWorker<TItem, TEdge>(this.Sink);

            if (item is TEdge) {
                if (!Source.Contains((TEdge)item))
                    Source.Add((TEdge)item);
            } else if (!Source.Contains(item))
                Source.Add(item);

            worker.Add(item);

            CommitAdd(worker);

            return worker.Changes;
        }

        /// <summary>
        /// Adds data to SubGraph and data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ICollection<TItem> Add(IEnumerable<TItem> elements) {
            var worker = new WalkerWorker<TItem, TEdge>(this.Sink);

            foreach (var item in elements) {
                if (item is TEdge) {
                    if (!Source.Contains((TEdge)item))
                        Source.Add((TEdge)item);
                } else if (!Source.Contains(item))
                    Source.Add(item);
                
                worker.Add(item);
            }

            CommitAdd(worker);

            return worker.Changes;
        }

        public virtual ICollection<TItem> Expand(IEnumerable<TItem> elements, bool deep) {

            var worker = new WalkerWorker<TItem, TEdge>(this.Sink);

            foreach (var item in elements) {
                if (deep)
                    worker.AddDeepExpanded(item, this.Source);
                else
                    worker.AddExpanded(item, this.Source);
            }

            CommitAdd(worker);

            return worker.Changes;
        }


        /// <summary>
        /// add all items of worker into SubGraph
        /// add invisible, but valid edges into SubGraph:
        /// </summary>
        /// <param name="worker"></param>
        protected virtual void CommitAdd(WalkerWorker<TItem, TEdge> worker) {
            var changeStack = new Stack<TItem>(worker.Changes);
            while (changeStack.Count > 0) {
                var item = changeStack.Pop();

                Sink.Add(item);

                // look if we have invisible, but valid edges
                var sink = Source as ISinkGraph<TItem, TEdge>;
                if (sink != null) {
                    foreach (var edge in sink.ComplementEdges(item, Sink)) {
                        if (!worker.Changes.Contains(edge)) {
                            changeStack.Push(edge);
                            worker.Changes.Add(edge);
                        }
                    }
                } else  foreach (var edge in Source.Fork(item)) {
                    if (worker.Contains(edge.Root) && worker.Contains(edge.Leaf)) {
                        if (!worker.Changes.Contains(edge)) {
                            changeStack.Push(edge);
                            worker.Changes.Add(edge);
                        }
                    }
                }
            }
        }

        protected void NeverRemove(WalkerWorker<TItem, TEdge> worker, IEnumerable<TItem> items) {
            foreach (var item in items) {
                worker.NeverRemove(item);
                worker.Graph.Add(item);
                if (item is TEdge) {
                    foreach (var edge in worker.Graph.Vein((TEdge)item)) {
                        worker.NeverRemove(edge);
                        worker.Graph.Add(edge);
                        worker.NeverRemove(edge.Root);
                        worker.Graph.Add(edge.Root);
                        worker.NeverRemove(edge.Leaf);
                        worker.Graph.Add(edge.Leaf);
                    }
                } else {
                    foreach (var edge in worker.Graph.Twig(item)) {
                       if(worker.NoTouch.Contains(edge.Root)&&
                           worker.NoTouch.Contains(edge.Leaf)) {
                               worker.NeverRemove(edge);
                               worker.Graph.Add(edge);
                       }
                    }
                }
            }
        }


        protected virtual void RevoveEdgesOfChanged(WalkerWorker<TItem, TEdge> worker) {
            var changeStack = new Stack<TItem>(worker.Changes);
            while (changeStack.Count != 0) {
                TItem item = changeStack.Pop ();
                if (item is TEdge) {
                    TEdge edge = (TEdge)item;
                    if (worker.Changes.Contains(edge.Root) || worker.Changes.Contains(edge.Leaf)) {
                        worker.Remove(edge);
                    }
                }
            }          
        }

        public ICollection<TItem> Collapse( IEnumerable<TItem> elements ) {

            var worker = new WalkerWorker<TItem, TEdge>(this.Sink);

            NeverRemove(worker, elements);


            foreach (var item in elements) {
                worker.RemoveCollapsed(item, this.Sink);
            }

            RevoveEdgesOfChanged (worker);

            CommitRemove(worker);
            var result = worker.Changes;

            if (RemoveOrphans) {
                var changes = new List<TItem>(worker.Changes);
                worker.Changes.Clear ();
                worker.RemoveOrphans(this.Sink);
                CommitRemove(worker);
                changes.AddRange(worker.Changes);
                worker.Changes.Clear();
                result = changes;
            }
           
            return result;

        }

        public virtual ICollection<TItem> CollapseToFocused(IEnumerable<TItem> elements) {
            var worker = new WalkerWorker<TItem, TEdge>(this.Sink);

            NeverRemove(worker, elements);

            foreach (TItem item in Sink) {
                worker.RemoveCollapsed(item, Sink);
                worker.Remove(item);
            }

            CommitRemove(worker);

            return worker.Changes;
        }

        public ICollection<TItem> Hide(IEnumerable<TItem> elements) {

            var worker = new WalkerWorker<TItem, TEdge>(this.Sink);

            foreach (var item in elements) {
                worker.Remove(item);
                foreach (var edge in this.Sink.Twig(item)) {
                    worker.Remove(edge);
                }
            }

            RevoveEdgesOfChanged(worker);

            CommitRemove(worker);
            //List<TItem> changes = new List<TItem>(WalkerWorker.Changes);
            //WalkerWorker.Changes.Clear();

            //WalkerWorker.RemoveOrphans(this.View);
            //CommitRemove(WalkerWorker);
            //changes.AddRange(WalkerWorker.Changes);

            //WalkerWorker.Changes.Clear();
            return worker.Changes;

        }

        protected virtual void CommitRemove(WalkerWorker<TItem, TEdge> worker) {
            foreach(var item in worker.Changes) {
                worker.Graph.Remove(item);
            }
        }

    }
}