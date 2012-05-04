/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Drawing;

namespace Limaki.Visuals {

    public class VisualEdge<T>:Visual<T>, IVisualEdge {
        //public VisualEdge():base() {}
        public VisualEdge(T data):base(data) {}
        public VisualEdge(T data,IVisual root, IVisual leaf) : this(data) {
            this.Root = root;
            this.Leaf = leaf;
        }
        IVisual _root = null;
        public IVisual Root {
            get { return _root; }
            set { _root = value; }
        }
        Anchor _sourceAnchor = Anchor.Center;
        public Anchor RootAnchor {
            get { return _sourceAnchor; }
            set { _sourceAnchor = value; }
        }

        IVisual _leaf = null;
        public IVisual Leaf {
            get { return _leaf; }
            set { _leaf = value; }
        }

        Anchor _targetAnchor = Anchor.Center;
        public Anchor LeafAnchor {
            get { return _targetAnchor; }
            set { _targetAnchor = value; }
        }

        public override string ToString() {
            return String.Format("[{0}->{1},{2}]", Root, Leaf, Data);
;
        }
    }
}
