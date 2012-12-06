/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing.Gdi;
using Limaki.Viewers;
using Limaki.View.Visuals;
using Limaki.View.Swf.Visualizers;
using Limaki.Visuals;
using Xwt.Gdi;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Rendering;

namespace Limaki.View.Swf.Visuals {

    public class VisualsSceneGdiPainter:GraphScenePainter<IVisual, IVisualEdge> {

        public virtual void Compose() {
            var composer = new GraphSceneGdiPainterComposer<IVisual, IVisualEdge>();

            this.GraphItemRenderer = new VisualsRenderer();

            composer.Factor(this);
            composer.Compose(this);
        }
             
    }
}