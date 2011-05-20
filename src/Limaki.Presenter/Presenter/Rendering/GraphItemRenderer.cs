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


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter {
    public abstract class GraphItemRenderer<TItem, TEdge> : ContentRenderer<TItem>, IGraphItemRenderer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public Get<IGraphLayout<TItem, TEdge>> Layout {get;set;}


    }
}