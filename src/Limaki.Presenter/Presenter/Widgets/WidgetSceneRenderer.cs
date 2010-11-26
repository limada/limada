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

using Limaki.Drawing;
using Limaki.Presenter.UI;
using Limaki.Widgets;


namespace Limaki.Presenter.Widgets {
    public class WidgetRenderer : GraphItemRenderer<IWidget, IEdgeWidget> {
        public WidgetRenderer() {}

        protected object GetData(IWidget widget) {
            var data = widget.Data;
            if (data == null)
                data = "<<null>>";
            return data;
        }
        public override void Render(IWidget widget, IRenderEventArgs e) {
            var layout = this.Layout();
            var style = layout.GetStyle(widget);
            var shape = layout.GetShape(widget);

            var shapePainter = layout.GetPainter(widget.Shape.GetType());
            if (shapePainter != null) {
                shapePainter.Shape = shape;
                shapePainter.Style = style;
                shapePainter.Render(e.Surface);
            }

            bool paintData = style.PaintData;
            if (paintData) {
                var data = GetData(widget);

                var dataPainter = layout.GetPainter(data.GetType()) as IDataPainter;
                if (dataPainter != null) {
                    dataPainter.Data = data;
                    dataPainter.Style = style;
                    dataPainter.Shape = shape;
                    dataPainter.Render(e.Surface);
                }
            }
        }



    }
}
