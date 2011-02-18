/*
 * Limaki 
 * Version 0.081
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
using Limaki.Common.Collections;

namespace Limaki.Graphs {
    /// <summary>
    /// a pair of two graphs of same type
    /// Data is a Graph which holds all items
    /// View is a a subgraph of Data
    /// every operation (add,remove etc.) is performed on both graphs
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class GraphView<TItem, TEdge> : GraphBase<TItem, TEdge>, IGraphPair<TItem, TItem, TEdge,TEdge>
        where TEdge : IEdge<TItem>, TItem {

        public GraphView(){}
        public GraphView(IGraph<TItem,TEdge> data,  IGraph<TItem,TEdge> view ) {
            this.One = view;
            this.Two = data;
        }
        
        IGraph<TItem, TEdge> _source = null;
        public virtual IGraph<TItem, TEdge> Two {
            get { return _source; }
            set { _source = value; }
        }

        IGraph<TItem, TEdge> _sub = null;
        public virtual IGraph<TItem, TEdge> One {
            get { return _sub; }
            set { _sub = value; }
        }


        protected override void AddEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override bool RemoveEdge(TEdge edge, TItem item) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void ChangeEdge(TEdge edge, TItem newItem, bool changeRoot) {
            TItem rootBefore = edge.Root;
            TItem leafBefore = edge.Leaf;

            One.ChangeEdge (edge, newItem, changeRoot);

            //edge.Root = rootBefore;
            //edge.Leaf = leafBefore;
            Two.ChangeEdge (edge, newItem, changeRoot);
        }

        public override void RevertEdge(TEdge edge) {
            TItem rootBefore = edge.Root;
            TItem leafBefore = edge.Leaf;
            One.RevertEdge(edge);

            edge.Root = rootBefore;
            edge.Leaf = leafBefore;
            Two.RevertEdge(edge);
        }

        public override bool Contains(TEdge edge) {
            return One.Contains(edge);
        }

        public override void Add(TEdge edge) {
            Two.Add(edge);
            One.Add(edge);
            //OnGraphChanged(edge, GraphChangeType.Add);
        }

        public override bool Remove(TEdge edge) {
            Two.Remove(edge);
            bool result = One.Remove(edge);
            //OnGraphChanged(edge, GraphChangeType.Remove);
            return result;
        }

        public override int EdgeCount(TItem item) {
            return One.EdgeCount(item);
        }

        public override ICollection<TEdge> Edges(TItem item) {
            return One.Edges(item);
        }

        public override IEnumerable<TEdge> Edges() {
            return One.Edges();
        }

        public override IEnumerable<KeyValuePair<TItem, ICollection<TEdge>>> ItemsWithEdges() {
            return One.ItemsWithEdges();
        }

        public override void Add(TItem item) {
            One.Add(item);
            Two.Add(item);
            //OnGraphChanged(item, GraphChangeType.Add);
        }

        public override void Clear() {
            One.Clear();
        }

        public override bool Contains(TItem item) {
            return One.Contains(item);
        }

        public override void CopyTo(TItem[] array, int arrayIndex) {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int Count {
            get { return One.Count; }
        }

        public override bool IsReadOnly {
            get { return One.IsReadOnly; }
        }

        public override bool Remove(TItem item) {
            Two.Remove(item);
            bool result = One.Remove(item);
            //OnGraphChanged (item, GraphChangeType.Remove);
            return result;
        }

        public override IEnumerator<TItem> GetEnumerator() {
            return One.GetEnumerator();
        }

        public override void OnDataChanged(TItem item) {
            // change the data graph first, then call view change-event
            Two.OnDataChanged (item);
            base.OnDataChanged(item);
        }

        public override void OnChangeData(TItem item, object data) {
            Two.OnChangeData(item, data);
            base.OnChangeData(item, data);
        }
        public override void OnGraphChanged( TItem item, GraphChangeType changeType ) {
            Two.OnGraphChanged(item, changeType);
            base.OnGraphChanged(item, changeType);
        }

        
        #region IGraphPair<TItem,TEdge,TItem,TEdge> Member

        IDictionary<TItem, TItem> IGraphPair<TItem, TItem, TEdge, TEdge>.One2Two {
            get { return new DictionaryAdapter<TItem> (this.Two); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        IDictionary<TItem, TItem> IGraphPair<TItem, TItem, TEdge, TEdge>.Two2One {
            get { return new DictionaryAdapter<TItem>(this.Two); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        GraphMapper<TItem, TItem, TEdge, TEdge> IGraphPair<TItem, TItem, TEdge, TEdge>.Mapper {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public virtual IEnumerable<TEdge> ComplementEdges(TItem item, IGraph<TItem, TEdge> graph) {
            throw new NotImplementedException("ComplementEdges not implemented in "+this.GetType().Name);
        }

        //TItem IGraphPair<TItem, TItem, TEdge, TEdge>.Get(TItem a) { return a; }
        //IGraph<TItem, TEdge> IGraphPair<TItem, TItem, TEdge, TEdge>.One {
        //    get { return One; }
        //    set { One = value; }
        //}

        //IGraph<TItem, TEdge> IGraphPair<TItem, TItem, TEdge, TEdge>.Two {
        //    get { return this.Two; }
        //    set { Two = value; }
        //}

        public virtual TItem Get(TItem a) { return a; }

        #endregion


        #region IFactoryListener<TItem> Member

        private Action<TItem> _createListener = null;
        Action<TItem> IFactoryListener<TItem>.ItemCreated {
            get { return _createListener; }
            set {_createListener = value;}
        }

        #endregion

        public override IEnumerable<TItem> Where(System.Linq.Expressions.Expression<Func<TItem, bool>> predicate) {
            return One.Where(predicate);
        }
    }
}