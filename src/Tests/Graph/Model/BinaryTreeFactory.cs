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


namespace Limaki.Tests.Graph.Model {
    public class BinaryTreeFactory : BinaryGraphFactoryBase {

        public override string Name {
            get { return "Binary Tree"; }
        }

        public override void Populate(int start) {

            base.Populate(start);

            GraphEdge linkWidget = null;

            #region Binarytree Links

            linkWidget = new GraphEdge(Node1, Node2);
            Graph.Add(linkWidget);
            Link1 = linkWidget;

            linkWidget = new GraphEdge(Node1, Node4);
            Graph.Add(linkWidget);
            Link2 = linkWidget;

            linkWidget = new GraphEdge(Node2, Node3);
            Graph.Add(linkWidget);
            Link3 = linkWidget;

            linkWidget = new GraphEdge(Node4, Node5);
            Graph.Add(linkWidget);
            Link4 = linkWidget;


            linkWidget = new GraphEdge(Node4, Node8);
            Graph.Add(linkWidget);
            Link5 = linkWidget;

            linkWidget = new GraphEdge(Node5, Node6);
            Graph.Add(linkWidget);
            Link6 = linkWidget;

            linkWidget = new GraphEdge(Node5, Node7);
            Graph.Add(linkWidget);
            Link7 = linkWidget;


            linkWidget = new GraphEdge(Node8, Node9);
            Graph.Add(linkWidget);
            Link8 = linkWidget;

            #endregion
        }
    }
}