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
    public class BinaryGraphFactory : BinaryGraphFactoryBase {

        public override string Name {
            get { return "Binary Graph"; }
        }

        public override void Populate(int start) {

            base.Populate(start);

            GraphEdge edge = null;

            #region Binarytree Edges

            edge = new GraphEdge(Node1, Node2);
            Graph.Add(edge);
            Link1 = edge;

            edge = new GraphEdge(Node4, Node3);
            Graph.Add(edge);
            Link2 = edge;

            edge = new GraphEdge(Link1, Link2);
            Graph.Add(edge);
            Link3 = edge;

            edge = new GraphEdge(Node5, Node8);
            Graph.Add(edge);
            Link4 = edge;


            edge = new GraphEdge(Node5, Node6);
            Graph.Add(edge);
            Link5 = edge;

            edge = new GraphEdge(Node5, Node7);
            Graph.Add(edge);
            Link6 = edge;

            edge = new GraphEdge(Node8, Node9);
            Graph.Add(edge);
            Link7 = edge;

            if (!SeperateLattice) {
                edge = new GraphEdge (Link2, Link4);
                Graph.Add (edge);
                Link8 = edge;
            }

            #endregion
        }
    }
}