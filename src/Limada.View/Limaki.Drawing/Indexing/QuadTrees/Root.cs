/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the license below.
 *
 * Changes:
 * Adopted to work with Xwt.Rectangle and Xwt.Point
 * Generic Items introduced
 * 
 * Author of changes: Lytico
 *
 * http://www.limada.org
 * 
 */
using System;
using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// QuadRoot is the root of a single Quadtree.  
    /// It is centred at the origin,
    /// and does not have a defined extent.
    /// </summary>
    public class Root<TItem> : NodeBase<TItem> {
        // the singleton root quad is centred at the origin.
        private static readonly Point origin = new Point(0.0d, 0.0d);

        /// <summary>
        /// 
        /// </summary>
        public Root() {
            QuadItems = new Dictionary<TItem, NodeBase<TItem>>();
        }

        /// <summary> 
        /// Insert an item into the quadtree this is the root of.
        /// </summary>
        public virtual void Add(Rectangle itemEnv, TItem item) {
            var index = GetSubnodeIndex(itemEnv, origin);
            // if index is none, itemEnv must cross the X or Y axis.
            if (index == none) {
                AddItem(item);
                return;
            }
            /*
            * the item must be contained in one quadrant, so insert it into the
            * tree for that quadrant (which may not yet exist)
            */
            var node = Subnodes[index];
            /*
            *  If the subquad doesn't exist or this item is not contained in it,
            *  have to expand the tree upward to contain the item.
            */
            if (node == null || !DrawingExtensions.Contains(node.Envelope, itemEnv)) {
                var largerNode = Node<TItem>.Create(node, itemEnv);
                largerNode.QuadItems = this.QuadItems;
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
        private void InsertContained(Node<TItem> tree, Rectangle itemEnv, TItem item) {
            if (!DrawingExtensions.Contains(tree.Envelope, itemEnv))
                throw new Exception();
            /*
            * Do NOT create a new quad for zero-area envelopes - this would lead
            * to infinite recursion. Instead, use a heuristic of simply returning
            * the smallest existing quad containing the query
            */
            var itemEnvX = itemEnv.X;
            var itemEnvR = itemEnvX + itemEnv.Width;
            var itemEnvY = itemEnv.Y;
            var itemEnvB = itemEnvY + itemEnv.Height;
            var isZeroX = DoubleBits.IsZeroWidth(itemEnvX, itemEnvR);
            var isZeroY = DoubleBits.IsZeroWidth(itemEnvY, itemEnvB);
            NodeBase<TItem> node;
            if (isZeroX || isZeroY)
                node = tree.Find(itemEnv);
            else
                node = tree.GetNode(itemEnv);
            node.AddItem(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(Rectangle searchEnv) {
            return true;
        }


 
    }
}
