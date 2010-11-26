using System;
using System.Collections;
using System.Collections.Generic;
using Limaki.Common.Collections;

namespace Limaki.Graphs {
    public abstract class GraphBase<TItem, TEdge> : IGraph<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        protected static TEdge NullEdge = default(TEdge);

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
        public static bool ItemIsStorableClazz {
            get {
                if (_itemIsStorable == null) {
                    _itemIsStorable = !(typeof(TItem).IsPrimitive || typeof(TItem) == typeof(string));
                }
                return _itemIsStorable.Value;
            }
        }

        public virtual bool ItemIsStorable {
            get { return true; }
        }

        #region TEdge
        public static IEnumerable<TEdge> emptyEgdes {
            get { yield break; }
        }

        protected abstract void AddEdge(TEdge edge, TItem item);
        protected abstract bool RemoveEdge(TEdge edge, TItem item);

        public virtual void ChangeEdge(TEdge edge, TItem oldItem, TItem newItem) {
            if (this.Contains(edge)) {
                if (!this.Contains(newItem)) {
                    this.Add(newItem);
                }
                RemoveEdge(edge, oldItem);
                AddEdge(edge, newItem);
            }
        }

        #endregion

        #region abstract IGraph<TItem,TEdge> Member

        public abstract bool Contains(TEdge edge);

        public abstract void Add(TEdge edge);

        public abstract bool Remove(TEdge edge);

        public abstract int EdgeCount(TItem item);

        public abstract IEnumerable<TEdge> Edges(TItem item);

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

        public virtual IEnumerable<TEdge> PreorderEdges(TItem source) {
            foreach (TEdge edge in Edges(source)) {
                yield return edge;
                if (EdgeIsItem) {
                    TItem edgeAsItem = (TItem)(object)edge;
                    foreach (TEdge recurse in PreorderEdges(edgeAsItem)) {
                        yield return recurse;
                    }
                }
            }
        }
        public virtual IEnumerable<TEdge> PostorderEdges(TItem source) {
            foreach (TEdge edge in Edges(source)) {
                if (EdgeIsItem) {
                    TItem edgeAsItem = (TItem)(object)edge;
                    foreach (TEdge recurse in PreorderEdges(edgeAsItem)) {
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
        #endregion


    }
}