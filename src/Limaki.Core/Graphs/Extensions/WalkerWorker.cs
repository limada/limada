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

using System.Collections.Generic;
using Limaki.Common.Collections;
using System.Linq;
using Limaki.Common.Linqish;

namespace Limaki.Graphs.Extensions {

    /// <summary>
    /// a helper class for collapse/expand
    /// uses a walker to iterate through subgraphs of an item
    /// Adds and Removes are stored in the Changes-Collection
    /// to get the unit of works of the operations
    /// </summary>
    public class WalkerWorker<TITem, TEdge>
        where TEdge : TITem, IEdge<TITem> {

        public IGraph<TITem, TEdge> Graph;
        public ICollection<TITem> Changes = new Set<TITem>();
        public ICollection<TITem> NoTouch = new Set<TITem>();

        public WalkerWorker (IGraph<TITem, TEdge> graph) {
            this.Graph = graph;
        }

        public virtual bool Contains (TITem curr) {
            return Graph.Contains(curr) || Changes.Contains(curr);
        }

        public virtual bool Add (TITem curr) {
            bool result = !Contains(curr);
            if (result) {
                Changes.Add(curr);
            }
            return result;
        }

        public void AddExpanded (TITem root, IGraph<TITem, TEdge> data) {
            new Walker<TITem, TEdge>(data)
                .ExpandWalk(root, 0).ForEach(item => this.Add(item.Node));
        }

        public void AddDeepExpanded (TITem root, IGraph<TITem, TEdge> data) {
            new Walker<TITem, TEdge>(data)
                .DeepWalk(root, 0).ForEach(item => this.Add(item.Node));
        }

        public virtual bool Remove (TITem curr) {
            bool result = !Changes.Contains(curr) && !NoTouch.Contains(curr);
            if (result) {
                Changes.Add(curr);
            }
            return result;
        }

        public virtual void Remove (IEnumerable<LevelItem<TITem>> remove) {
            foreach (var item in remove) {
                this.Remove(item.Node);
            }
        }

        public virtual bool NeverRemove (TITem curr) {
            bool result = NoTouch.Contains(curr);
            if (!result)
                NoTouch.Add(curr);
            return result;
        }

        public void RemoveCollapsed (TITem root, IGraph<TITem, TEdge> graph) {
            new Walker<TITem, TEdge>(graph)
                .CollapseWalk(root, 0).ForEach(item => this.Remove(item.Node));
        }

        public void RemoveOrphans (IGraph<TITem, TEdge> graph) {
            graph.Where(item => !(item is TEdge) && !graph.Edges(item).Any())
                .ForEach(item=>this.Remove(item));
        }

    }
}