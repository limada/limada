using Limaki.Presenter.UI;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.WPF;
using Limaki.Drawing.WPF.Shapes;
using System.Windows.Media;

namespace Limaki.Presenter.WPF {

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

        public override IShape Shape {
            get { return base.Shape; }
            set {
                if (value != base.Shape) {
                    RemoveShape();
                }
                base.Shape = value;
            }
        }

        public override void Clear() {
            RemoveShape();
            base.Clear();
        }
        public RenderType RenderType { get; set; }

        public override void OnPaint(IRenderEventArgs e) {
            if (Shape != null) {
                var g = ((WPFSurface)e.Surface).Graphics;

                // we paint the Shape transformed, otherwise it looses its line-size
                // that means, that the linesize is zoomed which makes an ugly effect
                var transform = this.Camera.Matrice;
                //g.RenderTransformOrigin = new System.Windows.Point (-transform.OffsetX, -transform.OffsetY);
                //g.RenderTransform = new System.Windows.Media.TranslateTransform(transform.OffsetX, transform.OffsetY);
   
                if (RenderType != RenderType.None) {
                    var shape = ((IWPFShape)Shape).Shape;
                    //shape.RenderTransform = emptyMatrix;
                    //shape.RenderTransform = new TranslateTransform(Camera.Matrice.OffsetX, Camera.Matrice.OffsetY);


                    //Camera.FromSource(this.Shape);

                    Painter.RenderType = RenderType;
                    Painter.Shape = this.Shape;
                    Painter.Style = this.Style;
                    Painter.Render(e.Surface);

                    if (!g.Children.Contains(shape)) {
                        g.Children.Add(shape);
                    }
                }

                // paint the grips
                base.OnPaint(e);
            }
        }
        protected virtual void RemoveShape() {
            if (Shape != null && lastSurface != null) {
                var shape = ((IWPFShape)Shape).Shape;
                var g = ((WPFSurface)lastSurface).Graphics;
                g.Children.Remove(shape);
            } 
        }
        public override void UpdateSelection() {
            RemoveShape();
        }
        public override void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                RectangleI a = oldShape.BoundsRect;
                RectangleI b = newShape.BoundsRect;

                RectangleI bigger = RectangleI.Union(a, b);
                bigger = Camera.FromSource(bigger);
                bigger = ShapeUtils.NormalizedRectangle(bigger);

                if (bigger.Width <= halfborder || bigger.Height <= halfborder) {
                    bigger.Inflate(halfborder, halfborder);
                    Device.Invalidate(bigger);
                } else {
                    bigger.Inflate(halfborder, halfborder);

                    RectangleI smaller = RectangleI.Intersect(a, b);
                    smaller = Camera.FromSource(smaller);
                    smaller = ShapeUtils.NormalizedRectangle(smaller);
                    smaller.Inflate(-halfborder, -halfborder);

                    Device.Invalidate(
                        RectangleI.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top));

                    Device.Invalidate(
                        RectangleI.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom));

                    Device.Invalidate(
                        RectangleI.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom));

                    Device.Invalidate(
                        RectangleI.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom));

                }
            }
        }
    }
}