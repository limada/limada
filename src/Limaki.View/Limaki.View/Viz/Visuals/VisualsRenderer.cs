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

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.View.Visuals;
using Limaki.View.Viz.Rendering;
using Xwt;

namespace Limaki.View.Viz.Visuals {

    public class VisualsRenderer : GraphItemRenderer<IVisual, IVisualEdge> {

        public VisualsRenderer() {}

        protected object GetData(IVisual visual) { return GetData (visual.Data); }

        protected object GetData (object data) {
            if (data == null)
                return "<<null>>";
            return data;
        }

        public override void Render(IVisual visual, IRenderEventArgs e) {
            var layout = this.Layout();
            var style = layout.GetStyle(visual);
            var shape = layout.GetShape(visual);

            var shapePainter = layout.GetPainter(shape.GetType());
            if (shapePainter != null) {
                shapePainter.Shape = shape;
                shapePainter.Style = style;
                shapePainter.Render(e.Surface);
            }

            bool paintData = style.PaintData;
            if (paintData) {

                Action<object, IShape> paint = (data, dataShape) => {
                    data = GetData (data);
                    var dataPainter = layout.GetPainter (data.GetType ()) as IDataPainter;
                    if (dataPainter != null) {
                        dataPainter.Data = data;
                        dataPainter.Style = style;
                        dataPainter.OuterShape = dataShape;
                        dataPainter.Render (e.Surface);
                    }
                };

                var enumerable = visual.Data as System.Collections.IEnumerable;
                if (!(visual.Data is string) && enumerable != null) {
                    var dimension = Dimension.X;
                    var shapeBounds = shape.BoundsRect;
                    foreach (var d in enumerable) {
                        var size = layout.GetSize(d, style);
                        if (dimension == Dimension.X)
                            size.Height = shapeBounds.Height;
                        var dshape = new RectangleShape { Location = shapeBounds.Location, Size = size };
                        paint (d, dshape);
                        if (dimension == Dimension.X)
                            shapeBounds.Location = new Point (shapeBounds.Location.X + size.Width, shapeBounds.Y);
                    }
                } else {
                    paint (visual.Data, shape);
                }
            }
        }
    }
}
