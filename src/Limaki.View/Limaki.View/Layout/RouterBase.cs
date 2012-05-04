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

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Visuals;

namespace Limaki.View.Layout {
    public class RouterBase<TItem, TEdge> : IRouter<TItem, TEdge>
        where TItem : IVisual
        where TEdge : TItem, IEdge<TItem> {
        public virtual void RouteEdge(TEdge edge) {
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