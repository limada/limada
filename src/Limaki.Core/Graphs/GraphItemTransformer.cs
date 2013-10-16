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


namespace Limaki.Graphs {

    /// <summary>
    /// transforms TSource into TSink and vice versa (bidirectional) of IGraphPair
    /// used to implement the concrete transform view of items in GraphPair
    /// its mainly composed of some factory methods to create items and
    /// some observer methods to listen on changes of items
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    public abstract class GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public abstract TSinkItem CreateSinkItem(IGraph<TSourceItem, TSourceEdge> source,
            IGraph<TSinkItem, TSinkEdge> sink, TSourceItem item);
        public abstract TSinkEdge CreateSinkEdge(IGraph<TSourceItem, TSourceEdge> source,
            IGraph<TSinkItem, TSinkEdge> sink, TSourceEdge item);

        public abstract TSourceItem CreateSourceItem(IGraph<TSinkItem, TSinkEdge> sink, 
            IGraph<TSourceItem, TSourceEdge> source, TSinkItem item);
        public abstract TSourceEdge CreateSourceEdge(IGraph<TSinkItem, TSinkEdge> sink, 
            IGraph<TSourceItem, TSourceEdge> source, TSinkEdge item);

        public abstract void ChangeData ( IGraph<TSinkItem, TSinkEdge> sink, TSinkItem item, object data );
        public abstract void ChangeData ( IGraph<TSourceItem, TSourceEdge> source, TSourceItem item, object data);
        public virtual void EdgeCreated ( TSinkEdge sinkEdge, TSourceEdge sourceEdge) {}
        public virtual void EdgeCreated ( TSourceEdge sourceEdge, TSinkEdge sinkEdge) {}

        public virtual GraphItemTransformer<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> ReverseAdapter() {
            return new ReverseGraphItemTransformer<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge>(this);
        }
    }

    public class ReverseGraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : 
        GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        GraphItemTransformer<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> source = null;

        public ReverseGraphItemTransformer(GraphItemTransformer<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> source) {
            this.source = source;
        }

        public override TSinkItem CreateSinkItem(IGraph<TSourceItem, TSourceEdge> source, IGraph<TSinkItem, TSinkEdge> sink, TSourceItem item) {
            return this.source.CreateSourceItem(source,sink,item);
        }

        public override TSinkEdge CreateSinkEdge(IGraph<TSourceItem, TSourceEdge> source, IGraph<TSinkItem, TSinkEdge> sink, TSourceEdge item) {
            return this.source.CreateSourceEdge(source,sink,item);
        }

        public override TSourceItem CreateSourceItem(IGraph<TSinkItem, TSinkEdge> sink, IGraph<TSourceItem, TSourceEdge> source, TSinkItem item) {
            return this.source.CreateSinkItem(sink,source,item);
        }

        public override TSourceEdge CreateSourceEdge(IGraph<TSinkItem, TSinkEdge> sink, IGraph<TSourceItem, TSourceEdge> source, TSinkEdge item) {
            return this.source.CreateSinkEdge(sink,source,item);
        }
        public override void EdgeCreated(TSinkEdge sinkEdge, TSourceEdge sourceEdge) {
            source.EdgeCreated (sinkEdge, sourceEdge);
        }
        public override void EdgeCreated(TSourceEdge sourceEdge, TSinkEdge sinkEdge) {
            source.EdgeCreated (sourceEdge, sinkEdge);
        }
        public override void ChangeData(IGraph<TSinkItem, TSinkEdge> sink, TSinkItem item, object data) {
            source.ChangeData (sink, item, data);
        }
        public override void ChangeData(IGraph<TSourceItem, TSourceEdge> source, TSourceItem item, object data) {
            source.ChangeData(source, item, data);
        }

        public override GraphItemTransformer<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> ReverseAdapter() {
            return source;
        }
    }
}