using System;
using System.Collections;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Common;

namespace Limaki.Graphs {
    /// <summary>
    /// an abstract base class to implement Graphs 
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public abstract class GraphBase<TItem, TEdge> : IGraph<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        private static Nullable<bool> _edgeIsItem = null;
        public static bool EdgeIsItemClazz {
            get {
                if (_edgeIsItem == null) {
                    _edgeIsItem = typeof(TItem).IsAssignableFrom(typeof(TEdge));
                }
                return _edgeIsItem.Value;
            }
        }

        public virtual bool EdgeIsItem { get { return EdgeIsItemClazz; } }

        private static Nullable<bool> _itemIsStorable = null;
        protected static bool ItemIsStorableClazz {
            get {
                if (_itemIsStorable == null) {
                    _itemIsStorable = Reflector.IsStorable(typeof(TItem));
                }
                return _itemIsStorable.Value;
            }
        }

        protected static TEdge NullEdge = default(TEdge);

        public virtual bool ItemIsStorable {
            get { return ItemIsStorableClazz; }
        }

        private Action<TItem> _dataChanged=null;
        public virtual Action<TItem> DataChanged {
            get { return _dataChanged; }
            set { _dataChanged = value; }
        }

        public virtual void OnDataChanged( TItem item ) {
            if (DataChanged != null) {
                DataChanged(item);
            }
        }

        #region TEdge
        //public static IEnumerable<TEdge> emptyEgdes {
        //    get { yield break; }
        //}

        public static ICollection<TEdge> emptyEgdes = new EmptyCollection<TEdge> ();

        protected abstract void AddEdge(TEdge edge, TItem item);
        protected abstract bool RemoveEdge(TEdge edge, TItem item);
        protected void CheckEdge(TEdge edge) {
            if (ItemIsStorable &&(edge.Root == null || edge.Leaf == null)) {
                string m = "Edge " + edge;
                
                throw new ArgumentException(m+"\tRoot or Leaf is null");
            }
        }
         
        public virtual void ChangeEdge(TEdge edge, TItem oldItem, TItem newItem) {
            CheckEdge(edge);
            if (oldItem == null || newItem == null ) {
                throw new ArgumentException();
            }
            if (edge.Root.Equals(oldItem)) {
                edge.Root = newItem;
            } else if (edge.Leaf.Equals(oldItem)) {
                edge.Leaf = newItem;
            }
            if (this.Contains(edge)) {
                if (!this.Contains(newItem)) {
                    this.Add(newItem);
                }
                RemoveEdge(edge, oldItem);
                AddEdge(edge, newItem);
            } else {
                this.Add (edge);
            }
        }

        #endregion

        #region abstract IGraph<TItem,TEdge> Member

        public abstract bool Contains(TEdge edge);

        public abstract void Add(TEdge edge);

        public abstract bool Remove(TEdge edge);

        public abstract int EdgeCount(TItem item);

        public abstract ICollection<TEdge> Edges(TItem item);

        public abstract IEnumerable<TEdge> Edges();

        public abstract IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges();

        public abstract void Add(TItem item);

        public abstract void Clear();

        public abstract bool Contains(TItem item);
        public abstract void CopyTo(TItem[] array, int arrayIndex);
        public abstract int Count { get;}
        public abstract bool IsReadOnly { get;}

        public abstract bool Remove(TItem item);

        public abstract IEnumerator<TItem> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        #endregion

        # region Basic Algorithms

        public virtual IEnumerable<TEdge> Edges ( IEnumerable<TItem> source ) {
            Set<TEdge> done = new Set<TEdge>();
            foreach(TItem item in source) {
                foreach (TEdge edge in this.Edges(item)) {
                    if (! done.Contains(edge)) {
                        done.Add(edge);
                        yield return edge;
                    }
                }
            }
        }

        public virtual IEnumerable<TEdge> Twig ( TItem source ) {
            Queue<IEnumerable<TEdge>> work = new Queue<IEnumerable<TEdge>>();
            work.Enqueue(Edges(source));
            while ( work.Count > 0 ) {
                IEnumerable<TEdge> curr = work.Dequeue();
                foreach ( TEdge edge in curr ) {
                    if ( EdgeIsItem ) {
                        TItem edgeAsItem = (TItem) (object) edge;
                        work.Enqueue(Edges(edgeAsItem));
                    }
                    yield return edge;
                }
            }
        }

        public virtual IEnumerable<TEdge> DepthFirstTwig(TItem source) {
            Stack<IEnumerable<TEdge>> work = new Stack<IEnumerable<TEdge>>();
            Set<TEdge> done = new Set<TEdge>();
            work.Push(Edges(source));
            while ( work.Count>0 ) {
                IEnumerable<TEdge> curr = work.Pop();
                foreach ( TEdge edge in curr ) {
                    if (EdgeIsItem && ! done.Contains(edge)) {
                        done.Add(edge);
                        ICollection<TEdge> next = Edges((TItem) (object) edge);
                        if (next.Count !=0) {
                            work.Push(next);
                        }
                    }
                    yield return edge;
                }
            }
        }

        public virtual IEnumerable<TEdge> PostorderTwig(TItem source) {
            foreach (TEdge edge in Edges(source)) {
                if (EdgeIsItem) {
                    TItem edgeAsItem = (TItem)(object)edge;
                    foreach ( TEdge recurse in PostorderTwig(edgeAsItem) ) {
                        yield return recurse;
                    }
                }
                yield return edge;
            }
        }



        /// <summary>
        /// NOT TESTED!!!!
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEdge> PreorderLatticeEdges(TItem root) {
            if (EdgeIsItem) {
                Set<TItem> visited = new Set<TItem>();
                foreach (TEdge edge in Edges(root)) {
                    TItem edgeAsItem = (TItem)(object)edge;
                    if (!visited.Contains(edgeAsItem)) {
                        visited.Add(edgeAsItem);
                        yield return edge;
                        if (root.Equals(edge.Root))
                            foreach (TEdge recurse in PreorderLatticeEdges(edge.Leaf)) {
                                yield return recurse;
                            }
                        if (root.Equals(edge.Leaf))
                            foreach (TEdge recurse in PreorderLatticeEdges(edge.Root)) {
                                yield return recurse;
                            }
                        foreach (TEdge recurse in PreorderLatticeEdges(edgeAsItem)) {
                            yield return recurse;
                        }
                    }
                }
            }
        }


        public virtual IEnumerable<TEdge> Fork (TItem source) {
            Queue<TEdge> work = new Queue<TEdge>();
            Set<TEdge> done = new Set<TEdge>();
            if (source is TEdge) {
                TEdge curr = (TEdge)(object)source;
                work.Enqueue (curr);
            }

            foreach (TEdge edge in this.Edges(source)) {
                work.Enqueue(edge);
            }

            while (work.Count > 0) {
                TEdge curr = work.Dequeue();
                if (!done.Contains(curr)) {
                    if (curr.Root is TEdge) {
                        work.Enqueue((TEdge)(object)curr.Root);
                    }
                    if (curr.Leaf is TEdge) {
                        work.Enqueue((TEdge)(object)curr.Leaf);
                    }

                    done.Add(curr);

                    yield return curr;
                }
            }
        }

        public virtual IEnumerable<TEdge> Vein(TEdge source) {
            Queue<TEdge> work = new Queue<TEdge>();
            Set<TEdge> done = new Set<TEdge>();
            work.Enqueue(source);
            
            while (work.Count > 0) {
                TEdge curr = work.Dequeue();
                if (!done.Contains(curr)) {
                    if (curr.Root is TEdge) {
                        work.Enqueue((TEdge)(object)curr.Root);
                    }
                    if (curr.Leaf is TEdge) {
                        work.Enqueue((TEdge)(object)curr.Leaf);
                    }

                    done.Add(curr);

                    yield return curr;
                }
            }
        }

        public virtual IEnumerable<TItem> Foliage(IEnumerable<TEdge> edges) {
            ICollection<TItem> done = new Set<TItem>();
            foreach (TEdge edge in edges) {
                TItem result = edge.Root;
                if (!(result is TEdge) && !done.Contains(result)) {
                    done.Add (result);
                    yield return result;
                }
                result = edge.Leaf;
                if (!(result is TEdge) && !done.Contains(result)) {
                    done.Add(result);
                    yield return result;
                }
            }
        }

        #endregion
    }
}