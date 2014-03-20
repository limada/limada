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

        public T Root { get; set; }
        
        public T Leaf { get; set; }

        public override string ToString() {
            return GraphExtensions.EdgeString<T,IEdge<T>>(this);
        }
    }
}