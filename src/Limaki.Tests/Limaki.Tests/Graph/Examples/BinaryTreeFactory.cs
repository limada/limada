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


using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {

    public class BinaryTreeFactory : BinaryTreeFactory<IGraphEntity, IGraphEdge> { }

    public class BinaryTreeFactory<IGraphEntity, IGraphEdge> : BasicTestGraphFactory<IGraphEntity, IGraphEdge>
        where IGraphEdge : IEdge<IGraphEntity>, IGraphEntity {

        public override string Name {
            get { return "Binary Tree"; }
        }

        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph,int start) {

            base.Populate(Graph,start);

            IGraphEdge edge = default (IGraphEdge);

            #region Binarytree Links

            edge = CreateEdge(Nodes[1], Nodes[2]);
            Graph.Add(edge);
            Edges[1] = edge;

            edge = CreateEdge(Nodes[1], Nodes[4]);
            Graph.Add(edge);
            Edges[2] = edge;

            edge = CreateEdge(Nodes[2], Nodes[3]);
            Graph.Add(edge);
            Edges[3] = edge;

            edge = CreateEdge(Nodes[4], Nodes[5]);
            Graph.Add(edge);
            Edges[4] = edge;


            edge = CreateEdge(Nodes[4], Nodes[8]);
            Graph.Add(edge);
            Edges[5] = edge;

            edge = CreateEdge(Nodes[5], Nodes[6]);
            Graph.Add(edge);
            Edges[6] = edge;

            edge = CreateEdge(Nodes[5], Nodes[7]);
            Graph.Add(edge);
            Edges[7] = edge;


            edge = CreateEdge(Nodes[8], Nodes[9]);
            Graph.Add(edge);
            Edges[8] = edge;

            #endregion
        }
    }
}