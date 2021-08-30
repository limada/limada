/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend.Viz;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz.Visualizers {

    public class ImageExporter {

        private IGraphScene<IVisual, IVisualEdge> Scene;
        private IGraphSceneLayout<IVisual, IVisualEdge> Layout;
        public IStyleSheet StyleSheet { get; set; }

        public ImageExporter (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout) {
            this.Scene = scene;
            this.Layout = layout;
        }

        public Image ExportImage () {
            var painter = new GraphSceneContextPainter<IVisual, IVisualEdge>(Scene,Layout) {
                StyleSheet = this.StyleSheet
            };
            painter.GraphItemRenderer = new VisualsRenderer();
            painter.Compose();
            var size = Scene.Shape.Size + new Size(Layout.Border.Width, Layout.Border.Height);
            painter.Viewport.ClipOrigin = Scene.Shape.Location;

            var builder = new ImageBuilder(size.Width, size.Height);
            var ctx = builder.Context;
            painter.Paint(ctx);
            return builder.ToBitmap(ImageFormat.RGB24);
        }

        
    }
}
