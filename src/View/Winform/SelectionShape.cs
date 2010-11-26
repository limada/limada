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

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;

namespace Limaki.Winform {


    /// <summary>
    /// Displays a resizeable shape
    /// </summary>
    public class SelectionShape : ShapeActionBase {

        public SelectionShape(IWinControl control, ICamera camera) : base(control, camera) { }

        # region properties
        Type _shapeDataType = typeof(Rectangle);
        public virtual Type ShapeDataType {
            get { return _shapeDataType; }
            set {
                if (value != _shapeDataType) {
                    _painter = null;
                }
                _shapeDataType = value;
            }
        }

        private IShape _shape = null;
        /// <summary>
        /// the rectangle in graphics coordinates and transforms
        /// </summary>
        public override IShape Shape {
            get { return _shape; }
            set {
                if (_shape != null) {
                    _shape.Dispose();
                    //_shapeDataType = null;
                }
                _shape = value;
                if (_shape != null) {
                    // the type must be:typeof(Shape.Data) or typeof<T> 
                    // _shapeDataType = _shape.GetType ();
                }
            }
        }

        # endregion

        
        #region Selector-Handling

        protected RenderType _renderType = RenderType.Draw;
        public virtual RenderType RenderType {
            get { return _renderType; }
            set { _renderType = value; }
        }


        public virtual bool HitBorder(Point p) {
            if (Shape == null)
                return false;
            Point sp = camera.ToSource(p);
            bool result = Shape.IsBorderHit(sp, HitSize);
            return result;
        }

        

        #endregion

        # region Mouse-Handling


        public override bool HitTest(Point p) {
            bool result = false;
            if (Shape == null) {
                return result;
            }
            result = HitBorder(p);
            Anchor anchor = Anchor.None;
            if (result && ShowGrips) {
                anchor = HitAnchor (p);
                if (!Resolved) {
                    hitAnchor = anchor;
                }
            }
            SelectorHelper.SetCursor(anchor, result, p, savedCursor);

            return result;
        }

        
        public override void OnMouseHover(MouseEventArgs e) {
            base.OnMouseHover(e);
            if (!Resolved) {
                //savedCursor = control.Cursor;
            }
        }


        protected override void OnMouseMoveResolved(MouseEventArgs e) {
            // save previous shape
            IShape prevShape = (Shape != null ? (IShape)Shape.Clone() : null);
            if (moving) {
                if (Shape == null) return; // should never happen,but who knows?
                Rectangle delta = camera.ToSource(
                    Rectangle.FromLTRB(e.Location.X, e.Location.Y, LastMousePos.X, LastMousePos.Y));
                Shape.Location = Point.Subtract(Shape.Location, delta.Size);
            } else {
                // create rectangle and transform into graphics coordinates
                Rectangle rect = camera.ToSource(
                    Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                       LastMousePos.X, LastMousePos.Y)
                    );


                if (Shape == null) {
                    // Shape-Contructors normalize if usefull, otherwise not
                    Shape = ShapeFactory.Shape(ShapeDataType, rect.Location, rect.Size);
                } else {
                    // do not normalize Links!!
                    if (!(Shape is IEdgeShape)) {
                        rect = ShapeUtils.NormalizedRectangle(rect);
                    }
                    Shape.Location = rect.Location;
                    Shape.Size = rect.Size;
                }
                if (prevShape == null)
                    HitTest(e.Location);
            }

            // saving current mouse position to be used for next dragging
            this.LastMousePos = e.Location;

            // invalidate previous shape and draw the new one
            InvalidateShapeOutline(prevShape, Shape);
        }

        protected override void OnMouseMoveNotResolved(MouseEventArgs e) {
            savedCursor = Cursor.Current;
            HitTest(e.Location);
        }

        # endregion

        # region Paint

        protected override void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;

                Rectangle a = oldShape.BoundsRect;
                Rectangle b = newShape.BoundsRect;
                
                Rectangle bigger = Rectangle.Union (a, b);
                bigger = camera.FromSource(bigger);
                bigger = ShapeUtils.NormalizedRectangle(bigger);

                if (bigger.Width <=halfborder || bigger.Height <= halfborder ) {
                    bigger.Inflate(halfborder, halfborder);
                    control.Invalidate (bigger);
                    control.Update();
                } else {
                    bigger.Inflate(halfborder, halfborder);

                    Rectangle smaller = Rectangle.Intersect(a, b);
                    smaller = camera.FromSource(smaller);
                    smaller = ShapeUtils.NormalizedRectangle(smaller);
                    smaller.Inflate(-halfborder, -halfborder);

                    control.Invalidate(
                        Rectangle.FromLTRB (bigger.Left, bigger.Top, bigger.Right, smaller.Top));
                    control.Update ();
                    control.Invalidate(
                        Rectangle.FromLTRB (bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom));
                    control.Update();
                    control.Invalidate(
                        Rectangle.FromLTRB (bigger.Left, smaller.Top, smaller.Left, smaller.Bottom));
                    control.Update();
                    control.Invalidate(
                        Rectangle.FromLTRB (smaller.Right, smaller.Top, bigger.Right, smaller.Bottom));
                    control.Update();

                    //clipRegion.Intersect(smaller);
                    //clipRegion.Complement(bigger);
                }
            }
        }
        private PainterFactory _painterFactory = null;
        public PainterFactory PainterFactory {
            get { return _painterFactory; }
            set { _painterFactory = value; }
        }

        private ShapeFactory _shapeFactory = null;
        public ShapeFactory ShapeFactory {
            get { return _shapeFactory; }
            set { _shapeFactory = value; }
        }

        private IPainter _painter = null;
        public IPainter Painter {
            get {
                if ((_painter == null) && (Shape != null)) {
                    _painter = PainterFactory.CreatePainter(Shape);
                }
                return _painter;
            }
            set { _painter = value; }
        }


        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        public override void OnPaint(PaintActionEventArgs e) {
            if (Shape != null) {
                Graphics g = e.Graphics;

                // we paint the Shape transformed, otherwise it looses its line-size
                // that means, that the linesize is zoomed which makes an ugly effect
             
                if (RenderType != RenderType.None) {
                    Matrix transform = g.Transform;
                    g.Transform = emptyMatrix;
                    IShape paintShape = (IShape)this.Shape.Clone();
                    camera.FromSource(paintShape);

                    Painter.RenderType = RenderType;
                    Painter.Shape = paintShape;
                    Painter.Style = this.Style;
                    Painter.Render(e.Graphics);
                    g.Transform = transform;
                }

                // paint the grips
                base.OnPaint (e);
            }
        }

       
        public override void Clear() {
            Shape = null;
            base.Clear ();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Clear();
                Painter = null;
            }
            base.Dispose(disposing);
        }

        # endregion

        # region IAction
        public override bool Enabled {
            get {
                return base.Enabled;
            }
            set {
                bool oldVal = base.Enabled;
                base.Enabled = value;
                if (value != oldVal) {
                    this.InvalidateShapeOutline(this.Shape, this.Shape);
                }

            }
        }

        #endregion

    }
}
    



