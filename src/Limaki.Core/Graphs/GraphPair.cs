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
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;

namespace Limaki.Graphs {
    /// <summary>
    /// GraphPair couples two graphs of different type
    /// the coupling is done by the GraphMapper
    /// </summary>
    /// <typeparam name="TItemOne"></typeparam>
    /// <typeparam name="TItemTwo"></typeparam>
    /// <typeparam name="TEdgeOne"></typeparam>
    /// <typeparam name="TEdgeTwo"></typeparam>
    public class GraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : 
        GraphBase<TItemOne, TEdgeOne>,
        IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        public GraphPair(IGraph<TItemOne, TEdgeOne> one, IGraph<TItemTwo, TEdgeTwo> two,
            GraphModelAdapter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> adapter)
            : base() {
            this.Mapper = new GraphMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>(adapter);
            this.One = one;
            this.Two = two;
            this.ChangeData = adapter.ChangeData;
        }

        public virtual TItemTwo Get(TItemOne a) {
            return Mapper.Get (a);
        }

        public virtual TItemOne Get(TItemTwo a) {
            return Mapper.Get(a);
        }

        #region GraphBase<TItemOne,TEdgeOne>-Member

        protected override void AddEdge( TEdgeOne edge, TItemOne item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge( TEdgeOne edge, TItemOne item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge(TEdgeOne edge, TItemOne newItem, bool changeRoot) {
            TItemTwo two = Get(edge);
            One.ChangeEdge(edge, newItem, changeRoot);
            if (two != null) {
                TEdgeTwo edgeTwo = (TEdgeTwo) two;
                //TItemTwo oldTwo = Mapper.Get(oldItem);
                TItemTwo newTwo = Mapper.TryGetCreate(newItem);
                Two.ChangeEdge(edgeTwo, newTwo, changeRoot);
            }
        }

        public override void RevertEdge(TEdgeOne edge) {
            TItemTwo two = Get(edge);
            One.RevertEdge (edge);
            if (two != null) {
                TEdgeTwo edgeTwo = (TEdgeTwo) two;
                Two.RevertEdge (edgeTwo);
            }
        }

        public override bool Contains( TEdgeOne edge ) {
            return One.Contains(edge);
        }

        public override void Add( TEdgeOne edge ) {
            One.Add(edge);
            TEdgeTwo edgeTwo = (TEdgeTwo)Mapper.TryGetCreate(edge);
            Two.Add(edgeTwo);
        }

        /// <summary>
        /// removes edges out of Converter
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        protected virtual bool RemoveEdge(IEnumerable<TEdgeOne> edges) {
            foreach (TEdgeOne edge in edges) {
               Mapper.One2Two.Remove(edge);
            }
            return true;
        }

        protected virtual bool RemoveEdge(IEnumerable<TEdgeTwo> edges) {
            foreach (TEdgeTwo edge in edges) {
                Mapper.Two2One.Remove(edge);
            }
            return true;
        }

        public override bool Remove( TEdgeOne edge ) {
            return Remove ((TItemOne) edge);
        }

        public override bool Remove(TItemOne item) {
            if (item == null) return false;
            RemoveEdge (One.DepthFirstTwig (item));
            bool result = One.Remove(item);
            TItemTwo itemTwo = Get(item);
            if (itemTwo != null) {
                RemoveEdge(Two.DepthFirstTwig(itemTwo));
                Two.Remove(itemTwo);
                Mapper.Two2One.Remove(itemTwo);
            }
            Mapper.One2Two.Remove(item);

            return result;
        }

        public override int EdgeCount( TItemOne item ) {
            return One.EdgeCount(item);
        }


        public override ICollection<TEdgeOne> Edges( TItemOne item ) {
            return One.Edges(item);
        }

        public override IEnumerable<TEdgeOne> Edges() {
            return One.Edges();
        }

        public override IEnumerable<KeyValuePair<TItemOne, ICollection<TEdgeOne>>> ItemsWithEdges() {
            return One.ItemsWithEdges();
        }

        public override void Add( TItemOne item ) {
            if (item == null) return;
            One.Add(item);
            TItemTwo itemTwo = Mapper.TryGetCreate(item);
            Two.Add(itemTwo);
        }

        public override void Clear() {
            One.Clear();
            Two.Clear();
            Mapper.Clear();
        }

        public override bool Contains( TItemOne item ) {
            return One.Contains(item);
        }

        public override void CopyTo( TItemOne[] array, int arrayIndex ) {
            //throw new Exception("The method or operation is not implemented.");
            One.CopyTo (array, arrayIndex);
        }

        public override int Count {
            get { return One.Count; }
        }

        public override bool IsReadOnly {
            get { return One.IsReadOnly; }
        }



        public override IEnumerator<TItemOne> GetEnumerator() {
            return One.GetEnumerator();
        }

        public override void OnDataChanged( TItemOne item ) {
            base.OnDataChanged(item);
            One.OnDataChanged(item);
            TItemTwo itemTwo = Get(item);
            Two.OnDataChanged(itemTwo);
        }

        public override void OnChangeData(TItemOne item, object data) {
            TItemTwo itemTwo = Get(item);
            Two.OnChangeData (itemTwo, data);
            One.OnChangeData (item, data);
            base.OnChangeData(item, data);
        }

        public override void OnGraphChanged(TItemOne item, GraphChangeType changeType) {
            base.OnGraphChanged(item, changeType);
            One.OnGraphChanged(item, changeType);
            TItemTwo itemTwo = Get(item);
            Two.OnGraphChanged(itemTwo, changeType);
        }

        #endregion

        #region IGraphPair<TItemOne,TItemTwo,TEdgeOne,TEdgeTwo> Member

        IGraph<TItemOne, TEdgeOne> _one = null;
        public virtual IGraph<TItemOne, TEdgeOne> One {
            get { return _one; }
            set {
                _one = value;
                Mapper.One = value;
            }
        }
        IGraph<TItemTwo, TEdgeTwo> _two = null;
        public virtual IGraph<TItemTwo, TEdgeTwo> Two {
            get { return _two; }
            set {
                _two = value;
                Mapper.Two = value;
            }
        }


        public virtual IDictionary<TItemOne, TItemTwo> One2Two {
            get { return Mapper.One2Two; }
            set { Mapper.One2Two = value; }
        }

        public virtual IDictionary<TItemTwo, TItemOne> Two2One {
            get { return Mapper.Two2One; }
            set { Mapper.Two2One = value; }
        }

        GraphMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> _mapper = null;
        public virtual GraphMapper<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> Mapper {
            get {return _mapper;}
            set {_mapper = value;}
        }


        #endregion

        #region IFactoryListener<TItemOne> Member

        Action<TItemOne> IFactoryListener<TItemOne>.ItemCreated {
            get { return Mapper.ItemCreated; }
            set { Mapper.ItemCreated = value; }
        }

        #endregion

        #region special algos

        public virtual IEnumerable<TEdgeOne> ComplementEdges(TItemOne item, IGraph<TItemOne,TEdgeOne> graph) {
            TItemTwo itemTwo = Get (item);

            foreach (TEdgeTwo edge in Two.Fork(itemTwo)) {
                var rootOne = Mapper.Get(edge.Root);
                bool doyield = rootOne != null && graph.Contains(rootOne);
                if (doyield) {
                    var leafOne = Mapper.Get(edge.Leaf);
                    doyield = leafOne != null && graph.Contains(leafOne);
                    if (doyield) {
                        var result = Get(edge);
                        if (result is TEdgeOne)
                            yield return (TEdgeOne)result;
                    }
                }
            }
        }

        #endregion

        public override IEnumerable<TItemOne> Where(System.Linq.Expressions.Expression<Func<TItemOne, bool>> predicate) {
            return One.Where(predicate);
        }
    }
}
