using Limaki.Common;
using System.Windows.Media;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Xwt;

namespace Limaki.View.WPF {

    public class MoveResizeRenderer : MoveResizeRendererBase {

        public override GripPainterBase GripPainter {
            get {
                if (_gripPainter == null) {
                    _gripPainter = new GripPainter();
                }
                return base.GripPainter;
            }
            set { _gripPainter = value; }
        }


        protected Transform emptyMatrix = new MatrixTransform();
        protected ISurface lastSurface = null;
        public override void OnPaint(IRenderEventArgs e) {
            var shape = this.Shape;
            lastSurface = e.Surface;
            if ((shape != null) && (ShowGrips)) {
                var g = ((WPFSurface)e.Surface).Graphics;

                //var transform = g.RenderTransform;
                
                //g.RenderTransform = new TranslateTransform(Camera.Matrice.OffsetX,Camera.Matrice.OffsetY);

                GripPainter.Render(e.Surface);

                //g.RenderTransform = transform;
            }
        }

        public override void InvalidateShapeOutline(Drawing.IShape oldShape, Drawing.IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                // the have do redraw the oldShape and newShape area
                RectangleD invalidRect = DrawingExtensions.Union(oldShape.BoundsRect, newShape.BoundsRect);
                // transform rectangle to control coordinates
                invalidRect = Camera.FromSource(invalidRect);

                invalidRect.Inflate(halfborder, halfborder);
                this.Device.Invalidate(invalidRect);
            }
        }

        public override void UpdateSelection() {
            if (!ShowGrips && lastSurface != null && Shape != null) {
                (GripPainter as GripPainter).RemoveGrips(lastSurface);
            }
        }
    }
}