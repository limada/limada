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


using Limaki.Common.Collections;
using System.Collections.Generic;

namespace Limaki.Graphs {
    public class WalkerBase<TItem,TEdge> where TEdge:IEdge<TItem> {
        protected IGraph<TItem, TEdge> graph = null;

        public WalkerBase(IGraph<TItem, TEdge> graph) {
            this.graph = graph;
        }

        public ICollection<TItem> Visited = new Set<TItem>();
    }
}