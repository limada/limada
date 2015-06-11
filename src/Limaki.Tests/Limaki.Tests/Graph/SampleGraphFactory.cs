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
       where TEdge : IEdge<TItem>, TItem {

        #region ItemFactory

        IGraphModelFactory<TItem, TEdge> _creator = null;
        public IGraphModelFactory<TItem, TEdge> Factory {
            get { return _creator ?? (_creator = Registry.Factory.Create<IGraphModelFactory<TItem, TEdge>> ()); }
        }

        public virtual TEdge CreateEdge (TItem root, TItem leaf) {
            return Factory.CreateEdge (root, leaf);
        }

        public virtual TEdge CreateEdge () {
            return Factory.CreateEdge ();
        }

        public virtual TItem CreateItem<T> (T data) {
            return Factory.CreateItem<T> (data);
        }

        public virtual TItem CreateItem<T> () {
            return Factory.CreateItem<T> (default (T));
        }

        public virtual TItem SetNode<T> (int i, T data) {
            var node = CreateItem<T> (data);
            Graph.Add (node);
            Nodes[i] = node;
            return node;
        }

        public virtual TEdge SetEdge (int i, TItem root, TItem leaf) {
            var edge = CreateEdge (root, leaf);
            Graph.Add (edge);
            Edges[i] = edge;
            return edge;
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
            get { return base.Graph ?? (base.Graph = Factory.Graph ()); }
            set { base.Graph = value; }
        }

        public override void Populate () {
            Populate (this.Graph);
        }

        public virtual void MakeEdgeStrings (IGraph<TItem, TEdge> graph) {
            var changer = Factory as IGraphModelPropertyChanger<TItem, TEdge>;
            if (changer != null)
                foreach (var edge in graph.Edges ()) {
                    changer.SetProperty (edge, GraphExtensions.EdgeString<TItem, TEdge> (edge));
                }
        }

        public override void Populate (IGraph<TItem, TEdge> graph) {

            var lastNode1 = default (TItem);
            var lastNode2 = default (TItem); ;
            var lastNode3 = default (TItem); ;
            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    lastNode1 = Nodes[1];
                    lastNode2 = Nodes[5];
                    lastNode3 = Nodes[8];
                }
                Populate (graph, Start + 1);
                if (i > 0) {
                    var edge = CreateEdge (lastNode1, Nodes[1]);
                    graph.Add (edge);
                    if (SeperateLattice) {
                        edge = CreateEdge (lastNode2, Nodes[5]);
                        graph.Add (edge);
                    }
                    if (AddDensity) {
                        edge = CreateEdge (Nodes[2], lastNode3);
                        graph.Add (edge);
                    }
                }
            }

        }

        public virtual void Populate (IGraph<TItem, TEdge> graph, int start) {

            var item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[1] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[2] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[3] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[4] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[5] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[6] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[7] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[8] = item;

            item = CreateItem<int> ((start++));
            graph.Add (item);
            Nodes[9] = item;
            this.Start = start;

        }
    }
}