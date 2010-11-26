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


using System;
using System.Collections.Generic;
using System.Text;

namespace Limaki.Graphs {
    public class Edge<T>:IEdge<T> {

        public Edge(T root, T leaf) {
            Root = root;
            Leaf = leaf;
        }


        public T _root = default( T );
        public T Root {
            get { return _root; }
            set {_root = value;}
        }

        public T _leaf = default(T);
        public T Leaf {
            get { return _leaf; }
            set { _leaf = value; }
        }
        public override string ToString() {
            return String.Format("{0}->{1}", this.Root, this.Leaf);
        }
   }

    public class EdgeComparer<T>:IComparer<IEdge<T>> {

        #region IComparer<IEdge<T>> Member

        public int Compare(IEdge<T> x, IEdge<T> y) {
            if ((x.Leaf.Equals(y.Leaf)) && (x.Root.Equals(y.Root))) {
                return 0;
            } else if (x.Leaf.Equals(y.Root)) {
                return -1;
            } else
                return 1;
        }

        #endregion
    }

    public class EdgeEqualityComparer<T> : IEqualityComparer<IEdge<T>> {

        #region IEqualityComparer<IEdge<T>> Member

        public bool Equals(IEdge<T> x, IEdge<T> y) {
            return x.Root.Equals (y.Root) && ( x.Leaf.Equals (y.Leaf) );
        }

        public int GetHashCode(IEdge<T> obj) {
            return (obj.Leaf.GetHashCode()+1) ^ obj.Root.GetHashCode();
        }

        #endregion
    }
}
