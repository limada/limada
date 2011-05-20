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

using Limaki.Graphs;
namespace Limaki.Presenter.Layout {
  
    
    /// <summary>
    /// This class is responsible for routing path of VisualLinks.
    /// </summary>
    public interface IRouter<TItem,TEdge> 
    where TEdge:TItem, IEdge<TItem> {
        /// <summary>
        /// Sets the RootAnchor and TargetAnchor of a VisualLink
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        void routeEdge(TEdge edge);

    }
}
