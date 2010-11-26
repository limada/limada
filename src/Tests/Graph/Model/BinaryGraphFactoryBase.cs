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
    public abstract class BinaryGraphFactoryBase : GenericGraphFactory<IGraphItem, IGraphEdge> {
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
            IGraphItem lastNode1 = null;
            IGraphItem lastNode2 = null;
            IGraphItem lastNode3 = null;
            for (int i = 0; i < Count; i++) {
                if (i > 0) {
                    lastNode1 = Node1;
                    lastNode2 = Node5;
                    lastNode3 = Node8;
                }
                Populate(Start+1);
                if (i > 0) {
                    IGraphEdge linkWidget = new GraphEdge(lastNode1, Node1);
                    Graph.Add(linkWidget);
                    if (SeperateLattice) {
                        linkWidget = new GraphEdge(lastNode2, Node5);
                        Graph.Add(linkWidget);
                    }
                    if (AddDensity) {
                        linkWidget = new GraphEdge(Node2, lastNode3);
                        Graph.Add(linkWidget);
                    }
                }
            }

        }
        
        public virtual void Populate(int start) {
            IGraphItem item = new GraphItem<int>((start++));
            Graph.Add(item);
            Node1 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node2 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node3 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node4 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node5 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node6 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node7 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node8 = item;

            item = new GraphItem<int>(( start++ ));
            Graph.Add(item);
            Node9 = item;
            this.Start = start;

        }
    }
}