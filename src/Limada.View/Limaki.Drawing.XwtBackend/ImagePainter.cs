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

using System;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Xwt.Drawing;
using Xwt;
using Xwt.Backends;

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
            if (rect.Width < 0.2d || rect.Height < 0.2d)
                return;

            var data = Data;
            var size = data.Size;

            var ratio = Math.Min (rect.Width / size.Width, rect.Height / size.Height);
            var loc = new Point (
                rect.Location.X + (rect.Width - size.Width * ratio) / 2,
                rect.Location.Y + (rect.Height - size.Height * ratio) / 2);

            var vectorImage = Data as VectorImage;
            if (vectorImage != null) {
                rect.Location = loc;
                DrawImage (vectorImage, ctx, rect, ratio);
                return;
            }

            var paintingImage = Data as PaintingImage;
            if (paintingImage != null) {
                rect.Location = loc;
                DrawImage (paintingImage, ctx, rect, ratio);
                return;
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
            if (OuterShape != null)
                location = OuterShape.Location;
            var hull = RectangleShape.Hull (new Rectangle (location, Data.Size), delta, extend);
            if (matrix != null && !matrix.IsIdentity)
                matrix.Transform (hull);
            return hull;
        }
        
        protected virtual void DrawImage (VectorImage image, Context ctx, Rectangle bounds, double ratio) {
            ctx.Save ();
            ctx.Translate (bounds.Location);
            var size = image.Size;

            var data = image.Data;
            var engine = ((IFrontend) image).ToolkitEngine;
            engine.VectorImageRecorderContextHandler.Draw (ctx.Handler, Toolkit.GetBackend (ctx), data);
            ctx.Restore ();
        }

        protected virtual void DrawImage (PaintingImage image, Context ctx, Rectangle bounds, double ratio) {
            ctx.Save ();
            ctx.Translate (bounds.Location);
            var size = image.Size;
            ctx.Scale (ratio, ratio);
            image.Paint (ctx);

            ctx.Restore ();
        }
    }
}