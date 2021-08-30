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
using System.Linq;

namespace Limaki.Graphs {

    public class FilteredGraph<TItem, TEdge> : GraphBase<TItem, TEdge>, IWrappedGraph<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        public FilteredGraph(IGraph<TItem, TEdge> source) {
            this.Source = source;
            source.GraphChange -= SourceGraphChanged;
            source.GraphChange += SourceGraphChanged;
        }

        public virtual IGraph<TItem, TEdge> Source { get; set; }

        public Func<TEdge, bool> EdgeFilter { get; set; }
        public Func<TItem, bool> ItemFilter { get; set; }

        protected override void AddEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge(TEdge sinkEdge, TItem newItem, bool changeRoot) {
            if (EdgeFilter(sinkEdge) && ItemFilter(newItem))
                Source.ChangeEdge(sinkEdge, newItem, changeRoot);
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
            foreach (var edge in Edges(item)) {
                result++;
            }
            return result;
        }

        public override ICollection<TEdge> Edges(TItem item) {
            return new FilteredCollection<TEdge>(Source.Edges(item), EdgeFilter);
        }

        public override IEnumerable<TEdge> Edges() {
            foreach (var edge in Source.Edges()) {
                if (EdgeFilter(edge)) {
                    yield return edge;
                }
            }
        }

        public override TItem Adjacent(TEdge edge, TItem item) {
            var result =  base.Adjacent(edge, item);
            if(object.Equals(result,default(TItem)))
                return default(TItem);
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
            var remove = new Set<TItem>();
            foreach (var item in this) {
                if (ItemFilter(item))
                    remove.Add(item);
            }
            foreach (var item in remove)
                Remove(item);
        }

        public override bool Contains(TItem item) {
            return ItemFilter(item) && Source.Contains(item);
        }

        public override void CopyTo (TItem[] array, int arrayIndex) {
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        /// <summary>
        /// this is expensive!
        /// </summary>
        public override int Count {
            get { int i = 0; foreach (var item in this) i++; return i; }
        }

        public override bool IsReadOnly {
            get { return Source.IsReadOnly; }
        }

        public override bool Remove(TItem item) {
            return ItemFilter(item) && Source.Remove(item);
        }

        public override IEnumerator<TItem> GetEnumerator () {
            foreach (var item in Source) {
                if (item is TEdge) {
                    if (EdgeFilter ((TEdge) (object) item)) {
                        yield return item;
                    }
                } else {
                    if (ItemFilter (item)) {
                        yield return item;
                    }
                }
            }
        }

        public override bool RootIsEdge(TEdge curr) =>  Source.RootIsEdge(curr);

        public override bool LeafIsEdge(TEdge curr) => Source.LeafIsEdge(curr);
        

        public override void DoChangeData(TItem item, object data) {
            Source.DoChangeData (item, data);
            base.DoChangeData(item, data);
        }

        public override void OnGraphChange (object sender, GraphChangeArgs<TItem, TEdge> args) {
            base.OnGraphChange (sender, args);
            Source.OnGraphChange (sender, args);
        }

        public virtual void SourceGraphChanged (object sender, GraphChangeArgs<TItem,TEdge> args) {
            base.OnGraphChange (sender, args);
        }

        public override IEnumerable<TItem> WhereQ(System.Linq.Expressions.Expression<Func<TItem, bool>> predicate) {
            return Source.WhereQ(predicate).Where(e => ItemFilter(e));
        }
    
    }

    public interface IWrappedGraph<TItem, TEdge> : IGraph<TItem, TEdge>
        where TEdge : IEdge<TItem> {
        IGraph<TItem, TEdge> Source { get; }
    }
}