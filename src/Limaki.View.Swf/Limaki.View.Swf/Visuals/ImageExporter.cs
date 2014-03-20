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

using System.Drawing;
using System.Drawing.Imaging;
using Limaki.Drawing;
using Limaki.View.Gdi.UI;
using Limaki.View.Visuals;
using Rectangle = System.Drawing.Rectangle;
using Size = Xwt.Size;

namespace Limaki.View.Swf.Visuals {

    public class ImageExporter : VisualsSceneGdiPainter {
        public ImageExporter(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout)
            : base() {
            this.Data = scene;
            this.Layout = layout;
            Compose();
        }

        public virtual void Paint(Graphics g, Rectangle clipRect) {
            if (Data == null)
                return;

            var e = new GdiRenderEventArgs(g, clipRect);
            OnPaint(e);
        }

        public virtual Image ExportImage() {
            if (Data == null)
                return null;

            var size = Data.Shape.Size + new Size(Layout.Border.Width, Layout.Border.Height);
            var f = DrawingExtensions.DpiFactor(new Size(72,72));
            this.Viewport.ClipOrigin = Data.Shape.Location;

            var clipRect = new Rectangle(0, 0, (int)size.Width, (int)size.Height);
            // Create image
            var result = new Bitmap((int)(size.Width*f.Width), (int)(size.Height*f.Height), PixelFormat.Format32bppArgb);
            var g = Graphics.FromImage(result);
            g.PageUnit = GraphicsUnit.Point;
            Paint (g, clipRect);

            return result;
        }

       
    }

   
}