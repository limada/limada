/*
 * Limaki 
 * Version 0.063
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
using System.Text;
using Limaki.Common.Collections;


namespace Limaki.Graphs {
    public class Graph<TItem, TEdge>:ICollection<TItem>
        where TEdge: IEdge<TItem> {
        protected MultiDictionary<TItem, TEdge> items = new MultiDictionary<TItem, TEdge>();
        protected Set<TEdge> edges = new Set<TEdge>();

        protected static TEdge NullEdge = default( TEdge );
        
        private static Nullable<bool> _edgeIsItem = null;
        public static bool EdgeIsItem {
            get {
                if (_edgeIsItem == null) {
                    _edgeIsItem = typeof(TItem).IsAssignableFrom(typeof(TEdge));
                }
                return _edgeIsItem.Value;
            }
        }

        # region TItem
        
        public bool Contains(TItem item) {
            return items.Contains(item);
        }

        /// <summary>
        /// adds an item without edge
        /// value will be null
        /// </summary>
        /// <param name="item"></param>
        public void Add(TItem item) {
            if (! items.Contains(item)) {
                items.Add(item,null);
            }
        }

        public bool Remove(TItem item) {
            if (this.Contains(item)) {
                new List<TEdge>(Edges(item))
                    .ForEach(delegate(TEdge edge) { Remove(edge); });
                return items.Remove(item);
            } else {
                return false;
            }
        }

        #endregion

        # region TEdge
        public bool Contains(TEdge edge) {
            return edges.Contains(edge);
        }

        protected void AddEdge(TEdge edge,TItem item) {
            if (item != null) {
                ICollection<TEdge> list = items[item];
                bool isKeyOnly = list.Count == 0;
                list.Add (edge);
                // TODO: give back a list which is aware of adding
                if (isKeyOnly)
                    items[item] = list;
            }
        }

        public void Add(TEdge edge) {
            if (! Contains(edge)) {
                AddEdge(edge,edge.Root);
                AddEdge(edge,edge.Leaf);
                edges.Add (edge);
            }
        }

        public void ChangeEdge(TEdge edge, TItem oldItem, TItem newItem) {
            if (this.Contains(edge)) {
                if (! this.Contains(newItem)) {
                    this.Add(newItem);
                }
                RemoveEdge (edge, oldItem);
                AddEdge (edge, newItem);
            }
        }

        public bool RemoveEdge(TEdge edge, TItem item) {
            bool result = false;
            ICollection<TEdge> list;
            if (items.Find(item, out list)) {
                if (list != null) {
                    result = list.Remove (edge);
                }
            }
            return result;
        }

        public bool Remove(TEdge edge) {
            if (this.Contains(edge)) {
                RemoveEdge (edge, edge.Root);
                RemoveEdge(edge, edge.Leaf);

                if ( EdgeIsItem ) {
                    object o = edge;
                    this.Remove ((TItem)o);
                }
                return this.edges.Remove (edge);
            } else {
                return false;
            }
        }

        public int EdgeCount(TItem item) {
            int result = 0;
            ICollection<TEdge> list;
            if (items.Find (item, out list)) {
                if (list != null) {
                    result = list.Count;
                }
            }
            return result;
        }

        //public ICollection<TEdge> EdgeList(TItem item) {
        //    ICollection<TEdge> list;
        //    if (items.Find(item, out list)) {
        //        if (list == null) {
        //            list = new EmptyCollection<TEdge> ();  
        //        } 
        //    }
        //    return new System.Collections.ObjectModel.ReadOnlyCollection<TEdge> (list);
        //}

        public IEnumerable<TEdge> Edges(TItem item) {
            ICollection<TEdge> list;
            if (items.Find(item, out list)) {
                if (list != null) {
                    foreach (TEdge edge in list) {
                        yield return edge;
                    }
                } else {
                    yield break;
                }
            } else {
                yield break;
            }
        }

        public IEnumerable<TEdge> Edges() {
            return edges; 
        }
        /// <summary>
        /// returns a KVP of all Items which have Edges
        /// if an item has no edges, it will NOT be listed
        /// </summary>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges() {
           foreach(C5.KeyValuePair<TItem, ICollection<TEdge>> kvp in items) {
               if ((kvp.Value!=null)&&(kvp.Value.Count != 0))
                   yield return new KeyValuePair<TItem,ICollection<TEdge>>(kvp.Key,kvp.Value);
           }
        }

        #endregion

        public void Clear() {
            items.Clear ();
            edges.Clear ();
        }

        public int Count {
            get { return items.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }


        #region ICollection<T> Member

        
        void ICollection<TItem>.CopyTo(TItem[] array, int arrayIndex) {
            items.Keys.CopyTo (array, arrayIndex);
        }


        #endregion

        #region IEnumerable<T> Member

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator() {
            return items.Keys.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return items.GetEnumerator ();
        }

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
                Set<TItem> visited = new Set<TItem> ();
                foreach (TEdge edge in Edges (root)) {
                    TItem edgeAsItem = (TItem) (object) edge;
                    if (!visited.Contains (edgeAsItem)) {
                        visited.Add (edgeAsItem);
                        yield return edge;
                        if (root.Equals (edge.Root))
                            foreach (TEdge recurse in PreorderLatticeEdges (edge.Leaf)) {
                                yield return recurse;
                            }
                        if (root.Equals (edge.Leaf))
                            foreach (TEdge recurse in PreorderLatticeEdges (edge.Root)) {
                                yield return recurse;
                            }
                        foreach (TEdge recurse in PreorderLatticeEdges (edgeAsItem)) {
                            yield return recurse;
                        }
                    }
                }
            }
        }
        #endregion


    }

    public class EmptyCollection<T>:ICollection<T> {

        #region ICollection<T> Member

        public void Add(T item) {}

        public void Clear() {}

        public bool Contains(T item) {
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex) {}

        public int Count { get {return 0;}}

        public bool IsReadOnly {get { return true; } }

        public bool Remove(T item) {return false; }

        #endregion

        #region IEnumerable<T> Member

        public IEnumerator<T> GetEnumerator() {
            yield break;
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator ();
        }

        #endregion
    }
}
