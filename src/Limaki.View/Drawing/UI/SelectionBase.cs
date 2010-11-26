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

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Drawing.UI {
    /// <summary>
    /// Displays a resizeable and moveable shape
    /// </summary>
    public abstract class SelectionBase : MouseDragActionBase, IPaintAction {
        public SelectionBase(): base() {
            this.Priority = ActionPriorities.SelectionPriority;
        }
        public SelectionBase(IControl control, ICamera camera): this() {
            this.control = control;
            this.camera = camera;
        }

        protected IControl control = null;
        protected ICamera camera = null;
        protected virtual PointI MouseDownPos { get; set;}

        private int _hitSize = 5;
        public virtual int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        public abstract IShape Shape { get; set; }

        protected Anchor hitAnchor = Anchor.None;
        //
        protected bool resizing = false;
        protected bool moving = false;

        ICursorHander _cursorHandler = null;
        public virtual ICursorHander CursorHandler {
            get {
                if (_cursorHandler == null) {
                    _cursorHandler = Registry.Factory.One<ICursorHander>();
                }
                return _cursorHandler;
            }
        }

        public virtual Anchor HitAnchor(PointI p) {
            IShape shape = this.Shape;
            if (shape == null)
                return Anchor.None;
            PointI sp = camera.ToSource(p);
            Anchor result = shape.IsAnchorHit(sp, HitSize);
            return result;
        }


        public abstract bool HitTest(PointI p);

        public static Anchor AdjacentAnchor(Anchor hitAnchor) {

            Anchor adjacentAnchor = Anchor.None;

            if (hitAnchor != Anchor.None)
                switch (hitAnchor) {
                    case Anchor.LeftTop:
                        //the origin is the right-bottom of the rectangle
                        adjacentAnchor = Anchor.RightBottom;
                        break;
                    case Anchor.LeftBottom:
                        //the origin is the right-top of the rectangle
                        adjacentAnchor = Anchor.RightTop;
                        break;
                    case Anchor.RightTop:
                        //the origin is the left-bottom of the rectangle
                        adjacentAnchor = Anchor.LeftBottom;
                        break;
                    case Anchor.RightBottom:
                        //the origin is the left-top of the tracker rectangle plus the little tracker offset
                        adjacentAnchor = Anchor.LeftTop;
                        break;
                    case Anchor.MiddleTop:
                        //the origin is the left-bottom of the rectangle
                        adjacentAnchor = Anchor.RightBottom;
                        break;
                    case Anchor.MiddleBottom:
                        adjacentAnchor = Anchor.RightTop;
                        //the origin is the left-top of the rectangle
                        break;
                    case Anchor.LeftMiddle:
                        //the origin is the right-top of the rectangle
                        adjacentAnchor = Anchor.RightBottom;
                        break;
                    case Anchor.RightMiddle:
                        //the origin is the left-top of the rectangle
                        adjacentAnchor = Anchor.LeftTop;
                        break;


                }

            return adjacentAnchor;
        }

        #region Mouse-Handling

        protected override PointI LastMousePos {
            get {
                return base.LastMousePos;
            }
            set {
                if (resizing) {
                    PointI _value = value;
                    switch (hitAnchor) {
                        case Anchor.MiddleTop:
                        case Anchor.MiddleBottom:
                            _value = camera.FromSource(this.Shape[Anchor.LeftTop]);
                            _value = new PointI(_value.X, value.Y);
                            break;
                        case Anchor.LeftMiddle:
                            _value = camera.FromSource(this.Shape[Anchor.LeftTop]);
                            _value = new PointI(value.X, _value.Y);
                            break;
                        case Anchor.RightMiddle:
                            _value = camera.FromSource(this.Shape[Anchor.RightBottom]);
                            _value = new PointI(value.X, _value.Y);
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
        public override void OnMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
            MouseDownPos = e.Location;
            if (e.Button == MouseActionButtons.Left) {
                resizing = false;
                hitAnchor = Anchor.None;
                CursorHandler.SaveCursor(this.control);
                moving = HitTest(e.Location);
                if (hitAnchor != Anchor.None) {
                    Anchor anchor = AdjacentAnchor(hitAnchor);
                    resizing = anchor != Anchor.None;
                    this.Resolved = resizing;
                    if (Resolved) {
                        moving = false;
                        this.LastMousePos = e.Location;
                        this.MouseDownPos = camera.FromSource(this.Shape[anchor]);
                    }
                } else {
                    Resolved = (this.Shape != null) && moving;
                }
            }
        }

        protected abstract void OnMouseMoveResolved(MouseActionEventArgs e);
        protected abstract void OnMouseMoveNotResolved(MouseActionEventArgs e);

        public override void OnMouseMove(MouseActionEventArgs e) {
            base.OnMouseMove(e);
            if (Resolved) {
                OnMouseMoveResolved(e);
            } else {
                OnMouseMoveNotResolved(e);
            }
        }

        protected override void EndAction() {
            //if (Resolved) {
            CursorHandler.RestoreCursor(control);
            resizing = false;
            moving = false;
            LastMousePos = PointI.Empty;
            MouseDownPos = PointI.Empty;
            hitAnchor = Anchor.None;
            SelectionPainter.RemoveSelection ();
            //}
            base.EndAction();
        }

        /// <summary>
        /// end dragging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(MouseActionEventArgs e) {
            base.OnMouseUp(e);
        }
        #endregion

        # region Paint

        public int GripSize {
            get { return SelectionPainter.GripSize; }
            set { this.SelectionPainter.GripSize = value; }
        }

        public IStyle Style { get;set; }
        public bool ShowGrips { get; set; }

        protected ISelectionPainter _selectionPainter = null;
        public virtual ISelectionPainter SelectionPainter {
            get {
                if (_selectionPainter == null) {
                    _selectionPainter = Registry.Factory.One<ISelectionPainter>();
                    _selectionPainter.camera = this.camera;
                    _selectionPainter.control = this.control;
                }

                return _selectionPainter;
            }
        }

        protected virtual void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            SelectionPainter.InvalidateShapeOutline(oldShape, newShape);
        }



        public virtual void OnPaint(IPaintActionEventArgs e) {
            IShape shape = this.Shape;
            if ((shape != null)) {
                SelectionPainter.Shape = shape;
                SelectionPainter.ShowGrips = this.ShowGrips;
                SelectionPainter.Style = this.Style;
                SelectionPainter.OnPaint (e);
            }
        }

        #endregion

        #region Dispose

        public virtual void Clear() {
            this.Shape = null;
            if (_selectionPainter != null) {
                _selectionPainter.Dispose();
            }
            _selectionPainter = null;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Clear();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}