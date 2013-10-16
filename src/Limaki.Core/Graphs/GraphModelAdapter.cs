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

    public abstract class GraphModelAdapter<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
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

        public virtual GraphModelAdapter<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> ReverseAdapter() {
            return
                new ReverseGraphModelAdapter<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge>(this);
        }
    }

    public class ReverseGraphModelAdapter<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : 
        GraphModelAdapter<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        GraphModelAdapter<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> source = null;

        public ReverseGraphModelAdapter(GraphModelAdapter<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> source) {
            this.source = source;
        }

        public override TSinkItem CreateSinkItem(IGraph<TSourceItem, TSourceEdge> source,
            IGraph<TSinkItem, TSinkEdge> sink, TSourceItem item) {
            return this.source.CreateSourceItem(source,sink,item);
        }

        public override TSinkEdge CreateSinkEdge(IGraph<TSourceItem, TSourceEdge> source,
            IGraph<TSinkItem, TSinkEdge> sink, TSourceEdge item) {
            return this.source.CreateSourceEdge(source,sink,item);
        }

        public override TSourceItem CreateSourceItem(IGraph<TSinkItem, TSinkEdge> sink,
            IGraph<TSourceItem, TSourceEdge> source, TSinkItem item) {
            return this.source.CreateSinkItem(sink,source,item);
        }

        public override TSourceEdge CreateSourceEdge(IGraph<TSinkItem, TSinkEdge> sink,
            IGraph<TSourceItem, TSourceEdge> source, TSinkEdge item) {
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

        public override GraphModelAdapter<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> ReverseAdapter() {
            return source;
        }
    }
}