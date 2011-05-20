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
 * http://limada.sourceforge.net
 * 
 */

using System.Collections.Generic;

namespace Limaki.Graphs {
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
            return x.Root.Equals(y.Root) && (x.Leaf.Equals(y.Leaf));
        }

        public int GetHashCode(IEdge<T> obj) {
            return (obj.Leaf.GetHashCode() + 1) ^ obj.Root.GetHashCode();
        }

        #endregion
    }
}