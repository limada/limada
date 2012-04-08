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
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Drawing.Gdi;
using Limaki.Viewers;
using Limaki.View.Visuals;
using Limaki.View.Swf.Display;
using Limaki.Visuals;
using Xwt.Gdi;

namespace Limaki.View.Viewers.Swf {

    public class ScenePainter:GraphScenePainter<IVisual, IVisualEdge> {

        public virtual void Compose() {
            var instrumenter = new GraphScenePainterGdiComposer<IVisual, IVisualEdge>();

            this.GraphItemRenderer = new VisualsRenderer();

            instrumenter.Factor(this);
            instrumenter.Compose(this);
        }
             
    }
}