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
using Limaki.Visuals;
using Limaki.Graphs;

namespace Limaki.Presenter.Layout {
    public class RouterBase<TItem, TEdge> : IRouter<TItem, TEdge>
        where TItem : IVisual
        where TEdge : TItem, IEdge<TItem> {
        public virtual void routeEdge(TEdge edge) {
            var e = edge as IVisualEdge;
            if (edge.Root is IEdge<IVisual>) {
                e.RootAnchor = Anchor.Center;
            }
            if (edge.Leaf is IEdge<IVisual>) {
                e.LeafAnchor = Anchor.Center;
            }
        }
    }
}