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
using System.Collections;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Common.Collections;
using System.Linq.Expressions;
using Limaki.Common.Reflections;

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
        
        #region TEdge

        public static ICollection<TEdge> EmptyEgdes = new EmptyCollection<TEdge> ();

        protected abstract void AddEdge(TEdge edge, TItem item);
        protected abstract bool RemoveEdge(TEdge edge, TItem item);

        public virtual bool ValidEdge(TEdge edge) {
            return !ItemIsStorable || (edge.Root != null && edge.Leaf != null);
        }

        public virtual bool HasSingleEdge (TItem item) {
            var first = false;
            foreach (var link in Edges(item)) {
                if (first)
                    return false;
                first = true;
            }
            return first;
        }

        protected void CheckEdge(TEdge edge) {
            if (!ValidEdge(edge)) {
                var m =  "Edge " + edge + "\tRoot or Leaf is null";
                throw new CorruptedLinkException<TItem, TEdge>(edge, m);
            }
        }

        public virtual void ChangeEdge(TEdge sinkEdge, TItem newItem, bool changeRoot) {
            CheckEdge(sinkEdge);
            if (newItem == null ) {
                throw new ArgumentException("ChangeEdge: Root or Leaf would be null");
            }
            var oldItem = default(TItem);
            var adjacent = default(TItem);
            if (changeRoot) {
                oldItem = sinkEdge.Root;
                adjacent = sinkEdge.Leaf;
                sinkEdge.Root = newItem;
            } else {
                oldItem = sinkEdge.Leaf;
                adjacent = sinkEdge.Root;
                sinkEdge.Leaf = newItem;
            }
            if (this.Contains(sinkEdge)) {
                if (!this.Contains(newItem)) {
                    this.Add(newItem);
                }
                if(!adjacent.Equals(oldItem))
                    RemoveEdge(sinkEdge, oldItem);
                AddEdge(sinkEdge, newItem);
            } else {
                this.Add(sinkEdge);
            }
        }

        protected virtual void RevertEdgeInternal ( TEdge edge ) {
            var root = edge.Root;
            edge.Root = edge.Leaf;
            edge.Leaf = root;
        }

        public abstract void RevertEdge ( TEdge edge );

        public virtual TItem Adjacent(TEdge edge, TItem item) { return edge.Adjacent (item); }

        #endregion

        #region abstract IGraph<TItem,TEdge> Member

        public abstract bool Contains(TEdge edge);

        public abstract void Add(TEdge edge);

        public abstract bool Remove(TEdge edge);

        public abstract int EdgeCount(TItem item);

        public abstract ICollection<TEdge> Edges(TItem item);

        public abstract IEnumerable<TEdge> Edges();

        public abstract IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges();

        public abstract bool Contains(TItem item);
        public abstract void Add(TItem item);
        public abstract bool Remove(TItem item);
        public abstract int Count { get; }
        public abstract IEnumerator<TItem> GetEnumerator();

        public abstract void Clear();
        public abstract void CopyTo(TItem[] array, int arrayIndex);
        public abstract bool IsReadOnly { get;}

        IEnumerator IEnumerable.GetEnumerator() { return this.GetEnumerator(); }

        #endregion

        # region Basic Algorithms

        public virtual IEnumerable<TEdge> Edges ( IEnumerable<TItem> source ) {
            var done = new Set<TEdge>();
            foreach(var item in source) {
                foreach (var edge in this.Edges(item)) {
                    if (! done.Contains(edge)) {
                        done.Add(edge);
                        yield return edge;
                    }
                }
            }
        }

        public virtual IEnumerable<TEdge> Twig ( TItem source ) {
            var work = new Queue<IEnumerable<TEdge>>();
            work.Enqueue(Edges(source));
            while ( work.Count > 0 ) {
                var curr = work.Dequeue();
                foreach ( var edge in curr ) {
                    if ( EdgeIsItem ) {
                        var edgeAsItem = (TItem) (object) edge;
                        work.Enqueue(Edges(edgeAsItem));
                    }
                    yield return edge;
                }
            }
        }

        public virtual IEnumerable<TEdge> DepthFirstTwig(TItem source) {
            var work = new Stack<IEnumerable<TEdge>>();
            var done = new Set<TEdge>();
            work.Push(Edges(source));
            while ( work.Count>0 ) {
                var curr = work.Pop();
                foreach ( var edge in curr ) {
                    if (EdgeIsItem && ! done.Contains(edge)) {
                        done.Add(edge);
                        var next = Edges((TItem) (object) edge);
                        if (next.Count !=0) {
                            work.Push(next);
                        }
                    }
                    yield return edge;
                }
            }
        }

        public virtual IEnumerable<TEdge> PostorderTwig(TItem source) {
            foreach (var edge in Edges(source)) {
                if (EdgeIsItem) {
                    var edgeAsItem = (TItem)(object)edge;
                    foreach ( var recurse in PostorderTwig(edgeAsItem) ) {
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
                var visited = new Set<TItem>();
                foreach (var edge in Edges(root)) {
                    var edgeAsItem = (TItem)(object)edge;
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

        public virtual bool RootIsEdge(TEdge curr) {
            return curr.Root is TEdge;
        }

        public virtual bool LeafIsEdge(TEdge curr) {
            return curr.Leaf is TEdge;
        }

        public virtual IEnumerable<TEdge> Fork (TItem source) {
            var work = new Queue<TEdge>();
            var done = new Set<TEdge>();
            if (source is TEdge) {
                var curr = (TEdge)(object)source;
                work.Enqueue (curr);
            }

            foreach (TEdge edge in this.Edges(source)) {
                work.Enqueue(edge);
            }

            while (work.Count > 0) {
                var curr = work.Dequeue();
                if (!done.Contains(curr)) {
                    if (RootIsEdge(curr)) {
                        work.Enqueue((TEdge)(object)curr.Root);
                    }
                    if (LeafIsEdge(curr)) {
                        work.Enqueue((TEdge)(object)curr.Leaf);
                    }

                    done.Add(curr);

                    yield return curr;
                }
            }
        }

        public virtual IEnumerable<TEdge> Fork(TItem item, Func<TItem,bool> pred) {
            foreach (var edge in Fork(item)) {
                if (pred(edge.Root) && pred(edge.Leaf)) {
                    yield return edge;
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
                    if (RootIsEdge(curr)) {
                        work.Enqueue((TEdge)(object)curr.Root);
                    }
                    if (LeafIsEdge(curr)) {
                        work.Enqueue((TEdge)(object)curr.Leaf);
                    }

                    done.Add(curr);

                    yield return curr;
                }
            }
        }

        public virtual IEnumerable<TItem> Foliage(IEnumerable<TEdge> edges) {
            ICollection<TItem> done = new Set<TItem>();
            foreach (var edge in edges) {
                var result = default(TItem);
                if (!(RootIsEdge(edge)) && !done.Contains(result=edge.Root)) {
                    done.Add(result);
                    yield return result;
                }
                if (!(LeafIsEdge(edge)) && !done.Contains(result=edge.Leaf)) {
                    done.Add(result);
                    yield return result;
                }
            }
        }

        #endregion

        #region events

        public virtual Action<IGraph<TItem, TEdge>, TItem, object> ChangeData { get; set; }

        public virtual void DoChangeData(TItem item, object data) {
            if (ChangeData != null) {
                ChangeData(this, item, data);
            }
        }
        
        public virtual Action<IGraph<TItem, TEdge>, TItem, GraphEventType> GraphChange0 { get; set; }

        public virtual Action<object, GraphChangeArgs<TItem, TEdge>> GraphChange { get; set; }

        public virtual void OnGraphChange (object sender, GraphChangeArgs<TItem, TEdge> args) {
            if (GraphChange != null) {
                GraphChange (sender, args);
            }
        }
        #endregion

        #region Linqishing

        public abstract IEnumerable<TItem> WhereQ(Expression<Func<TItem, bool>> predicate);

        #endregion
    }
}