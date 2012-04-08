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
using Limaki.Drawing.Gdi;
using Limaki.View.UI;
using Xwt;
using Xwt.Gdi;
using Xwt.Gdi.Backend;


namespace Limaki.View.Swf {
    /// <summary>
    /// Paints the grips of a rectangle
    /// </summary>
    public class GripPainter : GripPainterBase {
        public override void Render( ISurface surface ) {
            var g = ((GdiSurface)surface).Graphics;
            
            Shape.Size = new Size(GripSize, GripSize);
            innerPainter.Style = this.Style;
            int halfWidth = GripSize / 2;
            int halfHeight = GripSize / 2;

            var clipBounds = //RectangleI.Ceiling(
                GdiConverter.ToXwt(g.ClipBounds);
            // get near:
            var camera = new Camera(this.Camera.Matrice);

            foreach (Anchor anchor in Grips) {
                var anchorPoint = TargetShape[anchor];
                anchorPoint = camera.FromSource(anchorPoint);
                Shape.Location = new Point(anchorPoint.X - halfWidth, anchorPoint.Y - halfHeight);
                if (clipBounds.IntersectsWith(Shape.BoundsRect)) {
                    innerPainter.Render(surface);
                }
            }
        }


    }
}