/*
 * Limaki 
 * Version 0.07
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Common;

namespace Limaki.Graphs {
    public class GraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> : 
        GraphBase<TItemOne, TEdgeOne>,
        IGraphPair<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo>
        where TEdgeOne : IEdge<TItemOne>, TItemOne
        where TEdgeTwo : IEdge<TItemTwo>, TItemTwo {

        public GraphPair( IGraph<TItemOne, TEdgeOne> one, IGraph<TItemTwo, TEdgeTwo> two,
            GraphConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> converter )
            : base() {
            this.Converter = converter;
            this.One = one;
            this.Two = two;
            
        }

        #region GraphBase<TItemOne,TEdgeOne>-Member
        protected override void AddEdge( TEdgeOne edge, TItemOne item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge( TEdgeOne edge, TItemOne item ) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge( TEdgeOne edge, TItemOne oldItem, TItemOne newItem ) {
            TItemTwo two = Converter.Get(edge);
            One.ChangeEdge(edge, oldItem, newItem);
            if (two != null) {
                TEdgeTwo edgeTwo = (TEdgeTwo) two;
                TItemTwo oldTwo = Converter.Get(oldItem);
                TItemTwo newTwo = Converter.Convert(newItem);
                Two.ChangeEdge(edgeTwo, oldTwo, newTwo);
            }
        }

        public override bool Contains( TEdgeOne edge ) {
            return One.Contains(edge);
        }

        public override void Add( TEdgeOne edge ) {
            One.Add(edge);
            TEdgeTwo edgeTwo = (TEdgeTwo)Converter.Convert(edge);
            Two.Add(edgeTwo);
        }

        public override bool Remove( TEdgeOne edge ) {
            bool result = One.Remove(edge);
            TItemTwo edgeTwo = Converter.Get(edge);
            if (edgeTwo != null) {
                Two.Remove((TEdgeTwo)edgeTwo);
            }
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
            One.Add(item);
            TItemTwo itemTwo = Converter.Convert(item);
            Two.Add(itemTwo);
        }

        public override void Clear() {
            One.Clear();
            Two.Clear();
            Converter.Clear();
        }

        public override bool Contains( TItemOne item ) {
            return One.Contains(item);
        }

        public override void CopyTo( TItemOne[] array, int arrayIndex ) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int Count {
            get { return One.Count; }
        }

        public override bool IsReadOnly {
            get { return One.IsReadOnly; }
        }

        public override bool Remove( TItemOne item ) {
            bool result = One.Remove(item);
            TItemTwo itemTwo = Converter.Get(item);
            if (itemTwo != null) {
                Two.Remove(itemTwo);
            }
            return result;
        }

        public override IEnumerator<TItemOne> GetEnumerator() {
            return One.GetEnumerator();
        }

        public override void OnDataChanged( TItemOne item ) {
            base.OnDataChanged(item);
            One.OnDataChanged(item);
            TItemTwo itemTwo = Converter.Get(item);
            Two.OnDataChanged(itemTwo);
        }
        #endregion

        #region IGraphPair<TItemOne,TItemTwo,TEdgeOne,TEdgeTwo> Member

        IGraph<TItemOne, TEdgeOne> _one = null;
        public IGraph<TItemOne, TEdgeOne> One {
            get { return _one; }
            set {
                _one = value;
                Converter.One = value;
            }
        }
        IGraph<TItemTwo, TEdgeTwo> _two = null;
        public IGraph<TItemTwo, TEdgeTwo> Two {
            get { return _two; }
            set {
                _two = value;
                Converter.Two = value;
            }
        }


        public IDictionary<TItemOne, TItemTwo> One2Two {
            get { return Converter.One2Two; }
            set { Converter.One2Two = value; }
        }

        public IDictionary<TItemTwo, TItemOne> Two2One {
            get { return Converter.Two2One; }
            set { Converter.Two2One = value; }
        }

        GraphConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> _converter = null;
        public GraphConverter<TItemOne, TItemTwo, TEdgeOne, TEdgeTwo> Converter {
            get {return _converter;}
            set {_converter = value;}
        }

        #endregion


    }
}
