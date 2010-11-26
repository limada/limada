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
using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {
    public abstract class GenericGraphFactory<TItem, TEdge> : IGraphFactory<TItem, TEdge>
    where TEdge : IEdge<TItem> {

        #region ITestGraphFactory<TItem,TEdge> Member

        IGraph<TItem, TEdge> _graph = null;
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

        public bool SeperateLattice {
            get { return _seperateLattice; }
            set { _seperateLattice = value; }
        }

        public bool AddDensity {
            get { return _addDensity; }
            set { _addDensity = value; }
        }

        protected bool _seperateLattice = false;
        protected bool _addDensity = false;

        public abstract void Populate ();

        public TItem Node1;
        public TItem Node2;
        public TItem Node3;
        public TItem Node4;
        public TItem Node5;
        public TItem Node6;
        public TItem Node7;
        public TItem Node8;
        public TItem Node9;
        public TEdge Link1;
        public TEdge Link2;
        public TEdge Link3;
        public TEdge Link4;
        public TEdge Link5;
        public TEdge Link6;
        public TEdge Link7;
        public TEdge Link8;
        #endregion
    }
}