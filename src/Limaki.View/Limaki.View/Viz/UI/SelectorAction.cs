/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Rendering;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// Displays a resizeable shape
    /// </summary>
    public class SelectorAction : MoveResizeAction {

        # region properties
        Type _shapeDataType = typeof(Rectangle);
        public virtual Type ShapeDataType {
            get { return _shapeDataType; }
            set {
                //if (value != _shapeDataType) {
                //    _selectionRenderer = null;
                //}
                _shapeDataType = value;
            }
        }

        private IShape _shape = null;
        /// <summary>
        /// the rectangle in graphics coordinates and transforms
        /// </summary>
        public override IShape Shape {
            get { return _shape; }
            set { _shape = value; }
        }

        # endregion

        
        #region Selector-Handling

        public virtual bool HitBorder(Point p) {
            if (Shape == null)
                return false;
            var sp = Camera.ToSource(p);
            var result = Shape.IsBorderHit(sp, HitSize);
            return result;
        }

        

        #endregion

        # region Mouse-Handling


        public override bool HitTest(Point p) {
            var result = false;
            if (Shape == null) {
                return result;
            }
            result = HitBorder(p);
            var anchor = Anchor.None;
            if (result && ShowGrips) {
                anchor = HitAnchor (p);
                if (!Resolved) {
                    hitAnchor = anchor;
                }
            }
            CursorHandler.SetCursor (anchor, result);

            return result;
        }


        public override void OnMouseHover(MouseActionEventArgs e) {
            base.OnMouseHover(e);
            if (!Resolved) {
                //savedCursor = control.Cursor;
            }
        }

        public override void OnMouseMove (MouseActionEventArgs e) {
            base.OnMouseMove(e);
        }

        protected override void OnMouseMoveResolved(MouseActionEventArgs e) {
            // save previous shape
            IShape prevShape = (Shape != null ? (IShape)Shape.Clone() : null);
            if (moving) {
                if (Shape == null) return; // should never happen,but who knows?
                Rectangle delta = Camera.ToSource(
                    Rectangle.FromLTRB(e.Location.X, e.Location.Y, LastMousePos.X, LastMousePos.Y));
                Shape.Location = Shape.Location - delta.Size;
            } else {
                // create rectangle and transform into graphics coordinates
                Rectangle rect = Camera.ToSource(
                    Rectangle.FromLTRB(MouseDownPos.X, MouseDownPos.Y,
                                        LastMousePos.X, LastMousePos.Y)
                    );


                if (Shape == null) {
                    Shape = ShapeFactory.Shape(ShapeDataType, rect.Location, rect.Size,false);
                } else {
                    // do not normalize Links!!
                    if (!(Shape is IEdgeShape)) {
                        rect = rect.NormalizedRectangle();
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
            SelectionRenderer.InvalidateShapeOutline(prevShape, Shape);
            SelectionRenderer.Shape = this.Shape;
        }

        protected override void OnMouseMoveNotResolved(MouseActionEventArgs e) {
            CursorHandler.SaveCursor ();
            HitTest(e.Location);
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            base.OnMouseUp(e);
            SelectionRenderer.Shape = null;
        }
        # endregion

        # region Paint

        public virtual RenderType RenderType {
            get { return ((IShapedSelectionRenderer)SelectionRenderer).RenderType; }
            set { ((IShapedSelectionRenderer)SelectionRenderer).RenderType = value; }
        }

        private IShapeFactory _shapeFactory = null;
        public IShapeFactory ShapeFactory {
            get {
                if (_shapeFactory == null) {
                    _shapeFactory = Registry.Factory.Create<IShapeFactory>();
                }
                return _shapeFactory;
            }
           
        }

        public override ISelectionRenderer SelectionRenderer {
            get {
                var result = _selectionRenderer as IShapedSelectionRenderer;
                if (result != null) {
                    result.RenderType = RenderType.Draw;
                    result.ShowGrips = this.ShowGrips;
                }
                return result;
            }
            set { base.SelectionRenderer = value; }
        }

        public override void Clear() {
            Shape = null;
            ShowGrips = false;
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
                    SelectionRenderer.InvalidateShapeOutline(this.Shape, this.Shape);
                }
                if (SelectionRenderer != null)
                    SelectionRenderer.Enabled = value;
            }
        }

        #endregion

    }
}