/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Linq;
using Limaki.Graphs;
using Limaki.View.Visuals;

namespace Limaki.View.ContentViewers {

    public class ContentVisualViewerProvider:ContentViewerProvider {

        public virtual ContentVisualViewer Supports(IGraph<IVisual, IVisualEdge> graph, IVisual contentVisual) {
            return Viewers.OfType<ContentVisualViewer>().Where(v => v.Supports(graph, contentVisual)).FirstOrDefault();
        }
    }
}