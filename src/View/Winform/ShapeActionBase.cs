/*
 * Limaki 
 * Version 0.071
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

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Winform {
    /// <summary>
    /// Displays a resizeable and moveable Shape
    /// </summary>
    public abstract class ShapeActionBase : SelectionBase {

        public ShapeActionBase(IWinControl control, ICamera camera) : base(control, camera) { }

        private int _hitSize = 5;
        public virtual int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        public abstract IShape Shape { get;set;}

        protected Anchor hitAnchor = Anchor.None;
        protected Cursor savedCursor = Cursors.Default;
        protected bool resizing = false;
        protected bool moving = false;

        public virtual Anchor HitAnchor(Point p) {
            if (Shape == null)
                return Anchor.None;
            Point sp = camera.ToSource(p);
            Anchor result = Shape.IsAnchorHit(sp, HitSize);
            return result;
        }


        public abstract bool HitTest(Point p);


        #region Mouse-Handling

        protected override Point LastMousePos {
            get {
                return base.LastMousePos;
            }
            set {
                if (resizing) {
                    Point _value = value;
                    switch (hitAnchor) {
                        case Anchor.MiddleTop:
                        case Anchor.MiddleBottom:
                            _value = camera.FromSource(Shape[Anchor.LeftTop]);
                            _value = new Point(_value.X, value.Y);
                            break;
                        case Anchor.LeftMiddle:
                            _value = camera.FromSource(Shape[Anchor.LeftTop]);
                            _value = new Point(value.X, _value.Y);
                            break;
                        case Anchor.RightMiddle:
                            _value = camera.FromSource(Shape[Anchor.RightBottom]);
                            _value = new Point(value.X, _value.Y);
                            break;
                    }
                    base.LastMousePos = _value;

                } else {
                    base.LastMousePos = value;
                }
            }
        }

        /// <summary>
        /// sets initial dragging rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) {
                resizing = false;
                hitAnchor = Anchor.None;
                savedCursor = Cursor.Current;
                moving = HitTest(e.Location);
                if (hitAnchor != Anchor.None) {
                    Anchor anchor = SelectorHelper.AdjacentAnchor(hitAnchor);
                    resizing = anchor != Anchor.None;
                    this.Resolved = resizing;
                    if (Resolved) {
                        moving = false;
                        this.LastMousePos = e.Location;
                        this.MouseDownPos = camera.FromSource(Shape[anchor]);
                    }
                } else {
                    Resolved = (Shape != null) && moving;
                }
            }
        }

        protected abstract void OnMouseMoveResolved(MouseEventArgs e);
        protected abstract void OnMouseMoveNotResolved(MouseEventArgs e);

        public override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (Resolved) {
                OnMouseMoveResolved(e);
            } else {
                OnMouseMoveNotResolved(e);
            }
        }

        protected override void EndAction() {
            //if (Resolved) {
            Cursor.Current = control.Cursor;
            resizing = false;
            moving = false;
            LastMousePos = Point.Empty;
            MouseDownPos = Point.Empty;
            hitAnchor = Anchor.None;
            //}
            base.EndAction();
        }
        /// <summary>
        /// end dragging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
        }
        #endregion

        #region Grips

        private bool _showGrips = true;
        public virtual bool ShowGrips {
            get { return _showGrips; }
            set { _showGrips = value; }
        }

        private int _gripSize = 4;
        public virtual int GripSize {
            get { return _gripSize; }
            set { _gripSize = value; }
        }

        private GripPainter _gripPainter = null;
        protected virtual GripPainter gripPainter {
            get {
                if (_gripPainter == null) {
                    _gripPainter = new GripPainter();
                }
                return _gripPainter;
            }
            set { _gripPainter = value; }
        }

        #endregion


        # region Paint

        private IStyle _style = null;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }


        private GraphicsPath clipPath = new GraphicsPath();
        protected virtual void SetClipPath(Rectangle oldRect, Rectangle newRect) {
            // oldRect and newRect are not transformed
            Rectangle bigger = Rectangle.Union(oldRect, newRect);
            Rectangle smaller = Rectangle.Intersect(oldRect, newRect);

            Rectangle a = new Rectangle(bigger.X, bigger.Y, bigger.Width, smaller.X - bigger.X);


            clipPath.Reset();

        }

        // mono under linux dont work with Region.Complement, so disable it:
        bool useRegionForClipping = true;

        private Region clipRegion = new Region();
        protected virtual void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                if (useRegionForClipping) {
                    lock (clipRegion) {
                        clipRegion.MakeInfinite();
                        Rectangle a = oldShape.BoundsRect;
                        Rectangle b = newShape.BoundsRect;

                        Rectangle smaller = Rectangle.Intersect(a, b);
                        Rectangle bigger = Rectangle.Union(a, b);
                        smaller = camera.FromSource(smaller);
                        bigger = camera.FromSource(bigger);

                        smaller.Inflate(-halfborder, -halfborder);
                        bigger.Inflate(halfborder, halfborder);

                        // this is a mono workaround, as it don't like strange rectangles:
                        smaller = ShapeUtils.NormalizedRectangle(smaller);
                        bigger = ShapeUtils.NormalizedRectangle(bigger);

                        // this is a mono workaround, as it don't like strange rectangles:
                        if (smaller.Size == Size.Empty) {
                            clipRegion.Intersect(bigger);
                        } else {
                            clipRegion.Intersect(
                                Rectangle.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top));
                            clipRegion.Union(
                                Rectangle.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom));
                            clipRegion.Union(
                                Rectangle.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom));
                            clipRegion.Union(
                                Rectangle.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom));

                            //clipRegion.Intersect(smaller);
                            //clipRegion.Complement(bigger);
                        }
                        control.Invalidate(clipRegion);
                    }
                } else {
                    // the have do redraw the oldShape and newShape area
                    Rectangle invalidRect = Rectangle.Union(oldShape.BoundsRect, newShape.BoundsRect);
                    // transform rectangle to control coordinates
                    invalidRect = camera.FromSource(invalidRect);

                    invalidRect.Inflate(halfborder, halfborder);
                    control.Invalidate(invalidRect);
                }
            }
        }


        protected Matrix emptyMatrix = new Matrix();

        public override void OnPaint(PaintActionEventArgs e) {
            if ((Shape != null) && (ShowGrips)) {
                Graphics g = e.Graphics;
                Matrix transform = g.Transform;
                g.Transform = emptyMatrix;

                gripPainter.gripSize = this.GripSize;
                gripPainter.camera = this.camera;
                gripPainter.Style = this.Style;
                gripPainter.TargetShape = this.Shape;
                gripPainter.Render(g);

                g.Transform = transform;
            }
        }

        #endregion

        #region Dispose

        public override void Clear() {
            Shape = null;
            if (_gripPainter != null) {
                _gripPainter.Dispose();
            }
            _gripPainter = null;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Clear();
                emptyMatrix.Dispose();
                emptyMatrix = null;
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}