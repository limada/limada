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

namespace Limaki.Graphs {
    /// <summary>
    /// GraphPair couples two graphs of different type
    /// the coupling is done by the GraphMapper
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
            GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Transformer)
            : base() {
            this.Mapper = new GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>(Transformer);
            this.Sink = sink;
            this.Source = source;
            this.ChangeData = Transformer.ChangeData;
        }

        public virtual TSourceItem Get(TSinkItem a) {
            return Mapper.Get (a);
        }

        public virtual TSinkItem Get(TSourceItem a) {
            return Mapper.Get(a);
        }

        #region GraphBase<TSinkItem,TSinkEdge>-Member

        protected override void AddEdge( TSinkEdge edge, TSinkItem item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge( TSinkEdge edge, TSinkItem item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge(TSinkEdge edge, TSinkItem newItem, bool changeRoot) {
            TSourceItem two = Get(edge);
            Sink.ChangeEdge(edge, newItem, changeRoot);
            if (two != null) {
                TSourceEdge edgeTwo = (TSourceEdge) two;
                //TItemTwo oldTwo = Mapper.Get(oldItem);
                TSourceItem newTwo = Mapper.TryGetCreate(newItem);
                Source.ChangeEdge(edgeTwo, newTwo, changeRoot);
            }
        }

        public override void RevertEdge(TSinkEdge edge) {
            TSourceItem two = Get(edge);
            Sink.RevertEdge (edge);
            if (two != null) {
                TSourceEdge edgeTwo = (TSourceEdge) two;
                Source.RevertEdge (edgeTwo);
            }
        }

        public override bool Contains( TSinkEdge edge ) {
            return Sink.Contains(edge);
        }

        public override void Add( TSinkEdge edge ) {
            Sink.Add(edge);
            TSourceEdge edgeTwo = (TSourceEdge)Mapper.TryGetCreate(edge);
            Source.Add(edgeTwo);
        }

        /// <summary>
        /// removes edges out of Converter
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        protected virtual bool RemoveEdge(IEnumerable<TSinkEdge> edges) {
            foreach (TSinkEdge edge in edges) {
               Mapper.Sink2Source.Remove(edge);
            }
            return true;
        }

        protected virtual bool RemoveEdge(IEnumerable<TSourceEdge> edges) {
            foreach (TSourceEdge edge in edges) {
                Mapper.Source2Sink.Remove(edge);
            }
            return true;
        }

        public override bool Remove( TSinkEdge edge ) {
            return Remove ((TSinkItem) edge);
        }

        public override bool Remove(TSinkItem item) {
            if (item == null) return false;
            RemoveEdge (Sink.DepthFirstTwig (item));
            bool result = Sink.Remove(item);
            var sourceItem = Get(item);
            if (sourceItem != null) {
                RemoveEdge(Source.DepthFirstTwig(sourceItem));
                Source.Remove(sourceItem);
                Mapper.Source2Sink.Remove(sourceItem);
            }
            Mapper.Sink2Source.Remove(item);

            return result;
        }

        public override int EdgeCount( TSinkItem item ) {
            return Sink.EdgeCount(item);
        }


        public override ICollection<TSinkEdge> Edges( TSinkItem item ) {
            return Sink.Edges(item);
        }

        public override IEnumerable<TSinkEdge> Edges() {
            return Sink.Edges();
        }

        public override IEnumerable<KeyValuePair<TSinkItem, ICollection<TSinkEdge>>> ItemsWithEdges() {
            return Sink.ItemsWithEdges();
        }

        public override void Add( TSinkItem item ) {
            if (item == null) return;
            Sink.Add(item);
            TSourceItem itemTwo = Mapper.TryGetCreate(item);
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



        public override IEnumerator<TSinkItem> GetEnumerator() {
            return Sink.GetEnumerator();
        }

        public override void OnDataChanged( TSinkItem item ) {
            base.OnDataChanged(item);
            Sink.OnDataChanged(item);
            var sourceItem = Get(item);
            Source.OnDataChanged(sourceItem);
        }

        public override void OnChangeData(TSinkItem item, object data) {
            var sourceItem = Get(item);
            Source.OnChangeData (sourceItem, data);
            Sink.OnChangeData (item, data);
            base.OnChangeData(item, data);
        }

        public override void OnGraphChanged(TSinkItem item, GraphChangeType changeType) {
            base.OnGraphChanged(item, changeType);
            Sink.OnGraphChanged(item, changeType);
            var sourceItem = Get(item);
            Source.OnGraphChanged(sourceItem, changeType);
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

        #region IFactoryListener<TItemOne> Member

        Action<TSinkItem> IFactoryListener<TSinkItem>.ItemCreated {
            get { return Mapper.ItemCreated; }
            set { Mapper.ItemCreated = value; }
        }

        #endregion

        #region special algos

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

        public override IEnumerable<TSinkItem> Where(System.Linq.Expressions.Expression<Func<TSinkItem, bool>> predicate) {
            return Sink.Where(predicate);
        }
    }
}
