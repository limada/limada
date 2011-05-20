/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the license below.
 *
 * Changes:
 * Adopted to work with RectangleF and PointF
 * Generic Items introduced
 * 
 * Author of changes: Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

/* NetTopologySuite is a collection of .NET classes written in C# that
implement the fundamental operations required to validate a given
geo-spatial data set to a known topological specification.

This collection of classes is a porting (with some additions and modifications) of 
JTS Topology Suite (see next license for more informations).

Copyright (C) 2005 Diego Guidi

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

For more information, contact:

    Diego Guidi
    via Po 15
	61031 Cuccurano di Fano (PU)
    diegoguidi@libero.it
    http://blogs.ugidotnet.org/gissharpblog

*/

using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using System;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// QuadRoot is the root of a single Quadtree.  
    /// It is centred at the origin,
    /// and does not have a defined extent.
    /// </summary>
    public class Root<TItem> : NodeBase<TItem> {
        // the singleton root quad is centred at the origin.
        private static readonly PointS origin = new PointS(0.0f, 0.0f);

        /// <summary>
        /// 
        /// </summary>
        public Root() { }

        /// <summary> 
        /// Insert an item into the quadtree this is the root of.
        /// </summary>
        public virtual void Add(RectangleS itemEnv, TItem item) {
            int index = GetSubnodeIndex(itemEnv, origin);
            // if index is none, itemEnv must cross the X or Y axis.
            if (index == none) {
                Add(item);
                return;
            }
            /*
            * the item must be contained in one quadrant, so insert it into the
            * tree for that quadrant (which may not yet exist)
            */
            Node<TItem> node = Subnodes[index];
            /*
            *  If the subquad doesn't exist or this item is not contained in it,
            *  have to expand the tree upward to contain the item.
            */
            if (node == null || !ShapeUtils.Contains(node.Envelope, itemEnv)) {
                Node<TItem> largerNode = Node<TItem>.CreateExpanded(node, itemEnv);
                Subnodes[index] = largerNode;
            }
            /*
            * At this point we have a subquad which exists and must contain
            * contains the env for the item.  Insert the item into the tree.
            */
            InsertContained(Subnodes[index], itemEnv, item);
        }

        /// <summary> 
        /// Insert an item which is known to be contained in the tree rooted at
        /// the given QuadNode root.  Lower levels of the tree will be created
        /// if necessary to hold the item.
        /// </summary>
        private void InsertContained(Node<TItem> tree, RectangleS itemEnv, TItem item) {
            if (!ShapeUtils.Contains(tree.Envelope, itemEnv))
                throw new Exception();
            /*
            * Do NOT create a new quad for zero-area envelopes - this would lead
            * to infinite recursion. Instead, use a heuristic of simply returning
            * the smallest existing quad containing the query
            */
            float itemEnvX = itemEnv.X;
            float itemEnvR = itemEnvX + itemEnv.Width;
            float itemEnvY = itemEnv.Y;
            float itemEnvB = itemEnvY + itemEnv.Height;
            bool isZeroX = DoubleBits.IsZeroWidth(itemEnvX, itemEnvR);
            bool isZeroY = DoubleBits.IsZeroWidth(itemEnvY, itemEnvB);
            NodeBase<TItem> node;
            if (isZeroX || isZeroY)
                node = tree.Find(itemEnv);
            else
                node = tree.GetNode(itemEnv);
            node.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(RectangleS searchEnv) {
            return true;
        }


 
    }
}
