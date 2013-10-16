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

namespace Limaki.Graphs {
    /// <summary>
    /// Converts one graph into an other graph
    /// in both directions (bidirectional)
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    public class GraphMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>:
        IFactoryListener<TSinkItem>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public GraphMapper(GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> adapter) {
            this.Adapter = adapter;    
        }

        public GraphMapper() {}

        public GraphMapper( IGraph<TSinkItem, TSinkEdge> sink, IGraph<TSourceItem, TSourceEdge> source,
            GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> adapter)
            : this() {
            this.Adapter = adapter;
            this.Sink = sink;
            this.Source = source;
        }

        private GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> _adapter = null;
        public GraphItemTransformer<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> Adapter {
            get { return _adapter; }
            set {
                _adapter = value;
                CreateSinkItem = Adapter.CreateSinkItem;
                CreateSinkEdge = Adapter.CreateSinkEdge;
                CreateSourceItem = Adapter.CreateSourceItem;
                CreateSourceEdge = Adapter.CreateSourceEdge;
                EdgeCreatedSinkSource = Adapter.EdgeCreated;
                EdgeCreatedSourceSink = Adapter.EdgeCreated;
            }
        }

        protected GraphMapper(GraphSourceSinkMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> sinkSourceMapper,
            GraphSourceSinkMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> sourceSinkMapper)
            : this() {
            this._sinkSourceMapper = sinkSourceMapper;
            this._sourceSinkMapper = sourceSinkMapper;
        }

        GraphSourceSinkMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> _sinkSourceMapper = null;
        GraphSourceSinkMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> SinkSourceMapper {
            get {
                if (_sinkSourceMapper == null) {
                    _sinkSourceMapper = new GraphSourceSinkMapper<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>();
                    _sinkSourceMapper.CreateItem = this.CreateSourceItem;
                    _sinkSourceMapper.CreateEdge = this.CreateSourceEdge;
                    _sinkSourceMapper.EdgeCreated = this.EdgeCreatedSinkSource;
                    _sinkSourceMapper.RegisterPair = this.RegisterPair;
                }
                return _sinkSourceMapper;
            }
        }

        GraphSourceSinkMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> _sourceSinkMapper;
        GraphSourceSinkMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> SourceSinkMapper {
            get {
                if (_sourceSinkMapper == null) {
                    _sourceSinkMapper = new GraphSourceSinkMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge>();
                    _sourceSinkMapper.CreateItem = this.CreateSinkItem;
                    _sourceSinkMapper.CreateEdge = this.CreateSinkEdge;
                    _sourceSinkMapper.EdgeCreated = this.EdgeCreatedSourceSink;
                    _sourceSinkMapper.RegisterPair = this.RegisterPair;
                }
                return _sourceSinkMapper;
            }
        }

        public virtual IGraph<TSinkItem, TSinkEdge> Sink {
            get { return SinkSourceMapper.Source; }
            set {
                SinkSourceMapper.Source = value;
                SourceSinkMapper.Sink = value;
            }
        }

        public virtual IGraph<TSourceItem, TSourceEdge> Source {
            get { return SinkSourceMapper.Sink; }
            set {
                SinkSourceMapper.Sink = value;
                SourceSinkMapper.Source = value;
            }
        }

        public IDictionary<TSinkItem, TSourceItem> Sink2Source {
            get { return SinkSourceMapper.Dict; }
            set { SinkSourceMapper.Dict = value; }
        }

        public virtual void RegisterPair( TSinkItem sink, TSourceItem source ) {
            SinkSourceMapper.Dict[sink] = source;
            SourceSinkMapper.Dict[source] = sink;
        }

        public virtual void RegisterPair( TSourceItem b, TSinkItem a ) {
            RegisterPair(a, b);
        }

        #region SinkSource

        public IDictionary<TSourceItem,TSinkItem> Source2Sink {
            get { return SourceSinkMapper.Dict; }
            set { SourceSinkMapper.Dict = value; }
        }

        public virtual TSourceItem TryGetCreate( TSinkItem a ) {
            return SinkSourceMapper.TryGetCreate(a);
        }

        public virtual void ConvertSinkSource() {
            SinkSourceMapper.Convert();
        }

        public virtual TSourceItem Get( TSinkItem sink ) {
            var result = default(TSourceItem);
            if (sink == null)
                return result;
            SinkSourceMapper.Dict.TryGetValue(sink, out result);
            return result;

        }
        #endregion

        #region SourceSink

        public virtual TSinkItem TryGetCreate( TSourceItem b ) {
            return SourceSinkMapper.TryGetCreate(b);
        }

        public virtual TSinkItem Get( TSourceItem b ) {
            TSinkItem result = default(TSinkItem);
            if (b == null)
                return result;
            SourceSinkMapper.Dict.TryGetValue(b, out result);
            return result;

        }
        public virtual void ConvertSourceSink() {
            SourceSinkMapper.Convert();
        }

        public virtual void Clear() {
            SinkSourceMapper.Dict = null;
            SinkSourceMapper.Done = null;
            SourceSinkMapper.Dict = null;
            SourceSinkMapper.Done = null;
        }

        #endregion

        #region Factory-Methods

        public Func<IGraph<TSinkItem, TSinkEdge>, IGraph<TSourceItem, TSourceEdge>, TSinkItem, TSourceItem> 
            CreateSourceItem = null;
        public Func<IGraph<TSinkItem, TSinkEdge>, IGraph<TSourceItem, TSourceEdge>, TSinkEdge, TSourceEdge> 
            CreateSourceEdge = null;
        public Action<TSinkEdge, TSourceEdge> EdgeCreatedSinkSource=null;

        public Func<IGraph<TSourceItem, TSourceEdge>, IGraph<TSinkItem, TSinkEdge>, TSourceItem, TSinkItem> 
            CreateSinkItem = null;
        public Func<IGraph<TSourceItem, TSourceEdge>, IGraph<TSinkItem, TSinkEdge>, TSourceEdge, TSinkEdge> 
            CreateSinkEdge = null;
        public Action<TSourceEdge,TSinkEdge> EdgeCreatedSourceSink=null;

        #endregion

        #region IFactoryListener<TItemOne> Member
        public virtual Action<TSinkItem> ItemCreated {
            get { return SourceSinkMapper.ItemCreated; }
            set { SourceSinkMapper.ItemCreated = value; }
        }

        #endregion

        public GraphMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> ReverseMapper() {
            GraphMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge> result =
                new GraphMapper<TSourceItem, TSinkItem, TSourceEdge, TSinkEdge>(
                this.SourceSinkMapper, 
                this.SinkSourceMapper);

            result.Adapter = this.Adapter.ReverseAdapter ();
            result.Sink = this.Source;
            result.Source = this.Sink;

            return result;
        }
    }
}
