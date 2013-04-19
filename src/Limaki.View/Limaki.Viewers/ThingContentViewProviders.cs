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
using Limaki.Visuals;

namespace Limaki.Viewers {

    public class ThingContentViewProviders:ContentViewProviders {

        public ThingViewerController Supports(IGraph<IVisual, IVisualEdge> graph, IVisual thing) {
            return Viewers.OfType<ThingViewerController>().Where(v => v.Supports(graph, thing)).FirstOrDefault();
        }
    }
}