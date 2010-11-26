/*
 * Limaki 
 * Version 0.071
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

using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Graphs;
using Limaki.Widgets;
using Limaki.Widgets.Layout;

namespace Limaki.Widgets.Layout {
    /// <summary>
    /// a helper class for collapse/expand
    /// uses a walker to iterate through subgraphs of an item
    /// </summary>
    public class ViewBuilder {
        public IGraph<IWidget, IEdgeWidget> View;
        public ICollection<IWidget> Changes = new Set<IWidget>();
        public ICollection<IWidget> NoTouch = new Set<IWidget>();
        public ViewBuilder (IGraph<IWidget, IEdgeWidget> graph) {
            this.View = graph;
        }
        public virtual bool Contains(IWidget curr) {
            return View.Contains(curr) || Changes.Contains(curr);
        }

        public virtual bool Add(IWidget curr) {
            bool result = !Contains(curr);
            if (result) {
                Changes.Add(curr);
            }
            return result;
        }

        public virtual bool AddImmediate(IWidget curr) {
            bool result = !Contains(curr);
            if (result) {
                View.Add(curr);
            }
            return result;
        }

        public void AddExpanded(IWidget root, IGraph<IWidget, IEdgeWidget> data) {

            Walker<IWidget, IEdgeWidget> walker = new Walker<IWidget, IEdgeWidget>(data);
            foreach (LevelItem<IWidget> item in walker.ExpandWalk(root, 0)) {
                this.Add(item.Node);
            }
        }

        public virtual bool Remove(IWidget curr) {
            bool result = ! Changes.Contains(curr) && ! NoTouch.Contains(curr);
            if (result) {
                Changes.Add(curr);
            }
            return result;
        }

        public virtual bool NeverRemove(IWidget curr) {
            bool result = NoTouch.Contains(curr);
            if (!result)
                NoTouch.Add (curr);
            return result;
        }

        public void RemoveCollapsed(IWidget root, IGraph<IWidget, IEdgeWidget> data) {
            Walker<IWidget, IEdgeWidget> walker = new Walker<IWidget, IEdgeWidget>(data);
            foreach (LevelItem<IWidget> item in walker.CollapseWalk(root,0)) {
                this.Remove (item.Node);
            }
        }

        public void RemoveOrphans(IGraph<IWidget, IEdgeWidget> data) {
            // remove orphans
            foreach (IWidget widget in data) {
                if (!(widget is IEdgeWidget)) {
                    bool remove = true;
                    foreach (IWidget one in data.Edges(widget)) {
                        remove = false;
                        break;
                    }
                    if (remove) {
                        this.Remove(widget);
                    }
                }
            }
        }

    }
}