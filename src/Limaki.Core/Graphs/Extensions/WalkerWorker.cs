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
    public class WalkerWorker<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public IGraph<TItem, TEdge> Graph;
        protected ICollection<TItem> Changes = new Set<TItem> ();
        public ICollection<TItem> Affected = new List<TItem> ();
        public ICollection<TItem> NoTouch = new Set<TItem> ();

        public WalkerWorker (IGraph<TItem, TEdge> graph) {
            this.Graph = graph;
        }

        public virtual bool Contains (TItem curr) {
            return Graph.Contains(curr) || Changes.Contains(curr);
        }

        public virtual bool Changed (TItem curr) {
            return Changes.Contains (curr);
        }

        public virtual bool Add (TItem curr) {
            bool result = !Contains (curr);
            if (result) {
                Changes.Add (curr);
                Affected.Add (curr);
            }
            return result;
        }

        public void ClearChanges () {
            Changes.Clear();
            Affected.Clear();
        }

        public void AddExpanded (TItem root, IGraph<TItem, TEdge> data) {
            new Walker<TItem, TEdge>(data)
                .ExpandWalk(root, 0).ForEach(item => this.Add(item.Node));
        }

        public void AddDeepExpanded (TItem root, IGraph<TItem, TEdge> data) {
            new Walker<TItem, TEdge>(data)
                .DeepWalk(root, 0).ForEach(item => this.Add(item.Node));
        }

        public virtual bool Remove (TItem curr) {
            bool result = !Changes.Contains(curr) && !NoTouch.Contains(curr);
            if (result) {
                Add (curr);
            }
            return result;
        }

        public virtual void Remove (IEnumerable<LevelItem<TItem>> remove) {
            foreach (var item in remove) {
                this.Remove(item.Node);
            }
        }

        public virtual bool NeverRemove (TItem curr) {
            bool result = NoTouch.Contains(curr);
            if (!result)
                NoTouch.Add(curr);
            return result;
        }

        public void RemoveCollapsed (TItem root, IGraph<TItem, TEdge> graph) {
            new Walker<TItem, TEdge>(graph)
                .CollapseWalk(root, 0).ForEach(item => this.Remove(item.Node));
        }

        public void RemoveOrphans (IGraph<TItem, TEdge> graph) {
            graph.Where(item => !(item is TEdge) && !graph.Edges(item).Any())
                .ForEach(item=>this.Remove(item));
        }


       
    }
}