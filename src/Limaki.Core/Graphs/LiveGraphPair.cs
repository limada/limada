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

namespace Limaki.Graphs {
    public class LiveGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : GraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        public LiveGraphPair(IGraph<TItemOne, TEdgeOne> one, IGraph<TItemTwo, TEdgeTwo> two,
            GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> adapter)
            : base(one, two, adapter) { }


        public override TItemOne Get(TItemTwo a) {
            bool contains = false;
            if (a is TEdgeTwo) {
                contains = Two.Contains ((TEdgeTwo) a);
            } else {
                contains = Two.Contains(a);
            }
            if (contains)
                return Mapper.TryGetCreate(a);
            else
                return default(TItemOne);
        }

        public override TItemTwo Get(TItemOne a) {
            return base.Get(a);
        }

        public override ICollection<TEdgeOne> Edges(TItemOne item) {
            //ICollection<TEdgeOne> result = One.Edges (item);
            if (true){//(result == EmptyEgdes){
                TItemTwo itemTwo = Get(item);
                ICollection<TEdgeTwo> _edgesTwo = null;
                if (itemTwo != null) {
                    _edgesTwo = Two.Edges(itemTwo);
                } else {
                    _edgesTwo = new EmptyCollection<TEdgeTwo>();
                }
                foreach(TEdgeTwo edgeTwo in _edgesTwo) {
                    One.Add((TEdgeOne)Mapper.TryGetCreate(edgeTwo));
                }
            }
            return One.Edges(item);
        }

        public override int EdgeCount(TItemOne item) {
            TItemTwo itemTwo = Get (item);
            return Two.EdgeCount(itemTwo);
        }

        public override IEnumerator<TItemOne> GetEnumerator() {
            foreach (TItemTwo itemTwo in Two) {
                TItemOne itemOne = Mapper.TryGetCreate(itemTwo);
                yield return itemOne;
            }
        }
    }



    public class LiveEdgeCollection<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : ICollection<TEdgeOne>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {
        
        private GraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> _graph;
        private ICollection<TEdgeTwo> _edgesTwo;
        
        public LiveEdgeCollection(GraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> graph, TItemOne item) {
            this._graph = graph;
            TItemTwo itemTwo = _graph.Mapper.Get (item);
            if (itemTwo != null) {
                _edgesTwo = _graph.Two.Edges (itemTwo);
            } else {
                _edgesTwo = new EmptyCollection<TEdgeTwo> ();
            }
        }
        #region ICollection<TEdgeOne> Member

        public void Add(TEdgeOne item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear() {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Contains(TEdgeOne item) {
            var edgeTwo = (TEdgeTwo)_graph.Mapper.Get(item);
            if (edgeTwo == null)
                return false;
            else
                return _graph.Two.Contains(edgeTwo);

        }

        public void CopyTo(TEdgeOne[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Count {
            get { return _graph.Two.Count; }
        }

        public bool IsReadOnly {
            get { return true;  }
        }

        public bool Remove(TEdgeOne item) {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<TEdgeOne> Member

        public IEnumerator<TEdgeOne> GetEnumerator() {
            foreach(TEdgeTwo edgeTwo in _edgesTwo) {
                TEdgeOne edgeOne = (TEdgeOne)_graph.Mapper.TryGetCreate(edgeTwo);
                yield return edgeOne;
            }
        }

        #endregion

        #region IEnumerable Member

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return this.GetEnumerator ();
        }

        #endregion
        }
}