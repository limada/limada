/*
 * Limaki 
 * Version 0.063
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

namespace Limaki.Widgets {

    public class LinkWidget<T>:Widget<T>, ILinkWidget {
        //public LinkWidget():base() {}
        public LinkWidget(T data):base(data) {}
        public LinkWidget(T data,IWidget root, IWidget leaf) : this(data) {
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
    }
}
