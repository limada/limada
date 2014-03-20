/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.View.Visuals;
using Limaki.View.Viz.Rendering;

namespace Limaki.View.Viz.Visuals {

    public class VisualsRenderer : GraphItemRenderer<IVisual, IVisualEdge> {

        public VisualsRenderer() {}

        protected object GetData(IVisual visual) {
            var data = visual.Data;
            if (data == null)
                data = "<<null>>";
            return data;
        }

        public override void Render(IVisual visual, IRenderEventArgs e) {
            var layout = this.Layout();
            var style = layout.GetStyle(visual);
            var shape = layout.GetShape(visual);

            var shapePainter = layout.GetPainter(visual.Shape.GetType());
            if (shapePainter != null) {
                shapePainter.Shape = shape;
                shapePainter.Style = style;
                shapePainter.Render(e.Surface);
            }

            bool paintData = style.PaintData;
            if (paintData) {
                var data = GetData(visual);

                var dataPainter = layout.GetPainter(data.GetType()) as IDataPainter;
                if (dataPainter != null) {
                    dataPainter.Data = data;
                    dataPainter.Style = style;
                    dataPainter.OuterShape = shape;
                    dataPainter.Render(e.Surface);
                }
            }
        }
    }
}
