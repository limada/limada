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


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.UI {
    public interface IGraphSceneLayer<TItem, TEdge> : ILayer<IGraphScene<TItem, TEdge>> 
    where TEdge:TItem, IEdge<TItem> {
        
        Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }
    
    }
}