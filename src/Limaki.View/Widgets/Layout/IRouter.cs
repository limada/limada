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

namespace Limaki.Widgets.Layout {
  
    
    /// <summary>
    /// This class is responsible for routing path of link widgets.
    /// </summary>
    public interface IRouter {
        /// <summary>
        /// Sets the RootAnchor and TargetAnchor of a LinkWidget
        /// </summary>
        /// <param name="widget"></param>
        /// <returns></returns>
        void routeEdge(IEdgeWidget edge);

    }
}
