/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the license below.
 *
 * Changes:
 * Adopted to work with Xwt.Rectangle and Xwt.Point
 * Generic Items introduced
 * Add, Remove based on a dictionary <TItem,NodeBase>
 * optimized key calculation
 * 
 * Author of changes: Lytico
 *
 * http://www.limada.org
 * 
 */
using System.Collections.Generic;
using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// A Quadtree is a spatial index structure for efficient querying
    /// of 2D rectangles.  If other kinds of spatial objects
    /// need to be indexed they can be represented by their
    /// envelopes    
    /// The quadtree structure is used to provide a primary filter
    /// for range rectangle queries.  The Query() method returns a list of
    /// all objects which may intersect the query rectangle.  Note that
    /// it may return objects which do not in fact intersect.
    /// A secondary filter is required to test for exact intersection.
    /// Of course, this secondary filter may consist of other tests besides
    /// intersection, such as testing other kinds of spatial relationships.
    /// This implementation does not require specifying the extent of the inserted
    /// items beforehand.  It will automatically expand to accomodate any extent
    /// of dataset.
    /// This data structure is also known as an <c>MX-CIF quadtree</c>
    /// following the usage of Samet and others.
    /// </summary>
    public class Quadtree<TItem> {
        /// <summary>
        /// Ensure that the envelope for the inserted item has non-zero extents.
        /// Use the current minExtent to pad the envelope, if necessary.
        /// </summary>
        /// <param name="itemEnv"></param>
        /// <param name="minExtent"></param>
        public static Rectangle EnsureExtent(Rectangle itemEnv, double minExtent) {
            //The names "ensureExtent" and "minExtent" are misleading -- sounds like
            //this method ensures that the extents are greater than minExtent.
            //Perhaps we should rename them to "ensurePositiveExtent" and "defaultExtent".
            //[Jon Aquino]
            double minx = itemEnv.X;
            double maxx = itemEnv.X + itemEnv.Width;
            double miny = itemEnv.Y;
            double maxy = itemEnv.Y + itemEnv.Height;
            // has a non-zero extent
            if (minx != maxx && miny != maxy)
                return itemEnv;
            // pad one or both extents
            if (minx == maxx) {
                minx = minx - minExtent / 2.0;
                maxx = maxx + minExtent / 2.0;
            }
            if (miny == maxy) {
                miny = miny - minExtent / 2.0;
                maxy = maxy + minExtent / 2.0;
            }
            return Rectangle.FromLTRB((double)minx, (double)miny, (double)maxx, (double)maxy);
        }

        private Root<TItem> _root;
        public Root<TItem> Root {
            get { return _root; }
        }

        /// <summary>
        /// minExtent is the minimum envelope extent of all items
        /// inserted into the tree so far. It is used as a heuristic value
        /// to construct non-zero envelopes for features with zero X and/or Y extent.
        /// Start with a non-zero extent, in case the first feature inserted has
        /// a zero extent in both directions.  This value may be non-optimal, but
        /// only one feature will be inserted with this value.
        /// </summary>
        private double minExtent = 1.0;

        /// <summary>
        /// Constructs a Quadtree with zero items.
        /// </summary>
        public Quadtree() {
            _root = new Root<TItem>();
        }

        /// <summary> 
        /// Returns the number of levels in the tree.
        /// </summary>
        public virtual int Depth {
            get {
                //I don't think it's possible for root to be null. Perhaps we should
                //remove the check. [Jon Aquino]
                //Or make an assertion [Jon Aquino 10/29/2003]
                if (_root != null)
                    return _root.Depth;
                return 0;
            }
        }

        /// <summary> 
        /// Returns the number of items in the tree.
        /// </summary>
        public virtual int Count {
            get {
                if (_root != null)
                    return _root.Count;
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemEnv"></param>
        /// <param name="item"></param>
        public virtual void Add(Rectangle itemEnv, TItem item) {
            itemEnv = itemEnv.NormalizedRectangle();
            CollectStats(itemEnv);
            var insertEnv = EnsureExtent(itemEnv, minExtent);
            _root.Add(insertEnv, item);
        }

        /// <summary> 
        /// Removes a single item from the tree.
        /// </summary>
        /// <param name="itemEnv">The Envelope of the item to remove.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was found.</returns>
        public virtual bool Remove(Rectangle itemEnv, TItem item) {

            NodeBase<TItem> node = null;
            if(_root.QuadItems.TryGetValue(item, out node)){
                node.RemoveItem(item);
                _root.QuadItems.Remove (item);
                var delnode = node as Node<TItem>;
                if (delnode != null)
                    return _root.Remove(delnode.Envelope, delnode);
            }
            return false;
            itemEnv = itemEnv.NormalizedRectangle();
            var posEnv = EnsureExtent(itemEnv, minExtent);
            return _root.Remove(posEnv, item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        public virtual ICollection<TItem> Query(Rectangle searchEnv) {
            searchEnv = searchEnv.NormalizedRectangle();
            /*
            * the items that are matched are the items in quads which
            * overlap the search envelope
            */
            var visitor = new CollectionVisitor<TItem>();
            Query(searchEnv, visitor);
            return visitor.Items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        public virtual void Query(Rectangle searchEnv, IItemVisitor<TItem> visitor) {
            /*
            * the items that are matched are the items in quads which
            * overlap the search envelope
            */
            searchEnv = searchEnv.NormalizedRectangle();
            _root.Visit(searchEnv, visitor);
        }

        /// <summary>
        /// Return a list of all items in the Quadtree.
        /// </summary>
        public virtual ICollection<TItem> QueryAll() {
            ICollection<TItem> foundItems = new List<TItem>();
            _root.AddAllItems(ref foundItems);
            return foundItems;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemEnv"></param>
        private void CollectStats(Rectangle itemEnv) {
            double delX = itemEnv.Width;
            if (delX < minExtent && delX > 0.0)
                minExtent = delX;

            double delY = itemEnv.Height;
            if (delY < minExtent && delY > 0.0)
                minExtent = delY;
        }

        public virtual void QueryBounds(
            ref double left, ref double top, ref double right, ref double bottom,
            Root<TItem> root, Func<TItem, Rectangle> getBounds) {
            var l = left;
            var t = top;
            var r = right;
            var b = bottom;

            var loopStack = new Stack<NodeBase<TItem>>();
            loopStack.Push(root);

            while (loopStack.Count > 0) {
                var current = loopStack.Pop();

                if (current.HasItems) {
                    foreach (var item in current.Items) {
                        var env = getBounds (item);
                        var envX = env.X; var envY = env.Y;
                        var envR = env.Right; var envB = env.Bottom;
                        if (envX < l) l = envX;
                        if (envY < t) t = envY;
                        if (envR > r) r = envR;
                        if (envB > b) b = envB;
                    }
                }
                foreach (var node in current.Subnodes) {
                    if (node != null) {
                        var env = node.Envelope;
                        if (env.X < l || env.Y < t || env.Bottom > b || env.Right > r) {
                            loopStack.Push (node);
                        }
                    }
                }

            }

            //        System.Console.Out.WriteLine("Nodes:\t{0}\tw/items:\t{1}\tcalced:\t{2}\tcands:\t{3}\tlooped:\t{4}\tsuccess:\t{5}\t",
            //new object[] { iAllNodes, iNodesWithItems, iCalcNodes, iCandidates, iLooped, iSuccess });
            
            left = l;
            top = t;
            right = r;
            bottom = b;
        }

    }
}
