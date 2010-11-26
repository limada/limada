using System.Windows.Controls;
using System.Windows.Media;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.Drawing.WPF.Shapes;

namespace Limaki.Drawing.WPF.Painters {
    public class WPFPainter<T>:Limaki.Drawing.Painters.Painter<T> {

        public override void Render(ISurface surface) {
            if (this.Shape is IWPFShape) {
                var shape = ( (IWPFShape) this.Shape ).Shape;

                Canvas.SetZIndex(shape, 5);
                if ((this.RenderType & RenderType.Draw) == RenderType.Draw) {
                    shape.Stroke =
                        new SolidColorBrush (
                            WPFUtils.Convert (this.Style.PenColor)
                            );
                    shape.StrokeThickness = this.Style.Pen.Thickness;
                } else {
                    shape.Stroke = null;
                }

                if ((this.RenderType & RenderType.Fill) == RenderType.Fill) {
                    shape.Fill =
                        new SolidColorBrush (
                            WPFUtils.Convert (this.Style.FillColor)
                            );
                } else {
                    shape.Fill = null;
                }
            }
        }
    }
}