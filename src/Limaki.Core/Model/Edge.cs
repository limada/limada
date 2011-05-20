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


using System;
using System.Collections.Generic;
using System.Text;
using Limaki.Graphs;

namespace Limaki.Model {
    /// <summary>
    /// a generic edge class
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
            return GraphExtensions.EdgeString<T,IEdge<T>>(this);
        }
    }
}