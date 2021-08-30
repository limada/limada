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

using Limaki.Graphs;
using System;

namespace Limaki.View.Viz.Rendering {

    public interface IGraphSceneLayer<TItem, TEdge> : ILayer<IGraphScene<TItem, TEdge>> 
        where TEdge:TItem, IEdge<TItem> {
        
        Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
    
    }
}