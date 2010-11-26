using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.UI;

namespace Limaki.Winform {
    public class SelectionShapePainter : SelectionPainter, ISelectionShapePainter {
        private IPainter _painter = null;
        public IPainter Painter {
            get {
                if ((_painter == null) && (Shape != null)) {
                    var factory = Registry.Pool.TryGetCreate<IPainterFactory> ();
                    _painter = factory.CreatePainter(Shape);
                }
                return _painter;
            }
            set { _painter = value; }
        }

        public RenderType RenderType { get; set; }

        public override void OnPaint(IPaintActionEventArgs e) {
            if (Shape != null) {
                System.Drawing.Graphics g = ((GDISurface)e.Surface).Graphics;


                // we paint the Shape transformed, otherwise it looses its line-size
                // that means, that the linesize is zoomed which makes an ugly effect

                if (RenderType != RenderType.None) {
                    System.Drawing.Drawing2D.Matrix transform = g.Transform;
                    g.Transform = emptyMatrix;
                    IShape paintShape = (IShape)this.Shape.Clone();
                    camera.FromSource(paintShape);

                    Painter.RenderType = RenderType;
                    Painter.Shape = paintShape;
                    Painter.Style = this.Style;
                    Painter.Render(e.Surface);
                    g.Transform = transform;
                }

                // paint the grips
                base.OnPaint(e);
            }
        }

        public override void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                RectangleI a = oldShape.BoundsRect;
                RectangleI b = newShape.BoundsRect;

                RectangleI bigger = RectangleI.Union(a, b);
                bigger = camera.FromSource(bigger);
                bigger = ShapeUtils.NormalizedRectangle(bigger);

                if (bigger.Width <= halfborder || bigger.Height <= halfborder) {
                    bigger.Inflate(halfborder, halfborder);
                    control.Invalidate(bigger);
                    control.Update();
                } else {
                    bigger.Inflate(halfborder, halfborder);

                    RectangleI smaller = RectangleI.Intersect(a, b);
                    smaller = camera.FromSource(smaller);
                    smaller = ShapeUtils.NormalizedRectangle(smaller);
                    smaller.Inflate(-halfborder, -halfborder);

                    control.Invalidate(
                        RectangleI.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top));
                    control.Update();
                    control.Invalidate(
                        RectangleI.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom));
                    control.Update();
                    control.Invalidate(
                        RectangleI.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom));
                    control.Update();
                    control.Invalidate(
                        RectangleI.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom));
                    control.Update();

                    //clipRegion.Intersect(smaller);
                    //clipRegion.Complement(bigger);
                }
            }
        }
    }
}