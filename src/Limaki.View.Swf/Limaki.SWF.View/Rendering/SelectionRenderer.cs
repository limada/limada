using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Xwt;
using Xwt.Gdi;


namespace Limaki.View.Swf {

    public class SelectionRenderer : MoveResizeRenderer, IShapedSelectionRenderer {
        private IPainter _painter = null;
        public IPainter Painter {
            get {
                if ((_painter == null) && (Shape != null)) {
                    var factory = Registry.Pool.TryGetCreate<IPainterFactory>();
                    _painter = factory.CreatePainter(Shape);
                }
                return _painter;
            }
            set { _painter = value; }
        }

        public RenderType RenderType { get; set; }

        public override void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                var a = oldShape.BoundsRect;
                var b = newShape.BoundsRect;

                var bigger = DrawingExtensions.Union(a, b);
                bigger = Camera.FromSource(bigger);
                bigger = bigger.NormalizedRectangle();

                if (bigger.Width <= halfborder || bigger.Height <= halfborder) {
                    bigger = bigger.Inflate(halfborder, halfborder);
                    Backend.Invalidate(bigger);
                    Backend.Update();
                } else {
                    bigger = bigger.Inflate(halfborder, halfborder);

                    var smaller = DrawingExtensions.Intersect(a, b);
                    smaller = Camera.FromSource(smaller);
                    smaller = smaller.NormalizedRectangle();
                    smaller = smaller.Inflate(-halfborder, -halfborder);

                    Backend.Invalidate(
                        Rectangle.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top));
                    Backend.Update();
                    Backend.Invalidate(
                        Rectangle.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom));
                    Backend.Update();
                    Backend.Invalidate(
                        Rectangle.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom));
                    Backend.Update();

                    Backend.Invalidate(
                        Rectangle.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom));
                    Backend.Update();

                }
            }
        }

        public override void OnPaint(IRenderEventArgs e) {
            if (Shape != null) {
                var g = ((GdiSurface)e.Surface).Graphics;

                // we paint the Shape transformed, otherwise it looses its line-size
                // that means, that the linesize is zoomed which makes an ugly effect

                if (RenderType != RenderType.None) {
                    //save
                    var transform = g.Transform;
                    g.Transform = emptyMatrix;

                    var paintShape = (IShape)this.Shape.Clone();
                    Camera.FromSource(paintShape);

                    Painter.RenderType = RenderType;
                    Painter.Shape = paintShape;
                    Painter.Style = this.Style;
                    Painter.Render(e.Surface);

                    //restore
                    g.Transform = transform;
                }

                // paint the grips
                base.OnPaint(e);
            }
        }

    }
}