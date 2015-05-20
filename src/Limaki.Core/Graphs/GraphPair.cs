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


using Limaki.Common.Collections;
using System;
using System.Collections.Generic;

namespace Limaki.Graphs {

    /// <summary>
    /// GraphPair couples two graphs of different type
    /// the coupling is done by the GraphMapper
    /// the rules for the mapping are in the GraphItemTransformer
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    public class GraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : 
        GraphBase<TSinkItem, TSinkEdge>,
        IGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public GraphPair(IGraph<TSinkItem, TSinkEdge> sink, IGraph<TSourceItem, TSourceEdge> source,
            GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> transformer)
            : base() {
            this.Mapper = new GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>(transformer);
            this.Sink = sink;
            this.Source = source;
            this.ChangeData = transformer.ChangeData;
        }

        public virtual TSourceItem Get(TSinkItem a) {
            return Mapper.Get (a);
        }

        public virtual void UpdateSink (TSinkItem sinkItem) {
            Mapper.UpdateSink (sinkItem);
        }

        public virtual TSinkItem Get (TSourceItem source) {
            bool contains = false;
            if (source is TSourceEdge) {
                contains = Source.Contains ((TSourceEdge)source);
            } else {
                contains = Source.Contains (source);
            }
            if (contains)
                return Mapper.TryGetCreate (source);
            else
                return default (TSinkItem);
        }

        #region GraphBase<TSinkItem,TSinkEdge>-Member

        protected override void AddEdge( TSinkEdge edge, TSinkItem item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge( TSinkEdge edge, TSinkItem item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge(TSinkEdge sinkEdge, TSinkItem newItem, bool changeRoot) {
            var source = Get(sinkEdge);
            Sink.ChangeEdge(sinkEdge, newItem, changeRoot);
            if (source != null) {
                var sourceEdge = (TSourceEdge) source;
                var newSourceEdge = Mapper.TryGetCreate(newItem);
                Source.ChangeEdge(sourceEdge, newSourceEdge, changeRoot);
            }
        }

        public override void RevertEdge(TSinkEdge edge) {
            var two = Get(edge);
            Sink.RevertEdge (edge);
            if (two != null) {
                var edgeTwo = (TSourceEdge) two;
                Source.RevertEdge (edgeTwo);
            }
        }

        public override bool Contains( TSinkEdge edge ) {
            return Sink.Contains(edge);
        }

        public override void Add( TSinkEdge edge ) {
            Sink.Add(edge);
            var edgeTwo = (TSourceEdge)Mapper.TryGetCreate(edge);
            Source.Add(edgeTwo);
        }

        /// <summary>
        /// removes edges out of Converter
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        protected virtual bool RemoveEdge(IEnumerable<TSinkEdge> edges) {
            foreach (var edge in edges) {
               Mapper.Sink2Source.Remove(edge);
            }
            return true;
        }

        protected virtual bool RemoveEdge(IEnumerable<TSourceEdge> edges) {
            foreach (var edge in edges) {
                Mapper.Source2Sink.Remove(edge);
            }
            return true;
        }

        public override bool Remove( TSinkEdge edge ) {
            return Remove ((TSinkItem) edge);
        }

        protected virtual bool Remove(TSinkItem item, bool inSource) {
            if (item == null) return false;
            RemoveEdge (Sink.DepthFirstTwig (item));
            var result = false;
            var sinkGraph = Sink as ISinkGraph<TSinkItem, TSinkEdge>;
            if (inSource || sinkGraph == null)
                result = Sink.Remove (item);
            else
                result = sinkGraph.RemoveSinkItem (item);
            var sourceItem = Get(item);
            if (sourceItem != null) {
                RemoveEdge(Source.DepthFirstTwig(sourceItem));
                if (inSource) {
                    Source.Remove (sourceItem);
                }
                Mapper.Source2Sink.Remove(sourceItem);
            }

            Mapper.Sink2Source.Remove(item);

            return result;
        }

        public override bool Remove (TSinkItem item) {
            return Remove (item, true);
        }

        public virtual bool RemoveSinkItem (TSinkItem item) {
            var result = Remove (item, false);
            var sinkGraph = Source as ISinkGraph<TSinkItem, TSinkEdge>;
            if (sinkGraph != null)
                sinkGraph.RemoveSinkItem (item);
            return result;
        }

        public override int EdgeCount (TSinkItem item) {
            var itemTwo = Get (item);
            return Source.EdgeCount (itemTwo);
        }

        public override IEnumerable<TSinkEdge> Edges() {
            return Sink.Edges();
        }

        public override ICollection<TSinkEdge> Edges (TSinkItem item) {
            //ICollection<TEdgeOne> result = Sink.Edges (item);
            if (true) {//(result == EmptyEgdes){
                var itemTwo = Get (item);
                ICollection<TSourceEdge> _edgesTwo = null;
                if (itemTwo != null) {
                    _edgesTwo = Source.Edges (itemTwo);
                } else {
                    _edgesTwo = new EmptyCollection<TSourceEdge> ();
                }
                foreach (var edgeTwo in _edgesTwo) {
                    Sink.Add ((TSinkEdge)Mapper.TryGetCreate (edgeTwo));
                }
            }
            return Sink.Edges (item);
        }

        public override IEnumerable<KeyValuePair<TSinkItem, ICollection<TSinkEdge>>> ItemsWithEdges() {
            return Sink.ItemsWithEdges();
        }

        public override void Add( TSinkItem item ) {
            if (item == null) return;
            Sink.Add(item);
            var itemTwo = Mapper.TryGetCreate(item);
            Source.Add(itemTwo);
        }

        public override void Clear() {
            Sink.Clear();
            Source.Clear();
            Mapper.Clear();
        }

        public override bool Contains( TSinkItem item ) {
            return Sink.Contains(item);
        }

        public override void CopyTo( TSinkItem[] array, int arrayIndex ) {
            //throw new Exception("The method or operation is not implemented.");
            Sink.CopyTo (array, arrayIndex);
        }

        public override int Count {
            get { return Sink.Count; }
        }

        public override bool IsReadOnly {
            get { return Sink.IsReadOnly; }
        }

        public override IEnumerator<TSinkItem> GetEnumerator () {
            foreach (var itemTwo in Source) {
                var itemOne = Mapper.TryGetCreate (itemTwo);
                yield return itemOne;
            }
        }

        public override void OnDataChanged( TSinkItem item ) {
            base.OnDataChanged(item);
            Sink.OnDataChanged(item);
            var sourceItem = Get(item);
            Source.OnDataChanged(sourceItem);
        }

        public override void DoChangeData(TSinkItem item, object data) {
            var sourceItem = Get(item);
            Source.DoChangeData (sourceItem, data);
            Sink.DoChangeData (item, data);
            base.DoChangeData(item, data);
        }

        public override void OnGraphChange(TSinkItem item, GraphEventType eventType) {
            base.OnGraphChange(item, eventType);
            Sink.OnGraphChange(item, eventType);
            var sourceItem = Get (item);
            Source.OnGraphChange (sourceItem, eventType);
        }

        #endregion

        #region IGraphPair<TSinkItem,TSourceItem,TSinkEdge,TSourceEdge> Member

        IGraph<TSinkItem, TSinkEdge> _sink = null;
        public virtual IGraph<TSinkItem, TSinkEdge> Sink {
            get { return _sink; }
            set {
                _sink = value;
                Mapper.Sink = value;
            }
        }
        IGraph<TSourceItem, TSourceEdge> _source = null;
        public virtual IGraph<TSourceItem, TSourceEdge> Source {
            get { return _source; }
            set {
                _source = value;
                Mapper.Source = value;
            }
        }


        public virtual IDictionary<TSinkItem, TSourceItem> Sink2Source {
            get { return Mapper.Sink2Source; }
            set { Mapper.Sink2Source = value; }
        }

        public virtual IDictionary<TSourceItem, TSinkItem> Source2Sink {
            get { return Mapper.Source2Sink; }
            set { Mapper.Source2Sink = value; }
        }


        public virtual GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Mapper { get; set; }

        #endregion

        #region IFactoryListener<TSinkItem> Member

        Action<TSinkItem> IFactoryListener<TSinkItem>.ItemCreated {
            get { return Mapper.ItemCreated; }
            set { Mapper.ItemCreated = value; }
        }

        #endregion

        #region special algos

        /// <summary>
        /// gives back all (fork)egdes of item 
        /// contained in Source (the complement graph)
        /// </summary>
        /// <param name="item"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        public virtual IEnumerable<TSinkEdge> ComplementEdges(TSinkItem item, IGraph<TSinkItem,TSinkEdge> graph) {
            var itemTwo = Get (item);

            foreach (var edge in Source.Fork(itemTwo)) {
                var rootOne = Mapper.Get(edge.Root);
                bool doyield = rootOne != null && graph.Contains(rootOne);
                if (doyield) {
                    var leafOne = Mapper.Get(edge.Leaf);
                    doyield = leafOne != null && graph.Contains(leafOne);
                    if (doyield) {
                        var result = Get(edge);
                        if (result is TSinkEdge)
                            yield return (TSinkEdge)result;
                    }
                }
            }
        }

        #endregion

        public override IEnumerable<TSinkItem> WhereQ(System.Linq.Expressions.Expression<Func<TSinkItem, bool>> predicate) {
            return Sink.WhereQ(predicate);
        }
    }
}
