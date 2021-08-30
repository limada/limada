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
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Graphs;

namespace Limaki.Data {
    
    public abstract class DbGraph<TItem, TEdge> : GraphBase<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        # region EdgeList-Cache
        IDictionary<TItem, ICollection<TEdge>> _edgesCache = null;
        protected IDictionary<TItem, ICollection<TEdge>> EdgesCache{
            get {
                if (_edgesCache == null) {
                    _edgesCache = new Dictionary<TItem, ICollection<TEdge>> ();
                }
                return _edgesCache;
            }
        }

        protected virtual ICollection<TEdge> GetCached(TItem item) {
            ICollection<TEdge> result = null;
            if (item != null)
                EdgesCache.TryGetValue(item, out result);
            return result;
        }

        protected virtual void SetCached(TItem item, ICollection<TEdge> edges) {
            if (item != null) {
                if (edges != null) {
                    EdgesCache[item] = edges;
                } else {
                    EdgesCache.Remove(item);
                }
            }
        }

        protected virtual void ClearCaches() {
            _edgesCache = null;
        }

        #endregion

        protected abstract void Upsert(TItem item);
        protected abstract void Upsert(TEdge edge);
        protected abstract void Delete(TItem item);
        protected abstract void Delete(TEdge edge);

        /// <summary>
        /// removes item from the db reference system, but not in the database
        /// </summary>
        /// <param name="item"></param>
        public abstract void EvictItem (TItem item);

        public abstract void Flush();

        public virtual void Close() {
            ClearCaches ();
        }

        protected abstract ICollection<TEdge> EdgesOf(TItem item);
        protected abstract IEnumerable<TItem> Items { get; }

        public override void Add (TItem item) {
            // TODO: Prove what happens if item is TEdge
            if (item != null)
                try {
                    Upsert (item);
                } catch (Exception e) {
                    throw e;
                } finally { }
        }

        public override void Add(TEdge edge) {
            if (edge != null)
                try {
                    CheckEdge(edge);
                    AddEdge(edge, edge.Root);
                    AddEdge(edge, edge.Leaf);
                    Upsert(edge);
                } catch (Exception e) {
                    throw e;
                } finally { }
        }

        protected override void AddEdge(TEdge edge, TItem item) {
            if (item != null) {
                if (!Contains(item)) {
                    if (EdgeIsItem && ((object) item) is TEdge) {
                        Add ((TEdge) (object) item);
                    } else {
                        Add (item);
                    }
                }
                SetCached(item, null);
            }
        }

        public override void ChangeEdge(TEdge sinkEdge, TItem newItem, bool changeRoot) {
            var oldItem = default(TItem);
            if (changeRoot) {
                oldItem = sinkEdge.Root;
                sinkEdge.Root = newItem;
            } else {
                oldItem = sinkEdge.Leaf;
                sinkEdge.Leaf = newItem;
            }
            if (this.Contains(sinkEdge)) {
                if (!this.Contains(newItem)) {
                    this.Add(newItem);
                }
                SetCached(newItem, null);
                SetCached(oldItem, null);
                Upsert(sinkEdge);
            } else {
                this.Add(sinkEdge);
            }
        }

        public override void RevertEdge(TEdge edge) {
            base.RevertEdgeInternal(edge);
            RemoveCached (edge);
            Upsert(edge);
        }

        protected override bool RemoveEdge(TEdge edge, TItem item) {
            if (edge is TItem) {
                return RemoveCached ((TItem)(object)edge);
            }
            return RemoveCached (edge);
        }

        protected virtual bool RemoveCached(TEdge edge) {
            SetCached (edge.Leaf, null);
            SetCached (edge.Root, null);
            return true;
        }

        protected virtual bool RemoveCached (TItem item) {
            if (item is TEdge) {
                RemoveCached ((TEdge) (object) item);
            }
            SetCached (item, null);
            return true;
        }

        public override ICollection<TEdge> Edges(TItem item) {
            if (item != null) {
                var result = GetCached(item);
                if (result == null) {
                    result = EdgesOf(item);
                    SetCached(item, result);
                }
                return result;
            } else {
                return new EmptyCollection<TEdge>();
            }
        }

        public override bool Remove(TEdge edge) {
            bool result = false;
            
            if (edge == null)
                return result;

            try {
                if (Contains(edge)) {
                    if (EdgeIsItemClazz) {
                        object o = edge;
                        result = this.Remove((TItem)o);
                    } else {
                        Delete(edge);
                        result = true;
                    }
                    RemoveCached(edge);
                }
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public override bool Remove(TItem item) {
            if (item == null)
                return false;
            bool result = false;
            if (EdgeIsItem && item is TEdge) {
                RemoveCached((TEdge)(object)item);
            }
            try {
                var contained = Contains(item);
                if (contained || !ItemIsStorableClazz) {
                    foreach (var edge in DepthFirstTwig(item)) {
                        if (Remove(edge))
                            result = true; // Attention! lazy evaluation!
                    }
                }
                if (contained) {
                    Delete(item);
                    result = true;
                }
                SetCached(item, null);
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }

        public override IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges () {
            foreach (var item in this) {
                var result = Edges (item);
                if (result.Count != 0) {
                    yield return new KeyValuePair<TItem, ICollection<TEdge>> (item, result);
                }
            }
        }
    }
}