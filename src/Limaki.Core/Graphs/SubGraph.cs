/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common.Collections;

namespace Limaki.Graphs {
    /// <summary>
    /// a pair of two graphs of same type
    /// Source is a Graph which holds all items
    /// Sink is a a subgraph of Source
    /// every operation (add, remove etc.) is performed on both graphs
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class SubGraph<TItem, TEdge> : GraphBase<TItem, TEdge>, IGraphPair<TItem, TItem, TEdge,TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public SubGraph(){}

        public SubGraph(IGraph<TItem,TEdge> source,  IGraph<TItem,TEdge> sink ) {
            this.Source = source;
            this.Sink = sink;
        }

        /// <summary>
        /// the sub-graph of Source
        /// </summary>
        public virtual IGraph<TItem, TEdge> Sink { get; set; }

        /// <summary>
        /// the full graph
        /// </summary>
        public virtual IGraph<TItem, TEdge> Source { get; set; }


        protected override void AddEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge (TEdge sinkEdge, TItem newItem, bool changeRoot) {

            Func<TItem> getItem = () => changeRoot ? sinkEdge.Root : sinkEdge.Leaf;
            Action<TItem> setItem = item => { if (changeRoot) sinkEdge.Root = item; else sinkEdge.Leaf = item; };

            var itemBefore = getItem ();

            Source.ChangeEdge (sinkEdge, newItem, changeRoot);
            var lockObj = (object) sinkEdge;
            lock (lockObj) {
                // revert the changes, otherwise edge is not removed from Sink.Edges(itemBefore)
                setItem (itemBefore);

                Sink.ChangeEdge (sinkEdge, newItem, changeRoot);

                // ensure the changes are done
                if (!object.Equals (getItem (), newItem))
                    setItem (newItem);
            }
        }

        public override void RevertEdge(TEdge edge) {
            var rootBefore = edge.Root;
            var leafBefore = edge.Leaf;
            Sink.RevertEdge(edge);

            edge.Root = rootBefore;
            edge.Leaf = leafBefore;
            Source.RevertEdge(edge);
        }

        public override bool Contains(TEdge edge) {
            return Sink.Contains(edge);
        }

        public override void Add(TEdge edge) {
            Source.Add(edge);
            Sink.Add(edge);
        }

        public override bool Remove(TEdge edge) {
            Source.Remove(edge);
            bool result = Sink.Remove(edge);
            return result;
        }

        public override int EdgeCount(TItem item) {
            return Sink.EdgeCount(item);
        }

        public override ICollection<TEdge> Edges(TItem item) {
            return Sink.Edges(item);
        }

        public override IEnumerable<TEdge> Edges() {
            return Sink.Edges();
        }

        public override IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges() {
            return Sink.ItemsWithEdges();
        }

        public override void Add(TItem item) {
            Sink.Add(item);
            Source.Add(item);
        }

        public override void Clear() {
            Sink.Clear();
        }

        public override bool Contains(TItem item) {
            return Sink.Contains(item);
        }

        public override void CopyTo(TItem[] array, int arrayIndex) {
            Sink.CopyTo(array, arrayIndex);
        }

        public override int Count {
            get { return Sink.Count; }
        }

        public override bool IsReadOnly {
            get { return Sink.IsReadOnly; }
        }

        public override bool Remove(TItem item) {
            Source.Remove(item);
            bool result = Sink.Remove(item);
            return result;
        }

        public virtual bool RemoveSinkItem (TItem item) {
            Func<IGraph<TItem, TEdge>, bool> remove = graph => {
                var sinkGraph = graph as ISinkGraph<TItem, TEdge>;
                if (sinkGraph == null)
                    return graph.Remove (item);
                else
                    return sinkGraph.RemoveSinkItem (item);
            };
            remove (Source);
            return remove (Sink);
        }

        public override IEnumerator<TItem> GetEnumerator() {
            return Sink.GetEnumerator();
        }

        public override void DoChangeData(TItem item, object data) {
            Source.DoChangeData(item, data);
            base.DoChangeData(item, data);
        }

        public override void OnGraphChange (object sender, GraphChangeArgs<TItem, TEdge> args) {
            // change the full graph first, then call subgraph change-event
            Source.OnGraphChange (sender, args);
            base.OnGraphChange (sender, args);
        }

        #region IGraphPair<TItem,TEdge,TItem,TEdge> Member

        IDictionary<TItem, TItem> IGraphPair<TItem, TItem, TEdge, TEdge>.Sink2Source {
            get { return new DictionaryAdapter<TItem> (this.Source); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        IDictionary<TItem, TItem> IGraphPair<TItem, TItem, TEdge, TEdge>.Source2Sink {
            get { return new DictionaryAdapter<TItem>(this.Source); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        GraphMapper<TItem, TItem, TEdge, TEdge> IGraphPair<TItem, TItem, TEdge, TEdge>.Mapper {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual IEnumerable<TEdge> ComplementEdges(TItem item, IGraph<TItem, TEdge> graph) {
            throw new NotImplementedException("ComplementEdges not implemented in "+this.GetType().Name);
        }

        public virtual TItem Get(TItem a) { return a; }

        public void UpdateSink (TItem a) {
            var sinkGraph = Source as ISinkGraph<TItem, TEdge>;
            if (sinkGraph != null)
                sinkGraph.UpdateSink (a);
        }

        #endregion

        Action<TItem> IFactoryListener<TItem>.ItemCreated { get; set; }

        public override IEnumerable<TItem> WhereQ(System.Linq.Expressions.Expression<Func<TItem, bool>> predicate) {
            return Sink.WhereQ(predicate);
        }

    }
}