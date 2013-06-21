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
using Limaki.Common.Collections;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace Limaki.Graphs {
    /// <summary>
    /// a graph based on 
    /// IMultiDictionary to store TItems
    /// ICollection to store TEdge-Lists as IMultiDictionary.Value
    /// this class can be used directly with any IMultiDictionary and ICollection
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    /// <typeparam name="TMultiDictionary"></typeparam>
    /// <typeparam name="TCollection"></typeparam>
    public class MultiDictionaryGraph<TItem, TEdge, TMultiDictionary, TCollection> : GraphBase<TItem, TEdge>
        where TEdge: IEdge<TItem>
        where TMultiDictionary : class, IMultiDictionary<TItem, TEdge>,new()
        where TCollection : class, ICollection<TEdge>,new() {

        protected IMultiDictionary<TItem, TEdge> items = null;
        protected ICollection<TEdge> edges = null;

        public override bool ItemIsStorable {
            get { return true; }
        }

        public MultiDictionaryGraph() {
            items = new TMultiDictionary();
            edges = new TCollection ();
        } 

        # region TItem

        public override bool Contains(TItem item) {
            return items.Contains(item);
        }

        /// <summary>
        /// adds an item without edge
        /// value will be null
        /// </summary>
        /// <param name="item"></param>
        public override void Add(TItem item) {
            if (! items.Contains(item)) {
                if (EdgeIsItem) {
                    if (( (object) item ) is TEdge) {
                        this.Add ((TEdge) (object) item);
                    }
                }
                items.Add(item, null);
            }
        }

        public override bool Remove(TItem item) {
            if (EdgeIsItem && item is TEdge) {
                this.RemoveEdge ((TEdge) (object)item);
            }
            if (this.Contains(item)) {
                new List<TEdge>(DepthFirstTwig(item))
                    .ForEach(delegate(TEdge edge) { Remove(edge); });
                return items.Remove(item);
            } else {
                return false;
            }
        }

        #endregion

        # region TEdge

        public override bool Contains(TEdge edge) {
            return edges.Contains(edge);
        }

        protected override void AddEdge(TEdge edge, TItem item) {
            if (item != null) {
                ICollection<TEdge> list = items[item];
                bool isKeyOnly = list.Count == 0;
                if (!list.Contains(edge)) {
                    list.Add (edge);
                    if (isKeyOnly)
                        items[item] = list;
                    if (EdgeIsItem) {
                        if (( (object) item ) is TEdge) {
                            this.Add ((TEdge) (object) item);
                        }
                    }
                }
            }
        }

        public override void Add(TEdge edge) {
            CheckEdge(edge);
            if (! Contains(edge)) {
                AddEdge(edge,edge.Root);
                AddEdge(edge,edge.Leaf);
                edges.Add (edge);
            }
        }


        /// <summary>
        /// removes edge out of item.edges
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool RemoveEdge(TEdge edge, TItem item) {
            bool result = false;
            if ( item != null ) {
                ICollection<TEdge> list;
                if (items.TryGetValue(item, out list)) {
                    if (list != null) {
                        result = list.Remove(edge);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// removes edge out of edge.Root.edges and edge.Leaf.edges
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        protected virtual bool RemoveEdge(TEdge edge) {
            if (this.Contains(edge)) {
                RemoveEdge(edge, edge.Root);
                RemoveEdge(edge, edge.Leaf);
                return this.edges.Remove(edge);
            } else {
                return false;
            }
        }

        public override void RevertEdge(TEdge edge) {
            base.RevertEdgeInternal (edge);
        }

        public override bool Remove(TEdge edge) {
            bool result = RemoveEdge (edge);
            if (EdgeIsItem) {
                this.Remove((TItem)(object)edge);
            }
            return result;
        }

        public override int EdgeCount(TItem item) {
            int result = 0;
            ICollection<TEdge> list;
            if (items.TryGetValue (item, out list)) {
                if (list != null) {
                    result = list.Count;
                }
            }
            return result;
        }

        public override ICollection<TEdge> Edges(TItem item) {
            ICollection<TEdge> list = null;
            if (items.TryGetValue(item, out list)) {
                if (list != null) {
                    return list;
                }
            }
            return EmptyEgdes;
        }

        public override IEnumerable<TEdge> Edges() {
            return edges; 
        }

        /// <summary>
        /// returns a KVP of all Items which have Edges
        /// if an item has no edges, it will NOT be listed
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges() {
            foreach (KeyValuePair<TItem, ICollection<TEdge>> kvp in items) {
                if ((kvp.Value != null) && (kvp.Value.Count != 0))
                    yield return kvp;
           }
       }

        #endregion

        public override void Clear() {
            items.Clear ();
            edges.Clear ();
        }

        public override int Count {
            get { return items.Keys.Count; }
        }

        public override bool IsReadOnly {
            get { return items.IsReadOnly; }
        }


        #region ICollection<T> Member


        public override void CopyTo(TItem[] array, int arrayIndex) {
            items.Keys.CopyTo (array, arrayIndex);
        }


        public override IEnumerator<TItem> GetEnumerator() {
            foreach (var item in items.Keys)
                if (EdgeIsItem && !(item is TEdge))
                    yield return item;

            if (EdgeIsItem) {
                foreach (var item in Edges()) {
                    yield return (TItem)(object)item;
                }
            }
        }

        #endregion

        #region Linqishing

        //TODO: make a expression-cache here:

        public override  IEnumerable<TItem> Where(Expression<Func<TItem, bool>> predicate) {
            var clause = predicate.Compile();
            var result = items.Keys.Where(clause);
            if (EdgeIsItemClazz)
                result.Concat(edges.Cast<TItem>().Where(clause));
            return result;

            //foreach (var item in items.Keys.Where(clause))
            //    yield return item;

            //if(EdgeIsItemClazz)
            //    foreach (var item in edges.Cast<TItem>().Where(clause))
            //        yield return item;
        }

        #endregion 

    }


}
