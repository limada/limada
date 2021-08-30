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
using Xwt;

namespace Limaki.Drawing.Indexing.QuadTrees {
    /// <summary>
    /// Represents a node of a <c>Quadtree</c>.  Nodes contain
    /// items which have a spatial extent corresponding to the node's position
    /// in the quadtree.
    /// </summary>
    public class Node<TItem> : NodeBase<TItem> {
        /// <summary>
        /// 
        /// </summary>
        public static Node<TItem> CreateNode(Rectangle env) {
            Key key = new Key(env);
            Node<TItem> node = new Node<TItem>(key.Envelope, key.Level);
            return node;
        }
        /// <param name="env"></param>
        /// <returns></returns>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="addEnv"></param>
        /// <returns></returns>
        public static Node<TItem> Create(Node<TItem> node, Rectangle addEnv) {
            var expandEnv = new Rectangle(addEnv.Location, addEnv.Size);
            if (node != null)
                expandEnv = DrawingExtensions.Union(expandEnv, node._envelope);


            var largerNode = CreateNode(expandEnv);

            if (node != null) {
                largerNode.QuadItems = node.QuadItems;
                largerNode.InsertNode(node);
            }
            return largerNode;
        }

        private Rectangle _envelope;
        public virtual Rectangle Envelope {
            get { return _envelope; }
        }

        private Point _centre;
        public virtual Point Centre {
            get { return _centre; }
        }
        private int level;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <param name="level"></param>
        public Node(Rectangle env, int level) {
            this._envelope = env;
            this.level = level;
            _centre = new Point((env.X + env.X + env.Width) / 2,
                (env.Y + env.Y + env.Height) / 2);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchEnv"></param>
        /// <returns></returns>
        protected override bool IsSearchMatch(Rectangle searchEnv) {
        
            return !(searchEnv.X > _envelope.X + _envelope.Width ||
                (searchEnv.X + searchEnv.Width) < (_envelope.X) ||
                (searchEnv.Y > _envelope.Y + _envelope.Height) ||
                (searchEnv.Y + searchEnv.Height) < (_envelope.Y));
        }

        /// <summary> 
        /// Returns the subquad containing the envelope.
        /// Creates the subquad if
        /// it does not already exist.
        /// </summary>
        /// <param name="searchEnv"></param>
        public virtual Node<TItem> GetNode(Rectangle searchEnv) {
            var subnodeIndex = GetSubnodeIndex(searchEnv, _centre);
            // if subquadIndex is -1 searchEnv is not contained in a subquad
            if (subnodeIndex != none) {
                // create the quad if it does not exist
                var node = GetSubnode(subnodeIndex);
                // recursively search the found/created quad
                return node.GetNode(searchEnv);
            } 
            return this;
        }

        /// <summary>
        /// Returns the smallest <i>existing</i>
        /// node containing the envelope.
        /// </summary>
        /// <param name="searchEnv"></param>
        public virtual NodeBase<TItem> Find(Rectangle searchEnv) {
            var subnodeIndex = GetSubnodeIndex(searchEnv, _centre);
            if (subnodeIndex == none)
                return this;
            if (Subnodes[subnodeIndex] != null) {
                // query lies in subquad, so search it
                var node = Subnodes[subnodeIndex];
                return node.Find(searchEnv);
            }
            // no existing subquad, so return this one anyway
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public virtual void InsertNode(Node<TItem> node) {
            if (!(_envelope == default(Rectangle) ||
                   DrawingExtensions.Contains(_envelope, node.Envelope)))
                throw new Exception();

            int index = GetSubnodeIndex(node._envelope, _centre);
            if (node.level == level - 1)
                Subnodes[index] = node;
            else {
                // the quad is not a direct child, so make a new child quad to contain it
                // and recursively insert the quad
                var childNode = CreateSubnode(index);
                childNode.InsertNode(node);
                Subnodes[index] = childNode;
            }
        }

        /// <summary>
        /// Get the subquad for the index.
        /// If it doesn't exist, create it.
        /// </summary>
        /// <param name="index"></param>
        private Node<TItem> GetSubnode(int index) {
            if (Subnodes[index] == null)
                Subnodes[index] = CreateSubnode(index);
            return Subnodes[index];
        }

        /// <summary>
        ///      |
        /// 0=nw | 1=ne
        /// -----+-----
        /// 3=sw | 2=se
        ///      |
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Node<TItem> CreateSubnode(int index) {
            // create a new subquad in the appropriate quadrant
            var minx = 0.0d; var miny = 0.0d; var maxx = 0.0d; var maxy = 0.0d;
            switch (index) {
                case nw:
                    minx = _envelope.X; miny = _envelope.Y;
                    maxx = _centre.X; maxy = _centre.Y;
                    break;
                case ne:
                    minx = _centre.X; miny = _envelope.Y;
                    maxx = _envelope.X + _envelope.Width; maxy = _centre.Y;
                    break;
                case sw:
                    minx = _envelope.X; miny = _centre.Y;
                    maxx = _centre.X; maxy = _envelope.Y + _envelope.Height;
                    break;
                case se:
                    minx = _centre.X; miny = _centre.Y;
                    maxx = _envelope.X + _envelope.Width; maxy = _envelope.Y + _envelope.Height;
                    break;
                default:
                    break;
            }
            var sqEnv = Rectangle.FromLTRB(minx, miny, maxx, maxy);
            var node = new Node<TItem>(sqEnv, level - 1) { QuadItems = this.QuadItems };
            return node;
        }

        public override bool Remove(Rectangle itemEnv, TItem item) {
            // use envelope to restrict nodes scanned
            if (!IsSearchMatch(itemEnv))
                return false;

            var found = false;
            var i = GetSubnodeIndex(itemEnv, _centre);
            if (i != none && Subnodes[i] != null) {
                found = Subnodes[i].Remove(itemEnv, item);
                if (found) {
                    // trim subtree if empty
                    if (Subnodes[i].IsPrunable)
                        Subnodes[i] = null;
                }
            }

            // if item was found lower down, don't need to search for it here
            if (!found && HasItems ) {
                // otherwise, try and remove the item from the list of items in this node
                found = RemoveItem(item);
            }
            return found;
        }

    }
}
