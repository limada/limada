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

namespace Limaki.View.Layout {
  
    
    /// <summary>
    /// This class is responsible for routing path of VisualLinks.
    /// </summary>
    public interface IRouter<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        /// <summary>
        /// Sets the RootAnchor and TargetAnchor of a VisualLink
        /// </summary>
        /// <param name="edge"></param>
        /// <returns></returns>
        void RouteEdge (TEdge edge);

    }
}
