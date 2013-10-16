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
    public class LiveGraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : GraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        public LiveGraphPair (IGraph<TSinkItem, TSinkEdge> sink, IGraph<TSourceItem, TSourceEdge> source,
            GraphModelAdapter<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> adapter)
            : base(sink, source, adapter) { }


        public override TSinkItem Get (TSourceItem source) {
            bool contains = false;
            if (source is TSourceEdge) {
                contains = Source.Contains((TSourceEdge)source);
            } else {
                contains = Source.Contains(source);
            }
            if (contains)
                return Mapper.TryGetCreate(source);
            else
                return default(TSinkItem);
        }

        public override TSourceItem Get (TSinkItem sink) {
            return base.Get(sink);
        }

        public override ICollection<TSinkEdge> Edges (TSinkItem item) {
            //ICollection<TEdgeOne> result = Sink.Edges (item);
            if (true) {//(result == EmptyEgdes){
                TSourceItem itemTwo = Get(item);
                ICollection<TSourceEdge> _edgesTwo = null;
                if (itemTwo != null) {
                    _edgesTwo = Source.Edges(itemTwo);
                } else {
                    _edgesTwo = new EmptyCollection<TSourceEdge>();
                }
                foreach (TSourceEdge edgeTwo in _edgesTwo) {
                    Sink.Add((TSinkEdge)Mapper.TryGetCreate(edgeTwo));
                }
            }
            return Sink.Edges(item);
        }

        public override int EdgeCount (TSinkItem item) {
            TSourceItem itemTwo = Get(item);
            return Source.EdgeCount(itemTwo);
        }

        public override IEnumerator<TSinkItem> GetEnumerator () {
            foreach (TSourceItem itemTwo in Source) {
                TSinkItem itemOne = Mapper.TryGetCreate(itemTwo);
                yield return itemOne;
            }
        }
    }



    public class LiveEdgeCollection<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : ICollection<TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        private GraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> _graph;
        private ICollection<TSourceEdge> _sourceEdges;

        public LiveEdgeCollection (GraphPair<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> graph, TSinkItem item) {
            this._graph = graph;
            var sourceItem = _graph.Mapper.Get(item);
            if (sourceItem != null) {
                _sourceEdges = _graph.Source.Edges(sourceItem);
            } else {
                _sourceEdges = new EmptyCollection<TSourceEdge>();
            }
        }
        #region ICollection<TEdgeOne> Member

        public void Add (TSinkEdge item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear () {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains (TSinkEdge item) {
            var sourceEdge = (TSourceEdge)_graph.Mapper.Get(item);
            if (sourceEdge == null)
                return false;
            else
                return _graph.Source.Contains(sourceEdge);

        }

        public void CopyTo (TSinkEdge[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { return _graph.Source.Count; }
        }

        public bool IsReadOnly {
            get { return true; }
        }

        public bool Remove (TSinkEdge item) {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<TEdgeOne> Member

        public IEnumerator<TSinkEdge> GetEnumerator () {
            foreach (var sourceEdge in _sourceEdges) {
                var sinkEdge = (TSinkEdge)_graph.Mapper.TryGetCreate(sourceEdge);
                yield return sinkEdge;
            }
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
            return this.GetEnumerator();
        }

        #endregion
    }
}