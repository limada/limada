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
    /// Sink is a a subgraph of Data
    /// every operation (add,remove etc.) is performed on both graphs
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
        /// the sub-graph of Source with the visible items
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

        public override void ChangeEdge(TEdge edge, TItem newItem, bool changeRoot) {
            TItem rootBefore = edge.Root;
            TItem leafBefore = edge.Leaf;

            Sink.ChangeEdge (edge, newItem, changeRoot);

            //edge.Root = rootBefore;
            //edge.Leaf = leafBefore;
            Source.ChangeEdge (edge, newItem, changeRoot);
        }

        public override void RevertEdge(TEdge edge) {
            TItem rootBefore = edge.Root;
            TItem leafBefore = edge.Leaf;
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
            //OnGraphChanged(edge, GraphChangeType.Add);
        }

        public override bool Remove(TEdge edge) {
            Source.Remove(edge);
            bool result = Sink.Remove(edge);
            //OnGraphChanged(edge, GraphChangeType.Remove);
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
            //OnGraphChanged(item, GraphChangeType.Add);
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
            //OnGraphChanged (item, GraphChangeType.Remove);
            return result;
        }

        public virtual bool RemoveInSink (TItem item) {
            if (item is TEdge)
                return Sink.Remove((TEdge) item);
            return Sink.Remove(item);
        }

        public override IEnumerator<TItem> GetEnumerator() {
            return Sink.GetEnumerator();
        }

        public override void OnDataChanged(TItem item) {
            // change the data graph first, then call view change-event
            Source.OnDataChanged (item);
            base.OnDataChanged(item);
        }

        public override void DoChangeData(TItem item, object data) {
            Source.DoChangeData(item, data);
            base.DoChangeData(item, data);
        }
        public override void OnGraphChanged( TItem item, GraphEventType eventType ) {
            Source.OnGraphChanged(item, eventType);
            base.OnGraphChanged(item, eventType);
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

        #endregion

        Action<TItem> IFactoryListener<TItem>.ItemCreated { get; set; }

        public override IEnumerable<TItem> Where(System.Linq.Expressions.Expression<Func<TItem, bool>> predicate) {
            return Sink.Where(predicate);
        }
    }
}