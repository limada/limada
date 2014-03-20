using System.Collections.Generic;
using System.Linq;

namespace Limaki.Graphs {
    /// <summary>
    /// extends some operations to a SubGraph
    /// such as Expand, Collapse, Add
    /// each operation is a Unit of Work and gives back a collection of changed items
    /// shouldnt change elements order
    /// </summary>
    public class SubGraphWorker1<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {
        public SubGraphWorker1 (SubGraph<TItem, TEdge> subGraph) {
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
        public virtual ICollection<TItem> Add (TItem item) {
            var worker = new WalkerWorker1<TItem, TEdge> (this.Sink);

            if (item is TEdge) {
                if (!Source.Contains ((TEdge)item))
                    Source.Add ((TEdge)item);
            } else if (!Source.Contains (item))
                Source.Add (item);

            worker.Add (item);

            CommitAdd (worker);

            return worker.Affected;
        }

        /// <summary>
        /// Adds data to SubGraph and data
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public virtual ICollection<TItem> Add (IEnumerable<TItem> elements) {
            var worker = new WalkerWorker1<TItem, TEdge> (this.Sink);

            foreach (var item in elements) {
                if (item is TEdge) {
                    if (!Source.Contains ((TEdge)item))
                        Source.Add ((TEdge)item);
                } else if (!Source.Contains (item))
                    Source.Add (item);

                worker.Add (item);
            }

            CommitAdd (worker);

            return worker.Affected;
        }

        public virtual ICollection<TItem> Expand (IEnumerable<TItem> elements, bool deep) {

            var worker = new WalkerWorker1<TItem, TEdge> (this.Sink);

            foreach (var item in elements) {
                if (deep)
                    worker.AddDeepExpanded (item, this.Source);
                else
                    worker.AddExpanded (item, this.Source);
            }

            CommitAdd (worker);

            return worker.Affected;
        }


        /// <summary>
        /// add all items of worker into SubGraph
        /// add invisible, but valid edges into SubGraph:
        /// </summary>
        /// <param name="worker"></param>
        protected virtual void CommitAdd (WalkerWorker1<TItem, TEdge> worker) {
            var changeStack = new Queue<TItem> (worker.Affected);
            while (changeStack.Count > 0) {
                var item = changeStack.Dequeue ();

                Sink.Add (item);

                // look if we have invisible, but valid edges
                var sink = Source as ISinkGraph<TItem, TEdge>;
                if (sink != null) {
                    foreach (var edge in sink.ComplementEdges (item, Sink)) {
                        if (!worker.Changed (edge)) {
                            changeStack.Enqueue (edge);
                            worker.AddChange (edge);
                        }
                    }
                } else foreach (var edge in Source.Fork (item)) {
                    if (worker.Contains (edge.Root) && worker.Contains (edge.Leaf)) {
                        if (!worker.Changed (edge)) {
                            changeStack.Enqueue (edge);
                            worker.AddChange (edge);
                        }
                    }
                }
            }
        }

        protected void NeverRemove (WalkerWorker1<TItem, TEdge> worker, IEnumerable<TItem> items) {
            foreach (var item in items) {
                worker.NeverRemove (item);
                worker.Graph.Add (item);
                if (item is TEdge) {
                    foreach (var edge in worker.Graph.Vein ((TEdge)item)) {
                        worker.NeverRemove (edge);
                        worker.Graph.Add (edge);
                        worker.NeverRemove (edge.Root);
                        worker.Graph.Add (edge.Root);
                        worker.NeverRemove (edge.Leaf);
                        worker.Graph.Add (edge.Leaf);
                    }
                } else {
                    foreach (var edge in worker.Graph.Twig (item)) {
                        if (worker.NoTouch.Contains (edge.Root) &&
                            worker.NoTouch.Contains (edge.Leaf)) {
                            worker.NeverRemove (edge);
                            worker.Graph.Add (edge);
                        }
                    }
                }
            }
        }


        protected virtual void RevoveEdgesOfChanged (WalkerWorker1<TItem, TEdge> worker) {
            foreach (var edge in worker.Affected.OfType<TEdge>().ToArray()) {
                if (worker.Changed (edge.Root) || worker.Changed (edge.Leaf)) {
                    worker.Remove (edge);
                }
            }
        }

        public ICollection<TItem> Collapse (IEnumerable<TItem> elements) {

            var worker = new WalkerWorker1<TItem, TEdge> (this.Sink);

            NeverRemove (worker, elements);


            foreach (var item in elements) {
                worker.RemoveCollapsed (item, this.Sink);
            }

            RevoveEdgesOfChanged (worker);

            CommitRemove (worker);
            var result = worker.Affected;

            if (RemoveOrphans) {
                var changes = new List<TItem> (worker.Affected);
                worker.ChangesClear ();
                worker.RemoveOrphans (this.Sink);
                CommitRemove (worker);
                changes.AddRange (worker.Affected);
                worker.ChangesClear ();
                result = changes;
            }

            return result;

        }

        public virtual ICollection<TItem> CollapseToFocused (IEnumerable<TItem> elements) {
            var worker = new WalkerWorker1<TItem, TEdge> (this.Sink);

            NeverRemove (worker, elements);

            foreach (TItem item in Sink) {
                worker.RemoveCollapsed (item, Sink);
                worker.Remove (item);
            }

            CommitRemove (worker);

            return worker.Affected;
        }

        public ICollection<TItem> Hide (IEnumerable<TItem> elements) {

            var worker = new WalkerWorker1<TItem, TEdge> (this.Sink);

            foreach (var item in elements) {
                worker.Remove (item);
                foreach (var edge in this.Sink.Twig (item)) {
                    worker.Remove (edge);
                }
            }

            RevoveEdgesOfChanged (worker);

            CommitRemove (worker);
            //List<TItem> changes = new List<TItem>(WalkerWorker.Changes);
            //WalkerWorker.Changes.Clear();

            //WalkerWorker.RemoveOrphans(this.View);
            //CommitRemove(WalkerWorker);
            //changes.AddRange(WalkerWorker.Changes);

            //WalkerWorker.Changes.Clear();
            return worker.Affected;

        }

        protected virtual void CommitRemove (WalkerWorker1<TItem, TEdge> worker) {
            foreach (var item in worker.Affected) {
                worker.Graph.Remove (item);
            }
        }

    }
}