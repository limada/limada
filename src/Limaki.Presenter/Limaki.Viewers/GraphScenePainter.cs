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

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.View;
using Limaki.View.Clipping;
using Limaki.View.Display;
using Limaki.View.Rendering;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Viewers {

    public class GraphScenePainter<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public virtual IShapeFactory ShapeFactory { get; set; }
        public virtual IPainterFactory PainterFactory { get; set; }
        public virtual IClipper Clipper { get; set; }
        public virtual IClipReceiver ClipReceiver { get; set; }

        public virtual IDisplayDevice<IGraphScene<TItem, TEdge>> Device { get; set; }
        public virtual IDeviceRenderer DeviceRenderer { get; set; }

        public virtual ILayer<IGraphScene<TItem, TEdge>> DataLayer { get; set; }
        public virtual IContentRenderer<IGraphScene<TItem, TEdge>> DataRenderer { get; set; }

        public virtual IViewport Viewport { get; set; }

        public virtual IGraphScene<TItem, TEdge> Data { get; set; }

        public virtual IGraphModelFactory<TItem, TEdge> ModelFactory { get; set; }

        public virtual IStyleSheet StyleSheet { get; set; }
        public virtual IGraphLayout<TItem, TEdge> Layout { get; set; }
        public virtual Color BackColor {
            get {
                if (Layout != null && Layout.StyleSheet != null) {
                    return Layout.StyleSheet.BackColor;
                }
                return KnownColors.FromKnownColor(KnownColor.Window);

            }
        }

        public virtual IGraphItemRenderer<TItem, TEdge> GraphItemRenderer { get; set; }

        public void OnPaint(IRenderEventArgs e) {
            DeviceRenderer.OnPaint(e);
        }
    }

    public class GraphScenePainterComposer<TItem, TEdge> : IComposer<GraphScenePainter<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {

        public virtual void Factor(GraphScenePainter<TItem, TEdge> display) {
            var context = Registry.ConcreteContext;
            display.Clipper = context.Factory.Create<IClipper>();
            display.ClipReceiver = context.Factory.Create<IClipReceiver>();
            display.DataRenderer = new GraphSceneRenderer<TItem, TEdge>();

        }

        public virtual Get<Size> DataSize { get; set; }
        public virtual Get<Point> DataOrigin { get; set; }
        public virtual Get<IClipper> Clipper { get; set; }
        public virtual Get<IViewport> Viewport { get; set; }
        public virtual Get<IDeviceRenderer> Renderer { get; set; }
        public virtual Get<ICamera> Camera { get; set; }
        public virtual Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }

        public virtual void Compose(GraphScenePainter<TItem, TEdge> display) {
            this.DataOrigin = () => {
                if (display.Data != null && display.Data.Shape != null) {
                    var result = display.Data.Shape.Location;
                    var border = display.Layout.Border;
                    if (result.X < 0) {
                        result.X -= border.Width;
                    }
                    if (result.Y < 0) {
                        result.Y -= border.Height;
                    }
                    return result;
                } else
                    return Point.Zero;
            };

            this.DataSize = () => {
                if (display.Data != null && display.Data.Shape != null) {
                    var result = display.Data.Shape.Size;
                    var border = display.Layout.Border;
                    result += border;
                    var offset = display.Data.Shape.Location;
                    if (offset.X < 0) {
                        result.Width += border.Width;
                    }
                    if (offset.Y < 0) {
                        result.Height += border.Height;
                    }
                    return result;
                } else
                    return Size.Zero;
            };

            this.Clipper = () => display.Clipper;
            this.Viewport = () => display.Viewport;
            this.Camera = () => display.Viewport.Camera;
            this.Layout = () => display.Layout;

            this.Renderer = () => display.DeviceRenderer;
            display.DeviceRenderer.BackColor = () => display.BackColor;

            display.ClipReceiver.Clipper = display.Clipper;
            display.ClipReceiver.Renderer = this.Renderer;
            display.ClipReceiver.Viewport = this.Viewport;

            display.DataLayer.Data = () => display.Data;
            display.DataLayer.Camera = this.Camera;
            display.DataLayer.Renderer = () => display.DataRenderer;

            display.Viewport.GetDataOrigin = this.DataOrigin;
            display.Viewport.GetDataSize = this.DataSize;

            display.PainterFactory = display.Layout.PainterFactory;
            display.ShapeFactory = display.Layout.ShapeFactory;

            var renderer = display.DataRenderer as IGraphSceneRenderer<TItem, TEdge>;
            renderer.ItemRenderer = display.GraphItemRenderer;
            renderer.Layout = this.Layout;
            renderer.Camera = this.Camera;
        }
    }
}