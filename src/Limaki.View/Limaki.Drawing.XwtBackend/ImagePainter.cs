/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using System;
using Xwt.Drawing;
using Xwt;

namespace Limaki.Drawing.XwtBackend {

    public class ImagePainter : Painter<Image>, IDataPainter<Image>, IDataPainter {

        public IShape OuterShape { get; set; }

        public override void Render (ISurface surface) {
            if (Data == null)
                return;
            
            var contextSurface = (ContextSurface) surface;
            var matrix = contextSurface.Matrix;
            if (matrix.M11 < 0.2d || matrix.M22 < 0.2d)
                return;
            var ctx = contextSurface.Context;

            var rect = OuterShape.BoundsRect.Padding (Style.Padding);
            var data = Data;
            var vecImg = Data as VectorImage;
            if (vecImg != null) {
                // TODO: draw on context direct
                data = vecImg.ToBitmap ();
            }
            if (rect.Width == data.Width && rect.Height == data.Height)
                ctx.DrawImage (data, rect.Location);
            else
                ctx.DrawImage (data, new Rectangle (0, 0, data.Width, data.Height), rect);
        }

        public Image Data { get; set; }
        object IDataPainter.Data { get { return this.Data; } set { this.Data = (Image) value; } }


        public override Point[] Measure (Matrix matrix, int delta, bool extend) {
            if (Data == null)
                return null;
            var location = Point.Zero;
            var hull = RectangleShape.Hull (new Rectangle (location, Data.Size), delta, extend);
            if (matrix != null)
                matrix.Transform (hull);
            return hull;
        }
    }
}