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
 * http://www.limada.org
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
using Limaki.Common.Collections;
using Xwt;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// The base class for nodes in a <c>Quadtree</c>.
    /// </summary>
    public abstract class NodeBase<TItem> {

        public IDictionary<TItem, NodeBase<TItem>> QuadItems { get; set; }

        /// <summary> 
        /// Returns the index of the subquad that wholly contains the given envelope.
        /// If none does, returns -1.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="centre"></param>
        public static int GetSubnodeIndex(Rectangle env, Point centre) {
            var subnodeIndex = none;
            var envX = env.X;
            var envY = env.Y;
            var envR = env.X + env.Width;
            var envB = env.Y + env.Height;
            var centreX = centre.X;
            var centreY = centre.Y;
            if (envX >= centreX) {
                if (envY >= centreY)
                    subnodeIndex = se;
                if (envB <= centreY)
                    subnodeIndex = ne;
            }
            if (envR <= centreX) {
                if (envY >= centreY)
                    subnodeIndex = sw;
                if (envB <= centreY)
                    subnodeIndex = nw;
            }
            return subnodeIndex;
        }

        /// <summary>
        /// 
        /// </summary>
        private ICollection<TItem> _items = null;
        public virtual ICollection<TItem> Items {
            get { return _items ?? (_items = new Set<TItem>()); }
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
        public Node<TItem>[] Subnodes = new Node<TItem>[4];

        /// <summary>
        /// 
        /// </summary>
        public NodeBase() { }

    
        /// <summary>
        /// 
        /// </summary>
        public virtual bool HasItems {
            get { return _items != null && _items.Count != 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public virtual void AddItem(TItem item) {
            Items.Add(item);
            QuadItems[item] = this;
        }

        public virtual bool RemoveItem(TItem item) {
            return Items.Remove(item);
        }

        /// <summary> 
        /// Removes a single item from this subtree.
        /// </summary>
        /// <param name="itemEnv">The envelope containing the item.</param>
        /// <param name="item">The item to remove.</param>
        /// <returns><c>true</c> if the item was found and removed.</returns>
        public virtual bool Remove(Rectangle itemEnv, TItem item) {
            // use envelope to restrict nodes scanned
            if (!IsSearchMatch(itemEnv))
                return false;

            var found = false;

            for (int i = 0; i < 4; i++) {
                if (Subnodes[i] != null) {
                    found = Subnodes[i].Remove(itemEnv, item);
                    if (found) {
                        // trim subtree if empty
                        if (Subnodes[i].IsPrunable)
                            Subnodes[i] = null;
                        break;
                    }
                }
            }

            // if item was found lower down, don't need to search for it here
            if (!found && HasItems) {
                // otherwise, try and remove the item from the list of items in this node
                found = RemoveItem(item);
            }
            return found;
        }


        public virtual bool Remove(Rectangle nodeEnv, NodeBase<TItem> node) {
            // use envelope to restrict nodes scanned
            if (!IsSearchMatch(nodeEnv))
                return false;

            var found = false;

            for (int i = 0; i < 4; i++) {
                if (Subnodes[i] != null) {
                    found = Subnodes[i] == node || Subnodes[i].Remove(nodeEnv, node);
                    if (found) {
                        // trim subtree if empty
                        if (Subnodes[i].IsPrunable)
                            Subnodes[i] = null;
                        break;
                    } 
                }
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
                return
                    (Subnodes[0] != null) ||
                    (Subnodes[1] != null) ||
                    (Subnodes[2] != null) ||
                    (Subnodes[3] != null);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsEmpty {
            get {
                if (HasItems)
                    return false;

                foreach (var sub in Subnodes)
                    if (sub != null && !sub.IsEmpty)
                        return false;

                return true;
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
            if (HasItems)
                foreach (var o in Items)
                    resultItems.Add(o);
            foreach (var sub in Subnodes)
                if (sub != null)
                    sub.AddAllItems(ref resultItems);
            return resultItems;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected abstract bool IsSearchMatch(Rectangle searchEnv);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="resultItems"></param>
        public virtual void AddAllItemsFromOverlapping(Rectangle searchEnv, ref IList<TItem> resultItems) {
            if (!IsSearchMatch(searchEnv))
                return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            // resultItems.addAll(items);
            if (HasItems)
                foreach (var o in Items)
                    resultItems.Add(o);

            foreach (var sub in Subnodes)
                if (sub != null)
                    sub.AddAllItemsFromOverlapping(searchEnv, ref resultItems);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        public virtual void Visit(Rectangle searchEnv, IItemVisitor<TItem> visitor) {
            if (!IsSearchMatch(searchEnv))
                return;

            // this node may have items as well as subnodes (since items may not
            // be wholely contained in any single subnode
            VisitItems(searchEnv, visitor);

            // TODO: lytico: here we should make getSubNodeIndex (this.envelope, searchEnv) to speed up
            foreach (Node<TItem> sub in Subnodes)
                if (sub != null)
                    sub.Visit(searchEnv, visitor);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <param name="visitor"></param>
        protected virtual void VisitItems(Rectangle searchEnv, IItemVisitor<TItem> visitor) {
            // would be nice to filter items based on search envelope, but can't until they contain an envelope
            // lytico: what about having IItemVisitor.GetEnvelope(TItem item)
            if (HasItems)
                foreach (var current in Items) {
                    visitor.VisitItem(current);
                }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Depth {
            get {
                var maxSubDepth = 0;
                foreach (var sub in Subnodes)
                    if (sub != null) {
                        var sqd = sub.Depth;
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
                var subSize = 0;
                foreach (var sub in Subnodes)
                    if (sub != null)
                        subSize += sub.Count;
                return subSize + (_items != null ? _items.Count : 0);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int NodeCount {
            get {
                int subSize = 0;
                foreach (var sub in Subnodes)
                    if (sub != null)
                        subSize += sub.Count;
                return subSize + 1;
            }
        }
    }
}
