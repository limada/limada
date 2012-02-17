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

        protected virtual ICollection<TEdge> getCached(TItem item) {
            ICollection<TEdge> result = null;
            if (item != null)
                EdgesCache.TryGetValue(item, out result);
            return result;
        }

        protected virtual void setCached(TItem item, ICollection<TEdge> edges) {
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

        protected abstract void Store(TItem item);
        protected abstract void Store(TEdge edge);
        protected abstract void Delete(TItem item);
        protected abstract void Delete(TEdge edge);

        public abstract void EvictItem ( TItem item );
        public abstract void Flush();
        public virtual void Close() {
            ClearCaches ();
        }

        protected abstract ICollection<TEdge> edges(TItem item);
        protected abstract IEnumerable<TItem> items { get; }

        public override void Add(TEdge edge) {
            if (edge != null)
                try {
                    CheckEdge(edge);
                    AddEdge(edge, edge.Root);
                    AddEdge(edge, edge.Leaf);
                    Store(edge);
                } catch (Exception e) {
                    throw e;
                } finally { }
        }

        protected override void AddEdge(TEdge edge, TItem item) {
            if (item != null) {
                if (!Contains(item)) {
                    Add(item);
                    if (EdgeIsItem) {
                        if (((object)item) is TEdge) {
                            this.Add((TEdge)(object)item);
                        }
                    }
                }
                setCached(item, null);

            }
        }

        public override void ChangeEdge(TEdge edge, TItem newItem, bool changeRoot) {
            TItem oldItem = default(TItem);
            if (changeRoot) {
                oldItem = edge.Root;
                edge.Root = newItem;
            } else {
                oldItem = edge.Leaf;
                edge.Leaf = newItem;
            }
            if (this.Contains(edge)) {
                if (!this.Contains(newItem)) {
                    this.Add(newItem);
                }
                setCached(newItem, null);
                setCached(oldItem, null);
                Store(edge);
            } else {
                this.Add(edge);
            }
        }

        public override void RevertEdge(TEdge edge) {
            base.RevertEdgeInternal(edge);
            setCached(edge.Leaf, null);
            setCached(edge.Root, null);
            Store(edge);
        }

        protected override bool RemoveEdge(TEdge edge, TItem item) {
            setCached(edge.Leaf, null);
            setCached(edge.Root, null);
            return true;
        }

        protected virtual bool RemoveInternal(TEdge edge) {
            RemoveEdge(edge, edge.Root);
            RemoveEdge(edge, edge.Leaf);
            return true;
        }

        public override ICollection<TEdge> Edges(TItem item) {
            if (item != null) {
                ICollection<TEdge> result = getCached(item);
                if (result == null) {
                    result = edges(item);
                    setCached(item, result);
                }
                return result;
            } else {
                return new EmptyCollection<TEdge>();
            }
        }

        public override IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges() {
            foreach (TItem item in this) {
                ICollection<TEdge> result = Edges(item);
                if (result.Count != 0) {
                    yield return new KeyValuePair<TItem, ICollection<TEdge>>(item, result);
                }
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
                    RemoveInternal(edge);
                }
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }


        public override void Add(TItem item) {
            // TODO: Prove what happens if item is TEdge
            if (item != null)
                try {
                    Store(item);
                } catch (Exception e) {
                    throw e;
                } finally { }
        }

        public override bool Remove(TItem item) {
            if (item == null)
                return false;
            bool result = false;
            if (EdgeIsItem && item is TEdge) {
                RemoveInternal((TEdge)(object)item);
            }
            try {
                bool contained = Contains(item);
                if (contained || !ItemIsStorableClazz) {
                    foreach (TEdge edge in DepthFirstTwig(item)) {
                        if (Remove(edge))
                            result = true; // Attention! lazy evaluation!
                    }
                }
                if (contained) {
                    Delete(item);
                    result = true;
                }
                setCached(item, null);
            } catch (Exception e) {
                throw e;
            } finally { }
            return result;
        }


    }
}