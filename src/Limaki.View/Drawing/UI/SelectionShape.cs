/*
 * Limaki 
 * Version 0.081
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
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.UI;

namespace Limaki.Drawing.UI {
    /// <summary>
    /// Displays a resizeable shape
    /// </summary>
    public class SelectionShape : SelectionBase {

        public SelectionShape(IControl control, ICamera camera) : base(control, camera) {
            
        }

        # region properties
        Type _shapeDataType = typeof(RectangleI);
        public virtual Type ShapeDataType {
            get { return _shapeDataType; }
            set {
                if (value != _shapeDataType) {
                    _selectionPainter = null;
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
                //if (_shape != null) {
                //    _shape.Dispose();
                //    //_shapeDataType = null;
                //}
                _shape = value;
                if (_shape != null) {
                    // the type must be:typeof(Shape.Data) or typeof<T> 
                    // _shapeDataType = _shape.GetType ();
                }
            }
        }

        # endregion

        
        #region Selector-Handling

        public virtual bool HitBorder(PointI p) {
            if (Shape == null)
                return false;
            PointI sp = camera.ToSource(p);
            bool result = Shape.IsBorderHit(sp, HitSize);
            return result;
        }

        

        #endregion

        # region Mouse-Handling


        public override bool HitTest(PointI p) {
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
            CursorHandler.SetCursor (this.control, anchor, result);

            return result;
        }


        public override void OnMouseHover(MouseActionEventArgs e) {
            base.OnMouseHover(e);
            if (!Resolved) {
                //savedCursor = control.Cursor;
            }
        }


        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            // save previous shape
            IShape prevShape = (Shape != null ? (IShape)Shape.Clone() : null);
            if (moving) {
                if (Shape == null) return; // should never happen,but who knows?
                RectangleI delta = camera.ToSource(
                    RectangleI.FromLTRB(e.Location.X, e.Location.Y, LastMousePos.X, LastMousePos.Y));
                Shape.Location = PointI.Subtract(Shape.Location, delta.Size);
            } else {
                // create rectangle and transform into graphics coordinates
                RectangleI rect = camera.ToSource(
                    RectangleI.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                        LastMousePos.X, LastMousePos.Y)
                    );


                if (Shape == null) {
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

        protected override void OnMouseMoveNotResolved(MouseActionEventArgs e) {
            CursorHandler.SaveCursor (this.control);
            HitTest(e.Location);
        }

        # endregion

        # region Paint

        public virtual RenderType RenderType {
            get { return ((ISelectionShapePainter)SelectionPainter).RenderType; }
            set { ((ISelectionShapePainter)SelectionPainter).RenderType = value; }
        }

        private IShapeFactory _shapeFactory = null;
        public IShapeFactory ShapeFactory {
            get {
                if (_shapeFactory == null) {
                    _shapeFactory = Registry.Factory.One<IShapeFactory>();
                }
                return _shapeFactory;
            }
           
        }

        public override ISelectionPainter SelectionPainter {
            get {
                if (_selectionPainter==null) {
                    _selectionPainter = Registry.Factory.One<ISelectionShapePainter>();
                    _selectionPainter.camera = this.camera;
                    _selectionPainter.control = this.control;
                    ( (ISelectionShapePainter) _selectionPainter ).RenderType = RenderType.Draw;
                }
                return base.SelectionPainter;
            }
        }

        public override void Clear() {
            Shape = null;
            base.Clear ();
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