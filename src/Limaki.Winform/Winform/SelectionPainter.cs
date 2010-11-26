using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.UI;
using System;
using Limaki.Common;

namespace Limaki.Winform {
    public class SelectionPainter : ISelectionPainter {
        public ICamera camera { get; set; }
        public IControl control { get; set; }
        public IShape Shape { get; set; }
        public IStyle Style { get; set; }
        public virtual bool ShowGrips { get; set; }
        public virtual int GripSize { get; set; }

        private GripPainter _gripPainter = null;
        public virtual GripPainter gripPainter {
            get {
                if (_gripPainter == null) {
                    _gripPainter = new GripPainter();
                }
                return _gripPainter;
            }
            set { _gripPainter = value; }
        }

        protected System.Drawing.Drawing2D.Matrix emptyMatrix =
            new System.Drawing.Drawing2D.Matrix();

        public virtual void OnPaint(IPaintActionEventArgs e) {
            IShape shape = this.Shape;
            if ((shape != null) && (ShowGrips)) {
                System.Drawing.Graphics g = ((GDISurface)e.Surface).Graphics;

                System.Drawing.Drawing2D.Matrix transform = g.Transform;
                g.Transform = emptyMatrix;

                gripPainter.gripSize = this.GripSize;
                gripPainter.camera = this.camera;
                gripPainter.Style = this.Style;
                gripPainter.TargetShape = shape;

                gripPainter.Render(e.Surface);

                g.Transform = transform;
            }
        }


        bool useRegionForClipping = true;

        private System.Drawing.Region clipRegion = new System.Drawing.Region();
        public virtual void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                if (useRegionForClipping) {
                    lock (clipRegion) {
                        clipRegion.MakeInfinite();
                        RectangleI a = oldShape.BoundsRect;
                        RectangleI b = newShape.BoundsRect;

                        RectangleI smaller = RectangleI.Intersect(a, b);
                        RectangleI bigger = RectangleI.Union(a, b);
                        smaller = camera.FromSource(smaller);
                        bigger = camera.FromSource(bigger);

                        smaller.Inflate(-halfborder, -halfborder);
                        bigger.Inflate(halfborder, halfborder);

                        // this is a mono workaround, as it don't like strange rectangles:
                        smaller = ShapeUtils.NormalizedRectangle(smaller);
                        bigger = ShapeUtils.NormalizedRectangle(bigger);

                        // this is a mono workaround, as it don't like strange rectangles:
                        if (smaller.Size == SizeI.Empty) {
                            clipRegion.Intersect(GDIConverter.Convert(bigger));
                        } else {
                            clipRegion.Intersect(
                                GDIConverter.Convert(RectangleI.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top)));
                            clipRegion.Union(
                                GDIConverter.Convert(RectangleI.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom)));
                            clipRegion.Union(
                                GDIConverter.Convert(RectangleI.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom)));
                            clipRegion.Union(
                                GDIConverter.Convert(RectangleI.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom)));

                            //clipRegion.Intersect(smaller);
                            //clipRegion.Complement(bigger);
                        }
                        ((IWinControl)control).Invalidate(clipRegion);
                    }
                } else {
                    // the have do redraw the oldShape and newShape area
                    RectangleI invalidRect = RectangleI.Union(oldShape.BoundsRect, newShape.BoundsRect);
                    // transform rectangle to control coordinates
                    invalidRect = camera.FromSource(invalidRect);

                    invalidRect.Inflate(halfborder, halfborder);
                    control.Invalidate(invalidRect);
                }
            }
        }

        public virtual void RemoveSelection() {}

        public virtual void Clear() {
            this.Shape = null;
            if (_gripPainter != null) {
                _gripPainter.Dispose();
            }
            _gripPainter = null;
        }

        protected virtual void Dispose(bool disposing) {
            if (disposing) {
                Clear();
                emptyMatrix.Dispose();
                emptyMatrix = null;
            }
        }
        public virtual void Dispose() {
            Dispose(true);
        }
    }
}