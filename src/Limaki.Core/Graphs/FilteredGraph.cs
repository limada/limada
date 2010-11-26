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


using System;
using System.Collections.Generic;
using Limaki.Common.Collections;

namespace Limaki.Graphs {
    public class FilteredGraph<TItem, TEdge> : GraphBase<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        public FilteredGraph(IGraph<TItem, TEdge> source) {
            this.Source = source;
        }

        IGraph<TItem, TEdge> _source = null;
        public virtual IGraph<TItem, TEdge> Source {
            get { return _source; }
            set { _source = value; }
        }

        public Predicate<TEdge> EdgeFilter = null;
        public Predicate<TItem> ItemFilter = null;

        protected override void AddEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge(TEdge edge, TItem oldItem, TItem newItem) {
            if (EdgeFilter(edge) && ItemFilter(newItem))
                Source.ChangeEdge(edge, oldItem, newItem);
        }

        public override void RevertEdge(TEdge edge) {
            Source.RevertEdge (edge);
        }

        public override bool Contains(TEdge edge) {
            return EdgeFilter(edge) && Source.Contains(edge);
        }

        public override void Add(TEdge edge) {
            if (EdgeFilter(edge)) {
                Source.Add(edge);
            }
        }

        public override bool Remove(TEdge edge) {
            return EdgeFilter(edge) && Source.Remove(edge);
        }

        public override int EdgeCount(TItem item) {
            int result = 0;
            foreach (TEdge edge in Edges(item)) {
                result++;
            }
            return result;
        }

        public override ICollection<TEdge> Edges(TItem item) {
            return new FilteredCollection<TEdge>(Source.Edges(item), EdgeFilter);
        }

        public override IEnumerable<TEdge> Edges() {
            foreach (TEdge edge in Source.Edges()) {
                if (EdgeFilter(edge)) {
                    yield return edge;
                }
            }
        }

        public override TItem Adjacent(TEdge edge, TItem item) {
            TItem result =  base.Adjacent(edge, item);
            if (!ItemFilter(result))
                return default(TItem);
            if (result is TEdge && !EdgeFilter((TEdge)(object)result))
                return default( TItem );

            return result;
        }
        
        public override IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges() {
            foreach (KeyValuePair<TItem, ICollection<TEdge>> pair in Source.ItemsWithEdges()) {
                if (ItemFilter(pair.Key)) {
                    yield return new KeyValuePair<TItem, ICollection<TEdge>>(
                        pair.Key,
                        new FilteredCollection<TEdge>(pair.Value, EdgeFilter));
                }
            }
        }

        public override void Add(TItem item) {
            if (ItemFilter(item)) {
                Source.Add(item);
            }
        }

        public override void Clear() {
            Set<TItem> remove = new Set<TItem>();
            foreach (TItem item in this) {
                if (ItemFilter(item))
                    remove.Add(item);
            }
            foreach (TItem item in remove)
                Remove(item);
        }

        public override bool Contains(TItem item) {
            return ItemFilter(item) && Source.Contains(item);
        }

        public override void CopyTo(TItem[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int Count {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override bool IsReadOnly {
            get { return Source.IsReadOnly; }
        }

        public override bool Remove(TItem item) {
            return ItemFilter(item) && Source.Remove(item);
        }

        public override IEnumerator<TItem> GetEnumerator() {
            foreach (TItem item in Source) {
                if (item is TEdge) {
                    if(EdgeFilter((TEdge)(object)item)) {
                        yield return item;
                    }
                } else {
                    if (ItemFilter (item)) {
                        yield return item;
                    }
                }
            }
        }

        public override bool RootIsEdge(TEdge curr) {
            return Source.RootIsEdge(curr);
        }

        public override bool LeafIsEdge(TEdge curr) {
            return Source.LeafIsEdge(curr);
        }

        public override void OnChangeData(TItem item, object data) {
            Source.OnChangeData (item, data);
            base.OnChangeData(item, data);
        }

        public override void OnGraphChanged(TItem item, GraphChangeType changeType) {
            base.OnGraphChanged(item, changeType);
            Source.OnChangeData (item, changeType);
        }

        public override void OnDataChanged(TItem item) {
            base.OnDataChanged(item);
            Source.OnDataChanged (item);
        }
    }
}