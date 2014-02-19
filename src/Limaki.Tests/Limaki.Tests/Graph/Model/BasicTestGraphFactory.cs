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


using System.Collections.Generic;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {

    public class BasicTestGraphFactory<IGraphEntity, IGraphEdge> : TestGraphFactory<IGraphEntity, IGraphEdge> 
       where IGraphEdge : IEdge<IGraphEntity> , IGraphEntity {
        
        private int _start = 0;
        public int Start {
            get { return _start; }
            set { _start = value; }
        }

        public override string Name {
            get { return this.GetType ().Name; }
        }

        public override IGraph<IGraphEntity, IGraphEdge> Graph {
            get { return base.Graph ?? (base.Graph = Creator.Graph ()); }
            set { base.Graph = value; }
        }

        public override void Populate() {
            Populate (this.Graph);
        }

        public virtual void MakeEdgeStrings (IGraph<IGraphEntity, IGraphEdge> Graph) {
            var changer = Creator as IGraphModelPropertyChanger<IGraphEntity, IGraphEdge>;
            if (changer != null)
                foreach (var edge in Graph.Edges ()) {
                    changer.SetProperty (edge, GraphExtensions.EdgeString<IGraphEntity, IGraphEdge> (edge));
                }
        }

        #region ItemFactory

        public IGraphModelFactory<IGraphEntity, IGraphEdge> Creator {
            get { return Registry.Factory.Create<IGraphModelFactory<IGraphEntity, IGraphEdge>> (); }
        }

        public virtual IGraphEdge CreateEdge(IGraphEntity root, IGraphEntity leaf) {
            return Creator.CreateEdge (root, leaf);
        }

        public virtual IGraphEdge CreateEdge () {
            return Creator.CreateEdge ();
        }

        public virtual IGraphEntity CreateItem<T> (T data) {
            return Creator.CreateItem<T> (data);
        }

        public virtual IGraphEntity CreateItem<T> () {
            return Creator.CreateItem<T> (default (T));
        }
        
        #endregion

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph) {

            IGraphEntity lastNode1 = default (IGraphEntity);
            IGraphEntity lastNode2 = default (IGraphEntity); ;
            IGraphEntity lastNode3 = default (IGraphEntity); ;
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

        public virtual void Populate(IGraph<IGraphEntity, IGraphEdge> Graph, int start) {

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