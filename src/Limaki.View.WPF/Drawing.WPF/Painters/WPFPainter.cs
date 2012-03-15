using System.Windows.Media;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.WPF;
using Limaki.Drawing.WPF.Shapes;
using Xwt;
using Canvas = System.Windows.Controls.Canvas;

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

    public class WPFVectorPainter : WPFPainter<Vector>, IPainter<IVectorShape, Vector> {

    }
    public class WPFRectanglePainter : WPFPainter<Rectangle>, IPainter<IRectangleShape, Rectangle> {

    }
    public class WPFRoundedRectanglePainter : WPFRectanglePainter, IPainter<IRoundedRectangleShape, Rectangle> {

    }
    public class WPFBezierPainter : WPFPainter<Rectangle>, IPainter<IBezierShape, Rectangle> {

    }
}