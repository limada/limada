/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.Visualizers;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend.Viz {

    /// <summary>
    /// a GraphScenePainter using
    /// Xwt.Context to paint
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class GraphSceneContextPainter<TItem, TEdge> : GraphScenePainter<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public GraphSceneContextPainter (): base() { }

        public GraphSceneContextPainter (IGraphScene<TItem, TEdge> scene, IGraphSceneLayout<TItem, TEdge> layout)
            : base() {
             this.Data = scene;
             this.Layout = layout;
        }

        public GraphSceneContextPainter (IGraphScene<TItem, TEdge> scene, IGraphSceneLayout<TItem, TEdge> layout, IGraphItemRenderer<TItem, TEdge> itemRenderer)
            : this(scene,layout) {
            this.GraphItemRenderer = itemRenderer;
            Compose();
        }

        public virtual void Paint (Context context) {
            if (this.Data == null)
                return;

            var size = Data.Shape.Size + new Size (Layout.Border.Width, Layout.Border.Height);
            var f = DrawingExtensions.DpiFactor (new Size (72, 72));
            this.Viewport.ClipOrigin = Data.Shape.Location;

            var clipRect = new Rectangle (0, 0, (int) size.Width, (int) size.Height);

            var e = new RenderContextEventArgs (context, clipRect);

            OnPaint (e);
        }

        public virtual void Compose () {
            var composer = new GraphSceneContextPainterComposer<TItem, TEdge>();
           
            composer.Factor(this);
            composer.Compose(this);
        }
    }
}