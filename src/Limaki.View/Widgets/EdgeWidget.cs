/*
 * Limaki 
 * Version 0.08
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

using Limaki.Drawing;
using System;

namespace Limaki.Widgets {

    public class EdgeWidget<T>:Widget<T>, IEdgeWidget {
        //public LinkWidget():base() {}
        public EdgeWidget(T data):base(data) {}
        public EdgeWidget(T data,IWidget root, IWidget leaf) : this(data) {
            this.Root = root;
            this.Leaf = leaf;
        }
        IWidget _root = null;
        public IWidget Root {
            get { return _root; }
            set { _root = value; }
        }
        Anchor _sourceAnchor = Anchor.Center;
        public Anchor RootAnchor {
            get { return _sourceAnchor; }
            set { _sourceAnchor = value; }
        }

        IWidget _leaf = null;
        public IWidget Leaf {
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
