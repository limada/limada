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


using Limaki.Graphs;
using Limaki.Model;

namespace Limaki.Tests.Graph.Model {
    public class BinaryTreeFactory : GraphFactoryBase {

        public override string Name {
            get { return "Binary Tree"; }
        }

        public override void Populate(IGraph<IGraphItem, IGraphEdge> Graph,int start) {

            base.Populate(Graph,start);

            GraphEdge linkWidget = null;

            #region Binarytree Links

            linkWidget = new GraphEdge(Node[1], Node[2]);
            Graph.Add(linkWidget);
            Edge[1] = linkWidget;

            linkWidget = new GraphEdge(Node[1], Node[4]);
            Graph.Add(linkWidget);
            Edge[2] = linkWidget;

            linkWidget = new GraphEdge(Node[2], Node[3]);
            Graph.Add(linkWidget);
            Edge[3] = linkWidget;

            linkWidget = new GraphEdge(Node[4], Node[5]);
            Graph.Add(linkWidget);
            Edge[4] = linkWidget;


            linkWidget = new GraphEdge(Node[4], Node[8]);
            Graph.Add(linkWidget);
            Edge[5] = linkWidget;

            linkWidget = new GraphEdge(Node[5], Node[6]);
            Graph.Add(linkWidget);
            Edge[6] = linkWidget;

            linkWidget = new GraphEdge(Node[5], Node[7]);
            Graph.Add(linkWidget);
            Edge[7] = linkWidget;


            linkWidget = new GraphEdge(Node[8], Node[9]);
            Graph.Add(linkWidget);
            Edge[8] = linkWidget;

            #endregion
        }
    }
}