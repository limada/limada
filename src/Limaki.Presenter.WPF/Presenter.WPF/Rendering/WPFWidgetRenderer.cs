using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Presenter.Visuals;
using Limaki.Visuals;
using Limaki.Presenter.UI;
using Limaki.Drawing.WPF.Shapes;
using Limaki.Drawing.WPF;
using Limaki.Drawing;
using Limaki.Visuals.WPF;
using Limaki.Drawing.WPF.Painters;

namespace Limaki.Presenter.WPF {
    public class WpfVisualsRenderer : VisualsRenderer {
        public override void Render(IVisual visual, IRenderEventArgs e) {
            var layout = this.Layout();
            var shape = visual.Shape as IWPFShape;
            var g = ((WPFSurface)e.Surface).Graphics;
            var style = layout.GetStyle(visual);

            var shapePainter = layout.GetPainter(visual.Shape.GetType());
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
            var wpfVisual = visual as IWPFVisual;
            if (paintData) {
                var data = GetData(visual);
                var dataPainter = layout.GetPainter(data.GetType()) as WPFStringPainter;
                if (dataPainter != null && wpfVisual != null) {
                    var dataElement = wpfVisual.DataElement;
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
                if (wpfVisual != null && wpfVisual.DataElement != null) {
                    g.Children.Remove(wpfVisual.DataElement);
                }
            }
            
        }


    }
}
