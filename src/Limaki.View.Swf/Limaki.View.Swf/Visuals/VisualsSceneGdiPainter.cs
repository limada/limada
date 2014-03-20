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

using System;
using System.Drawing;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Gdi.UI;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using Xwt.Gdi.Backend;
using SWF = System.Windows.Forms;

namespace Limaki.View.Swf.Visuals {

    public class VisualsSceneGdiPainter:GraphScenePainter<IVisual, IVisualEdge> {

        public virtual void Compose() {
            var composer = new GraphSceneGdiPainterComposer<IVisual, IVisualEdge>();

            this.GraphItemRenderer = new VisualsRenderer();

            composer.Factor(this);
            composer.Compose(this);
        }

        public class GraphSceneGdiPainterComposer<TItem, TEdge> : GraphScenePainterComposer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {
            public override void Factor (GraphScenePainter<TItem, TEdge> painter) {
                base.Factor(painter);

                var layer = new GdiGraphSceneLayer<TItem, TEdge>();
                var renderer = new GraphScenePainterRenderer<IGraphScene<TItem, TEdge>>();
                renderer.Layer = layer;

                painter.BackendRenderer = renderer;
                painter.DataLayer = layer;

                painter.Viewport = new Viewport();
            }

            public override void Compose (GraphScenePainter<TItem, TEdge> display) {
                base.Compose(display);

            }

        }

        public class GraphScenePainterRenderer<T> : IBackendRenderer {
            public ILayer<T> Layer { get; set; }

            public void Render () {
                throw new NotImplementedException();
            }

            public void Render (IClipper clipper) {
                throw new NotImplementedException();
            }

            public Func<global::Xwt.Drawing.Color> BackColor { get; set; }

            public void OnPaint (IRenderEventArgs e) {
                this.OnPaint(Converter.Convert(e));
            }

            public virtual void OnPaint (SWF.PaintEventArgs e) {
                var g = e.Graphics;

                var b = new SolidBrush(GdiConverter.ToGdi(BackColor()));
                g.FillRectangle(b, e.ClipRectangle);

                Layer.OnPaint(Converter.Convert(e));

            }
        }
             
    }
}