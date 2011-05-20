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
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {
    public abstract class GraphFactoryBase : GenericGraphFactory<IGraphItem, IGraphEdge> {
        private int _start = 0;
        public int Start {
            get { return _start; }
            set { _start = value; }
        }

        public override IGraph<IGraphItem, IGraphEdge> Graph {
            get {
                if (base.Graph == null) {
                    base.Graph = new Graph<IGraphItem, IGraphEdge>();
                }
                return base.Graph;
            }
            set {
                base.Graph = value;
            }
        }
        public override void Populate() {
            Populate (this.Graph);
        }

        public virtual void MakeEdgeStrings(IGraph<IGraphItem, IGraphEdge> Graph) {
            foreach(IGraphEdge edge in Graph.Edges()) {
                if (edge.Data is string) {
                    edge.Data = GraphExtensions.EdgeString<IGraphItem, IGraphEdge> (edge);
                }
            }
        }

        public override void Populate(IGraph<IGraphItem, IGraphEdge> Graph) {
        
            IGraphItem lastNode1 = null;
            IGraphItem lastNode2 = null;
            IGraphItem lastNode3 = null;
            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    lastNode1 = Node[1];
                    lastNode2 = Node[5];
                    lastNode3 = Node[8];
                }
                Populate(Graph,Start+1);
                if (i > 0) {
                    var edge = new GraphEdge(lastNode1, Node[1]);
                    Graph.Add(edge);
                    if (SeperateLattice) {
                        edge = new GraphEdge(lastNode2, Node[5]);
                        Graph.Add(edge);
                    }
                    if (AddDensity) {
                        edge = new GraphEdge(Node[2], lastNode3);
                        Graph.Add(edge);
                    }
                }
            }

        }

        public virtual void Populate(IGraph<IGraphItem, IGraphEdge> Graph, int start) {
            var item = new GraphItem<int>((start++));
            Graph.Add(item);
            Node[1] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[2] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[3] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[4] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[5] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[6] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[7] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[8] = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node[9] = item;
            this.Start = start;

        }
    }
}