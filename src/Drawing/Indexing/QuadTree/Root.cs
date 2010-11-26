/*
 * Limaki 
 * Version 0.07
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
using Coordinate = System.Drawing.PointF;
using Envelope = System.Drawing.RectangleF;
using System;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// QuadRoot is the root of a single Quadtree.  
    /// It is centred at the origin,
    /// and does not have a defined extent.
    /// </summary>
    public class Root<TItem> : NodeBase<TItem> {
        // the singleton root quad is centred at the origin.
        private static readonly Coordinate origin = new Coordinate(0.0f, 0.0f);

        /// <summary>
        /// 
        /// </summary>
        public Root() { }

        /// <summary> 
        /// Insert an item into the quadtree this is the root of.
        /// </summary>
        public virtual void Add(Envelope itemEnv, TItem item) {
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
            Node<TItem> node = subnode[index];
            /*
            *  If the subquad doesn't exist or this item is not contained in it,
            *  have to expand the tree upward to contain the item.
            */
            if (node == null || ! ShapeUtils.Contains(node.Envelope,itemEnv)) {
                Node<TItem> largerNode = Node<TItem>.CreateExpanded(node, itemEnv);
                subnode[index] = largerNode;
            }
            /*
            * At this point we have a subquad which exists and must contain
            * contains the env for the item.  Insert the item into the tree.
            */
            InsertContained(subnode[index], itemEnv, item);
        }

        /// <summary> 
        /// Insert an item which is known to be contained in the tree rooted at
        /// the given QuadNode root.  Lower levels of the tree will be created
        /// if necessary to hold the item.
        /// </summary>
        private void InsertContained(Node<TItem> tree, Envelope itemEnv, TItem item) {
            if(!ShapeUtils.Contains(tree.Envelope,itemEnv))
                throw new ApplicationException();
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
        protected override bool IsSearchMatch(Envelope searchEnv) {
            return true;
        }


        public static void RightBottom(Node<TItem> node, Command<ICollection<TItem>, Coordinate> maxCommand) {
            if (node != null) {
                float r = maxCommand.Parameter.X;
                float b = maxCommand.Parameter.Y;
                //bool possibleR = r >= node.Envelope.X && r <= node.Envelope.Right;
                //bool possibleB = b >= node.Envelope.Y && b <= node.Envelope.Bottom;
                //if (possibleB || possibleR) {
                if ((r < node.Envelope.Right && r >= node.Envelope.X)
                    ||
                    (b < node.Envelope.Bottom && b >= node.Envelope.Y)) {
                    if (node.Items.Count != 0) {

                        maxCommand.Target = node.Items;
                        maxCommand.Execute();

                        if (r < maxCommand.Parameter.X) {
                            r = maxCommand.Parameter.X;
                        }
                        if (b < maxCommand.Parameter.Y) {
                            b = maxCommand.Parameter.Y;
                        }
                        maxCommand.Parameter = new Coordinate(r, b);
                    }
                    foreach(Node<TItem> sub in node.subnode) {
                        if (sub != null) RightBottom(sub, maxCommand);
                    }
                }
            }
        }


        public virtual Coordinate RightBottom(Command<ICollection<TItem>, Coordinate> maxCommand) {
            ICollection<TItem> result = new List<TItem>();
            maxCommand.Parameter = new Coordinate(float.MinValue, float.MinValue);
            if (this.Items.Count != 0) {
                maxCommand.Target = this.Items;
                maxCommand.Execute();
            }
            foreach (Node<TItem> sub in subnode)
                if (sub != null) {
                    if (maxCommand.Parameter.X == float.MinValue) {
                        maxCommand.Parameter = sub.Envelope.Location;
                    }
                    RightBottom(sub, maxCommand);
                }


            return maxCommand.Parameter;
        }
    }

}
