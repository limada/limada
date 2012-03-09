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
using Limaki.View.Rendering;

namespace Limaki.View.UI.GraphScene {
    public interface IGraphSceneLayer<TItem, TEdge> : ILayer<IGraphScene<TItem, TEdge>> 
    where TEdge:TItem, IEdge<TItem> {
        
        Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }
    
    }
}