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

using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Graphs;
using Limaki.Presenter.GDI.UI;
using Limaki.Presenter.UI;
using Limaki.Presenter.Visuals;
using Limaki.Presenter.Winform;
using Limaki.Presenter.Winform.Display;
using Limaki.Visuals;

namespace Limaki.Presenter.Viewers.Winform {
    public class GraphScenePainterGdiComposer<TItem, TEdge> : GraphScenePainterComposer<TItem, TEdge>
    where TEdge : TItem, IEdge<TItem> {
        public override void Factor(GraphScenePainter<TItem, TEdge> display) {
            base.Factor(display);

            var layer = new GDIGraphSceneLayer<TItem, TEdge>();
            var renderer = new GraphScenePainterRenderer<IGraphScene<TItem, TEdge>>();
            renderer.Layer = layer;

            display.DeviceRenderer = renderer;
            display.DataLayer = layer;

            display.Viewport = new GDIViewPort();
        }



        public override void Compose(GraphScenePainter<TItem, TEdge> display) {
            base.Compose(display);

        }

    }

    public class GraphScenePainterRenderer<T> : IDeviceRenderer {
        public ILayer<T> Layer { get; set; }

        public void Render() {
            throw new NotImplementedException();
        }

        public void Render(IClipper clipper) {
            throw new NotImplementedException();
        }

        public Get<Drawing.Color> BackColor { get; set; }

        public void OnPaint(IRenderEventArgs e) {
            this.OnPaint(Converter.Convert(e));
        }

        public virtual void OnPaint(PaintEventArgs e) {
            var g = e.Graphics;

            var b = new SolidBrush(GDIConverter.Convert(BackColor()));
            g.FillRectangle(b, e.ClipRectangle);

            Layer.OnPaint(Converter.Convert(e));

        }
    }

    public class ScenePainter:GraphScenePainter<IVisual, IVisualEdge> {
        public virtual void Instrument() {
            var instrumenter = new GraphScenePainterGdiComposer<IVisual, IVisualEdge>();

            this.GraphItemRenderer = new VisualsRenderer();

            instrumenter.Factor(this);
            instrumenter.Compose(this);
        }
             
    }
}