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
using System.Collections.Generic;
using Limaki.Graphs;

namespace Limaki.Tests.Graph.Model {

    public class Item<T> {
        protected T _data = default( T );
        public virtual T Data {
            get { return _data; }
            set { _data = value; }
        }
        public  Item(T data) {
            this.Data = data;
        }
        public override string ToString() {
            return Data.ToString();
        }
    }

    public class EdgeItem<T> : Item<T>, IEdge<Item<T>> {

        #region IEdge<Item> Member

        public EdgeItem ( Item<T> root, Item<T> leaf )
            : base(default(T)) {
            Root = root;
            Leaf = leaf;
        }


        public Item<T> _root = default(Item<T>);
        public Item<T> Root {
            get { return _root; }
            set { _root = value; }
        }

        public Item<T> _leaf = default(Item<T>);
        public Item<T> Leaf {
            get { return _leaf; }
            set { _leaf = value; }
        }
        public override string ToString () {
            return String.Format("[{0}->{1}]", this.Root, this.Leaf);
        }

        #endregion
    }
}