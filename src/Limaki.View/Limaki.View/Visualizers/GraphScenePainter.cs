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

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.View;
using Limaki.View.Clipping;
using Limaki.View.Visualizers;
using Limaki.View.Rendering;
using Xwt;
using Xwt.Drawing;
using System;

namespace Limaki.View.Visualizers {

    public class GraphScenePainter<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public virtual IShapeFactory ShapeFactory { get; set; }
        public virtual IPainterFactory PainterFactory { get; set; }
        public virtual IClipper Clipper { get; set; }
        public virtual IClipReceiver ClipReceiver { get; set; }

        public virtual IBackendRenderer BackendRenderer { get; set; }

        public virtual ILayer<IGraphScene<TItem, TEdge>> DataLayer { get; set; }
        public virtual IContentRenderer<IGraphScene<TItem, TEdge>> DataRenderer { get; set; }

        public virtual IViewport Viewport { get; set; }

        public virtual IGraphScene<TItem, TEdge> Data { get; set; }
        public virtual IGraphItemRenderer<TItem, TEdge> GraphItemRenderer { get; set; }
        public virtual IGraphModelFactory<TItem, TEdge> ModelFactory { get; set; }

        public virtual IGraphSceneLayout<TItem, TEdge> Layout { get; set; }
        public virtual IStyleSheet StyleSheet { get; set; }
        public virtual Color BackColor {
            get {
                if (Layout != null && Layout.StyleSheet != null) {
                    return Layout.StyleSheet.BackColor;
                }
                return SystemColors.Window;

            }
        }

        public void OnPaint(IRenderEventArgs e) {
            BackendRenderer.OnPaint(e);
        }

        
    }

    public class GraphScenePainterComposer<TItem, TEdge> : IComposer<GraphScenePainter<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {

        public virtual Func<Size> DataSize { get; set; }
        public virtual Func<Point> DataOrigin { get; set; }
        public virtual Func<IClipper> Clipper { get; set; }
        public virtual Func<IViewport> Viewport { get; set; }
        public virtual Func<IBackendRenderer> BackRenderer { get; set; }
        public virtual Func<ICamera> Camera { get; set; }
        public virtual Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }

        public virtual void Factor(GraphScenePainter<TItem, TEdge> painter) {
            var context = Registry.ConcreteContext;
            painter.Clipper = context.Factory.Create<IClipper>();
            painter.ClipReceiver = context.Factory.Create<IClipReceiver>();
            painter.DataRenderer = new GraphSceneRenderer<TItem, TEdge>();

        }
       
        public virtual void Compose(GraphScenePainter<TItem, TEdge> painter) {
            this.DataOrigin = () => {
                if (painter.Data != null && painter.Data.Shape != null) {
                    var result = painter.Data.Shape.Location;
                    var border = painter.Layout.Border;
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
                if (painter.Data != null && painter.Data.Shape != null) {
                    var result = painter.Data.Shape.Size;
                    var border = painter.Layout.Border;
                    result += border;
                    var offset = painter.Data.Shape.Location;
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

            this.Clipper = () => painter.Clipper;
            this.Viewport = () => painter.Viewport;
            this.Camera = () => painter.Viewport.Camera;
            this.Layout = () => painter.Layout;

            this.BackRenderer = () => painter.BackendRenderer;
            painter.BackendRenderer.BackColor = () => painter.BackColor;

            painter.ClipReceiver.Clipper = painter.Clipper;
            painter.ClipReceiver.Renderer = this.BackRenderer;
            painter.ClipReceiver.Viewport = this.Viewport;

            painter.DataLayer.Data = () => painter.Data;
            painter.DataLayer.Camera = this.Camera;
            painter.DataLayer.Renderer = () => painter.DataRenderer;

            painter.Viewport.GetDataOrigin = this.DataOrigin;
            painter.Viewport.GetDataSize = this.DataSize;

            painter.PainterFactory = painter.Layout.PainterFactory;
            painter.ShapeFactory = painter.Layout.ShapeFactory;

            var renderer = painter.DataRenderer as IGraphSceneRenderer<TItem, TEdge>;
            renderer.ItemRenderer = painter.GraphItemRenderer;
            renderer.Layout = this.Layout;
            renderer.Camera = this.Camera;
        }
    }
}