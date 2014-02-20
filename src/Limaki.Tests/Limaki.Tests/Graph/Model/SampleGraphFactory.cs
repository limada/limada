/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Collections.Generic;
using Limaki.Common;
using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {

    public class SampleGraphFactory<TItem, TEdge> : SampleGraphFactoryBase<TItem, TEdge> 
       where TEdge : IEdge<TItem> , TItem {

        #region ItemFactory

        public IGraphModelFactory<TItem, TEdge> Creator {
            get { return Registry.Factory.Create<IGraphModelFactory<TItem, TEdge>> (); }
        }

        public virtual TEdge CreateEdge (TItem root, TItem leaf) {
            return Creator.CreateEdge (root, leaf);
        }

        public virtual TEdge CreateEdge () {
            return Creator.CreateEdge ();
        }

        public virtual TItem CreateItem<T> (T data) {
            return Creator.CreateItem<T> (data);
        }

        public virtual TItem CreateItem<T> () {
            return Creator.CreateItem<T> (default (T));
        }

        #endregion

        private int _start = 0;
        public int Start {
            get { return _start; }
            set { _start = value; }
        }

        public override string Name {
            get { return this.GetType ().Name; }
        }

        public override IGraph<TItem, TEdge> Graph {
            get { return base.Graph ?? (base.Graph = Creator.Graph ()); }
            set { base.Graph = value; }
        }

        public override void Populate() {
            Populate (this.Graph);
        }

        public virtual void MakeEdgeStrings (IGraph<TItem, TEdge> Graph) {
            var changer = Creator as IGraphModelPropertyChanger<TItem, TEdge>;
            if (changer != null)
                foreach (var edge in Graph.Edges ()) {
                    changer.SetProperty (edge, GraphExtensions.EdgeString<TItem, TEdge> (edge));
                }
        }

        public override void Populate(IGraph<TItem, TEdge> Graph) {

            var lastNode1 = default (TItem);
            var lastNode2 = default (TItem); ;
            var lastNode3 = default (TItem); ;
            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    lastNode1 = Nodes[1];
                    lastNode2 = Nodes[5];
                    lastNode3 = Nodes[8];
                }
                Populate(Graph,Start+1);
                if (i > 0) {
                    var edge = CreateEdge (lastNode1, Nodes[1]);
                    Graph.Add(edge);
                    if (SeperateLattice) {
                        edge = CreateEdge (lastNode2, Nodes[5]);
                        Graph.Add(edge);
                    }
                    if (AddDensity) {
                        edge = CreateEdge (Nodes[2], lastNode3);
                        Graph.Add(edge);
                    }
                }
            }

        }

        public virtual void Populate(IGraph<TItem, TEdge> Graph, int start) {

            var item = CreateItem<int>((start++));
            Graph.Add(item);
            Nodes[1] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[2] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[3] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[4] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[5] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[6] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[7] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[8] = item;

            item = CreateItem<int> ((start++));
            Graph.Add(item);
            Nodes[9] = item;
            this.Start = start;

        }
    }
}