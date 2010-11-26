/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Drawing;
using System.Drawing.Imaging;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.UI;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.Paint;

namespace Limaki.Widgets {
    public class GDIImageExporter {
        Scene scene=null;
        ILayout<Scene, IWidget> layout = null;
        public GDIImageExporter(Scene scene, ILayout<Scene, IWidget> layout) {
            this.scene = scene;
            this.layout = layout;
        }

        WidgetLayerBase _layer = null;
        public WidgetLayerBase Layer {
            get {
                if (_layer == null) {
                    var matrice = new GDIMatrice ();

                    _layer = new GDIWidgetLayer (new Camera (matrice));
                    _layer.Data = scene;
                    _layer.Layout = layout;
                    matrice.Translate (-_layer.Offset.X, -_layer.Offset.Y);
                }
                return _layer;
            }
        }

        public virtual void Paint(Graphics g, Rectangle clipRect) {
            if (scene == null)
                return;

            IPaintActionEventArgs e = new GDIPaintActionEventArgs(g, clipRect);

            SolidBrush b = new SolidBrush(GDIConverter.Convert(layout.StyleSheet.BackColor));
            g.FillRectangle(b, clipRect);

            Layer.OnPaint(e);
        }

        public virtual Image ExportImage() {
            if (scene == null)
                return null;

            var layer = Layer;

            var size = layer.Size;
            Rectangle clipRect = new Rectangle(0, 0, size.Width, size.Height);
            // Create image
            Image result = new Bitmap((int)size.Width, (int)size.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(result);

            Paint (g, clipRect);

            return result;
        }
    }
}