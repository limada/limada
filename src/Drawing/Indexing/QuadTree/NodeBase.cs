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

using System;
using System.Collections;
using System.Text;
using Coordinate = System.Drawing.PointF;
using Envelope = System.Drawing.RectangleF;
using System.Collections.Generic;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// The base class for nodes in a <c>Quadtree</c>.
    /// </summary>
    public abstract class NodeBase<TItem> {
        /// <summary> 
        /// Returns the index of the subquad that wholly contains the given envelope.
        /// If none does, returns -1.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="centre"></param>
        public static int GetSubnodeIndex(Envelope env, Coordinate centre) {
            int subnodeIndex = none;
            float envX = env.X;
            float envY = env.Y;
            float envR = env.X + env.Width;
            float envB = env.Y + env.Height;
            float centreX = centre.X;
            float centreY = centre.Y;
            if (envX >= centreX) {
                if (envY >= centreY) subnodeIndex = se;
                if (envB <= centreY) subnodeIndex = ne;
            }
            if (envR <= centreX) {
                if (envY >= centreY) subnodeIndex = sw;
                if (envB <= centreY) subnodeIndex = nw;
            }
            return subnodeIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        private ICollection<TItem> _items = new List<TItem>();
        public virtual ICollection<TItem> Items {
            get { return _items; }
            set { _items = value; }
        }

        protected const int nw = 0;
        protected const int ne = 1;
        protected const int se = 2;
        protected const int sw = 3;

        protected const int none = -1;

        /// <summary>
        /// subquads are numbered as follows:
        ///      |
        /// 0=nw | 1=ne
        /// -----+-----
        /// 3=sw | 2=se
        ///      |
        /// 
        /// lytico: original comment is wrong! (in JTS.java)
        /// 
        /// </summary>
        public Node<TItem>[] subnode = new Node<TItem>[4];

        /// <summary>
        /// 
        /// </summary>
        public NodeBase() { }

        /// <summary>
        /// 
        /// </summary>


        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasItems {
            get { return _items.Count != 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void Add(TItem item) {
            _items.Add(item);
        }

        /// <summary> 
        /// Removes a single item from this subtree.
        /// </summary>
        /// <param name="itemEnv">The envelope containing the item.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was found and removed.</returns>
        public virtual bool Remove(Envelope itemEnv, TItem item) {
            // use envelope to restrict nodes scanned
            if (!IsSearchMatch(itemEnv))
                return false;

            bool found = false;

            for (int i = 0; i < 4; i++) {
                if (subnode[i] != null) {
                    found = subnode[i].Remove(itemEnv, item);
                    if (found) {
                        // trim subtree if empty
                        if (subnode[i].IsPrunable)
                            subnode[i] = null;
                        break;
                    }
                }
            }

            // if item was found lower down, don't need to search for it here
            if (!found) {
                // otherwise, try and remove the item from the list of items in this node
                found = _items.Remove(item);
            }
            return found;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsPrunable {
            get {
                return !(HasSubNodes || HasItems);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasSubNodes {
            get {
                return (subnode[0] != null) ||
                (subnode[1] != null) ||
                (subnode[2] != null) ||
                (subnode[3] != null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsEmpty {
            get {
                bool isEmpty = true;

                if (_items.Count != 0)
                    isEmpty = false;
                else {
                    foreach (Node<TItem> sub in subnode)
                        if (sub != null && !sub.IsEmpty)
                            isEmpty = false;
                }
                return isEmpty;
            }
        }

        /// <summary>
        /// Insert items in <c>this</c> into the parameter!
        /// </summary>
        /// <param name="resultItems">IList for adding items.</param>
        /// <returns>Parameter IList with <c>this</c> items.</returns>
        public virtual ICollection<TItem> AddAllItems(ref ICollection<TItem> resultItems) {
            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            // resultItems.addAll(this.items);
            foreach (TItem o in this._items)
                resultItems.Add(o);
            foreach (Node<TItem> sub in subnode)
                if (sub != null)
                    sub.AddAllItems(ref resultItems);
            return resultItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected abstract bool IsSearchMatch(Envelope searchEnv);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="resultItems"></param>
        public virtual void AddAllItemsFromOverlapping(Envelope searchEnv, ref IList<TItem> resultItems) {
            if (!IsSearchMatch(searchEnv))
                return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            // resultItems.addAll(items);
            foreach (TItem o in this._items)
                resultItems.Add(o);

            foreach (Node<TItem> sub in subnode)
                if (sub != null)
                    sub.AddAllItemsFromOverlapping(searchEnv, ref resultItems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        public virtual void Visit(Envelope searchEnv, IItemVisitor<TItem> visitor) {
            if (!IsSearchMatch(searchEnv))
                return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            VisitItems(searchEnv, visitor);

            // TODO: lytico: here we should make getSubNodeIndex (this.envelope, searchEnv) to speed up
            foreach (Node<TItem> sub in subnode)
                if (sub != null)
                    sub.Visit(searchEnv, visitor);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        protected virtual void VisitItems(Envelope searchEnv, IItemVisitor<TItem> visitor) {
            // would be nice to filter items based on search envelope, but can't until they contain an envelope
            // lytico: what about having IItemVisitor.GetEnvelope(TItem item)
            foreach (TItem current in _items) {
                visitor.VisitItem(current);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Depth {
            get {
                int maxSubDepth = 0;
                foreach (Node<TItem> sub in subnode)
                    if (sub != null) {
                        int sqd = sub.Depth;
                        if (sqd > maxSubDepth)
                            maxSubDepth = sqd;
                    }

                return maxSubDepth + 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Count {
            get {
                int subSize = 0;
                foreach (Node<TItem> sub in subnode)
                    if (sub != null)
                        subSize += sub.Count;
                return subSize + _items.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int NodeCount {
            get {
                int subSize = 0;
                foreach (Node<TItem> sub in subnode)
                    if (sub != null)
                        subSize += sub.Count;
                return subSize + 1;
            }
        }
    }
}
