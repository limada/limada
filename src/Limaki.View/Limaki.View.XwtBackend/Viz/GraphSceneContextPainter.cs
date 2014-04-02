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

            this.Viewport.ClipOrigin = Data.Shape.Location;

            OnPaint (new RenderContextEventArgs (
                context, new Rectangle (Point.Zero, Data.Shape.Size + Layout.Border)));
        }

        public virtual void Compose () {
            var composer = new GraphSceneContextPainterComposer<TItem, TEdge>();
           
            composer.Factor(this);
            composer.Compose(this);
        }
    }
}