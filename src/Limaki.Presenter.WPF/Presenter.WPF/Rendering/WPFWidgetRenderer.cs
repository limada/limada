using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Presenter.Widgets;
using Limaki.Widgets;
using Limaki.Presenter.UI;
using Limaki.Drawing.WPF.Shapes;
using Limaki.Drawing.WPF;
using Limaki.Drawing;
using Limaki.Widgets.WPF;
using Limaki.Drawing.WPF.Painters;

namespace Limaki.Presenter.WPF {
    public class WPFWidgetRenderer : WidgetRenderer {
        public override void Render(IWidget widget, IRenderEventArgs e) {
            var layout = this.Layout();
            var shape = widget.Shape as IWPFShape;
            var g = ((WPFSurface)e.Surface).Graphics;
            var style = layout.GetStyle(widget);

            var shapePainter = layout.GetPainter(widget.Shape.GetType());
            if (shapePainter != null) {
                if (!g.Children.Contains(shape.Shape)) {
                    shape.Shape.IsHitTestVisible = false;
                    g.Children.Add(shape.Shape);
                }
                shapePainter.Shape = shape;
                shapePainter.Style = style;
                shapePainter.Render(e.Surface);
            }
            bool paintData = style.PaintData;
            var wpfWidget = widget as IWPFWidget;
            if (paintData) {
                var data = GetData(widget);
                var dataPainter = layout.GetPainter(data.GetType()) as WPFStringPainter;
                if (dataPainter != null && wpfWidget != null) {
                    var dataElement = wpfWidget.DataElement;
                    dataPainter.DataElement = dataElement;

                    if (!g.Children.Contains(dataElement)) {
                        dataElement.IsHitTestVisible = false;
                        g.Children.Add(dataElement);
                    }
                    dataPainter.Data = data;
                    dataPainter.Style = style;
                    dataPainter.Shape = shape;
                    dataPainter.Render(e.Surface);
                }
            } else {
                if (wpfWidget != null && wpfWidget.DataElement != null) {
                    g.Children.Remove(wpfWidget.DataElement);
                }
            }
            
        }


    }
}
