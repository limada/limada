/*
 * Limaki 
 * Version 0.081
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
using Limaki.Widgets;
using Limaki.Graphs;

namespace Limaki.Presenter.Layout {
    public class RouterBase<TItem, TEdge> : IRouter<TItem, TEdge>
        where TItem : IWidget
        where TEdge : TItem, IEdge<TItem> {
        public virtual void routeEdge(TEdge edge) {
            var e = edge as IEdgeWidget;
            if (edge.Root is IEdge<IWidget>) {
                e.RootAnchor = Anchor.Center;
            }
            if (edge.Leaf is IEdge<IWidget>) {
                e.LeafAnchor = Anchor.Center;
            }
        }
    }
}