/*
 * Limaki 
 * Version 0.071
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
using Limaki.Graphs;
using Limaki.Common;

namespace Limaki.Tests.Sandbox.Graph {
    public class FilteredGraph<TItem, TEdge> : GraphBase<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        IGraph<TItem, TEdge> _source = null;
        public IGraph<TItem, TEdge> Source {
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
                if (ItemFilter(item)) {
                    yield return item;
                }
            }
        }
    }
}