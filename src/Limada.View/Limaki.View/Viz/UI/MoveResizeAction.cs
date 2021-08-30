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

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Rendering;
using Xwt;
using System;

namespace Limaki.View.Viz.UI {

    /// <summary>
    /// Makes a shape moveable and resizeable
    /// </summary>
    public abstract class MoveResizeAction : MouseDragActionBase, IMoveResizeAction, ICheckable {

        public MoveResizeAction(): base() {
            this.Priority = ActionPriorities.SelectionPriority;
			MovingEnabled = true;
			ResizingEnabled = true;
			DoBorderTest = true;
        }

        public virtual Func<ICamera> CameraHandler { get; set; }
        public virtual Func<ICursorHandler> CursorGetter { get; set; }

        protected ICamera Camera {
            get { return CameraHandler ();}
        }
        
        ICursorHandler _cursorHandler = null;
        public virtual ICursorHandler CursorHandler {
            get {
                if (_cursorHandler == null) {
                    _cursorHandler = this.CursorGetter();
                }
                return _cursorHandler;
            }
        }

        protected virtual Point MouseDownPos { get; set;}

        private int _hitSize = 5;
        public virtual int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        public abstract IShape Shape { get; set; }

        protected Anchor hitAnchor = Anchor.None;

        public bool ResizingEnabled { get; set; }
        public bool MovingEnabled { get; set; }

		/// <summary>
		/// HitTest tests the border
		/// </summary>
		/// <value><c>true</c> if hit on border; otherwise, <c>false</c>.</value>
        public bool DoBorderTest { get; set; }

        protected bool resizing = false;
        protected bool moving = false;

        public virtual Anchor HitAnchor (Point p) {
            var shape = this.Shape;
            if (shape == null)
                return Anchor.None;
            var sp = Camera.ToSource (p);
            var result = shape.IsAnchorHit (sp, HitSize);
            return result;
        }
        
        public abstract bool HitTest(Point p);

        public static Anchor AdjacentAnchor(Anchor hitAnchor) {

            var adjacentAnchor = Anchor.None;

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

        public override Point LastMousePos {
            get {
                return base.LastMousePos;
            }
            protected set {
                if (resizing) {
                    var _value = value;
                    switch (hitAnchor) {
                        case Anchor.MiddleTop:
                        case Anchor.MiddleBottom:
                            _value = Camera.FromSource(this.Shape[Anchor.LeftTop]);
                            _value = new Point(_value.X, value.Y);
                            break;
                        case Anchor.LeftMiddle:
                            _value = Camera.FromSource(this.Shape[Anchor.LeftTop]);
                            _value = new Point(value.X, _value.Y);
                            break;
                        case Anchor.RightMiddle:
                            _value = Camera.FromSource(this.Shape[Anchor.RightBottom]);
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
        public override void OnMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
            MouseDownPos = e.Location;
            if (e.Button == MouseActionButtons.Left) {
                resizing = false;
                hitAnchor = Anchor.None;
                CursorHandler.SaveCursor();
                moving = HitTest(e.Location);
				if (hitAnchor != Anchor.None && ResizingEnabled) {
                    var anchor = AdjacentAnchor(hitAnchor);
                    resizing = anchor != Anchor.None;
                    this.Resolved = resizing;
                    if (Resolved) {
                        moving = false;
                        this.LastMousePos = e.Location;
                        this.MouseDownPos = Camera.FromSource(this.Shape[anchor]);
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
            CursorHandler.RestoreCursor();
            resizing = false;
            moving = false;
            LastMousePos = Point.Zero;
            MouseDownPos = Point.Zero;
            hitAnchor = Anchor.None;
            if (SelectionRenderer != null)
                SelectionRenderer.UpdateSelection();
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

        public virtual bool ShowGrips { get; set; }

        protected ISelectionRenderer _selectionRenderer = null;
        public virtual ISelectionRenderer SelectionRenderer {
            get {
                if (_selectionRenderer != null) {
                    _selectionRenderer.Shape = this.Shape;
                    _selectionRenderer.ShowGrips = this.ShowGrips;
                    _selectionRenderer.Enabled = this.Enabled;
                }
                return _selectionRenderer;
            }
            set { _selectionRenderer = value; }
        }

        #endregion

        #region Dispose

        public virtual void Clear() {
            this.Shape = null;
            this.ShowGrips = false;
            if (SelectionRenderer != null) {
                SelectionRenderer.Clear ();
            }

        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                Clear();
                if (_selectionRenderer != null) {
                    _selectionRenderer.Dispose();
                }
                _selectionRenderer = null;
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ICheckable Member

        public virtual bool Check() {
            if (this.CameraHandler == null) {
                throw new CheckFailedException(this.GetType(), typeof(ICamera));
            }
            if (this.CursorGetter == null) {
                throw new CheckFailedException(this.GetType(), typeof(ICursorHandler));
            }
            return true;
        }

        #endregion
    }
}