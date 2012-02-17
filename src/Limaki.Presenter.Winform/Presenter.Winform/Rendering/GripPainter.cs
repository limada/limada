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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Presenter.UI;

namespace Limaki.Presenter.Winform {
    /// <summary>
    /// Paints the grips of a rectangle
    /// </summary>
    public class GripPainter : GripPainterBase {
        public override void Render( ISurface surface ) {
            System.Drawing.Graphics g = ((GDISurface)surface).Graphics;
           
            
            Shape.Size = new SizeI(GripSize, GripSize);
            innerPainter.Style = this.Style;
            int halfWidth = GripSize / 2;
            int halfHeight = GripSize / 2;
                
            RectangleI clipBounds = 
                RectangleI.Ceiling(GDIConverter.Convert(g.ClipBounds));
            // get near:
            Camera camera = new Camera(this.Camera.Matrice);

            foreach (Anchor anchor in Grips) {
                PointI anchorPoint = TargetShape[anchor];
                anchorPoint = camera.FromSource(anchorPoint);
                Shape.Location = new PointI(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                if (clipBounds.IntersectsWith(Shape.BoundsRect)) {
                    innerPainter.Render(surface);
                }
            }
        }


    }
}