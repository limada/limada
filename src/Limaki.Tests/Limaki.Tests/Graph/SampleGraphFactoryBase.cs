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
using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {

    /// <summary>
    /// a <see cref="ISampleGraphFactory{TItem,TEdge}"/> producing 
    /// a <see cref="IGraph{TItemOne, TEdgeOne}"/>
    /// </summary>
    public abstract class SampleGraphFactoryBase<TItem, TEdge> : ISampleGraphFactory<TItem, TEdge>
        where TEdge : IEdge<TItem> {

        #region IGraphFactory<TItem,TEdge> Member

        protected IGraph<TItem, TEdge> _graph = null;
        public virtual IGraph<TItem, TEdge> Graph {
            get { return _graph; }
            set { _graph = value; }
        }

        private int _count = 1;
        public int Count {
            get { return _count; }
            set { _count = value; }
        }

        public abstract string Name { get; }

        protected bool _seperateLattice = false;
        public bool SeperateLattice {
            get { return _seperateLattice; }
            set { _seperateLattice = value; }
        }

        protected bool _addDensity = false;
        public bool AddDensity {
            get { return _addDensity; }
            set { _addDensity = value; }
        }

        public virtual void Populate () {
            Populate (this.Graph);
        }

        public abstract void Populate(IGraph<TItem, TEdge> graph);

        IList<TItem> _node = null;
        public virtual IList<TItem> Nodes {
            get {
                if (_node == null) {
                    _node = new TItem[11];
                }
                return _node;
            }
        }


        IList<TEdge> _link = null;
        public virtual IList<TEdge> Edges {
            get {
                if (_link == null) {
                    _link = new TEdge[11];
                }
                return _link;
            }
        }

        public virtual void AddSamplesToGraph(IGraph<TItem,TEdge> graph) {
            foreach(TItem item in Nodes) {
                if (item != null) {
                    graph.Add (item);
                }
            }
            foreach (TEdge item in Edges) {
                if (item != null) {
                    graph.Add(item);
                }
            }
        }

        #endregion
    }
}