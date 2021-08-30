/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.View.Visuals;
using Limaki.View.Viz.Visuals;
using Limaki.Drawing;

namespace Limaki.View.XwtBackend.Viz {

    public class VisualSceneContextPainter : GraphSceneContextPainter<IVisual, IVisualEdge> {

        public VisualSceneContextPainter(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : base(scene,layout, new VisualsRenderer()) {}
     
    }
}