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
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {
    public abstract class GraphFactoryBase : GenericGraphFactory<IGraphEntity, IGraphEdge> {
        private int _start = 0;
        public int Start {
            get { return _start; }
            set { _start = value; }
        }

        public override IGraph<IGraphEntity, IGraphEdge> Graph {
            get {
                if (base.Graph == null) {
                    base.Graph = new Graph<IGraphEntity, IGraphEdge>();
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

        public virtual void MakeEdgeStrings(IGraph<IGraphEntity, IGraphEdge> Graph) {
            foreach(IGraphEdge edge in Graph.Edges()) {
                if (edge.Data is string) {
                    edge.Data = GraphExtensions.EdgeString<IGraphEntity, IGraphEdge> (edge);
                }
            }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph) {
        
            IGraphEntity lastNode1 = null;
            IGraphEntity lastNode2 = null;
            IGraphEntity lastNode3 = null;
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

        public virtual void Populate(IGraph<IGraphEntity, IGraphEdge> Graph, int start) {
            var item = new GraphEntity<int>((start++));
            Graph.Add(item);
            Node[1] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[2] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[3] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[4] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[5] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[6] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[7] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[8] = item;

            item = new GraphEntity<int>(( start++ ));
            Graph.Add(item);
            Node[9] = item;
            this.Start = start;

        }
    }
}