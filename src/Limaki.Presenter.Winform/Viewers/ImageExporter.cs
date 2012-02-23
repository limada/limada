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

using System.Drawing;
using System.Drawing.Imaging;
using Limaki.Drawing;
using Limaki.Presenter.GDI.UI;
using Limaki.Visuals;
using Xwt;
using Rectangle = System.Drawing.Rectangle;
using Size = Xwt.Size;

namespace Limaki.Presenter.Viewers.Winform {
    public class ImageExporter : ScenePainter {
        public ImageExporter(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout)
            : base() {
            this.Data = scene;
            this.Layout = layout;
            Compose();
        }

        public virtual void Paint(Graphics g, Rectangle clipRect) {
            if (Data == null)
                return;

            var e = new GDIRenderEventArgs(g, clipRect);
            OnPaint(e);
        }

        public virtual Image ExportImage() {
            if (Data == null)
                return null;

            var size = Data.Shape.Size + new Size(Layout.Border.Width, Layout.Border.Height);

            this.Viewport.ClipOrigin = Data.Shape.Location;

            var clipRect = new Rectangle(0, 0, (int)size.Width, (int)size.Height);
            // Create image
            Image result = new Bitmap((int)size.Width, (int)size.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(result);

            Paint (g, clipRect);

            return result;
        }
    }
}