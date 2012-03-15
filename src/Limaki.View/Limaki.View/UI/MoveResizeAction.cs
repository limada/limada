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
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.UI {
    /// <summary>
    /// Makes a shape moveable and resizeable
    /// </summary>
    public abstract class MoveResizeAction : MouseDragActionBase, IMoveResizeAction, ICheckable {
        public MoveResizeAction(): base() {
            this.Priority = ActionPriorities.SelectionPriority;
        }

        public virtual Get<ICamera> CameraHandler { get; set; }
        public virtual Get<IDeviceCursor> CursorGetter { get; set; }

        protected ICamera Camera {
            get { return CameraHandler ();}
        }
        
        IDeviceCursor _deviceCursor = null;
        public virtual IDeviceCursor DeviceCursor {
            get {
                if (_deviceCursor == null) {
                    _deviceCursor = this.CursorGetter();
                }
                return _deviceCursor;
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
        //
        protected bool resizing = false;
        protected bool moving = false;


        public virtual Anchor HitAnchor(Point p) {
            IShape shape = this.Shape;
            if (shape == null)
                return Anchor.None;
            Point sp = Camera.ToSource(p);
            Anchor result = shape.IsAnchorHit(sp, HitSize);
            return result;
        }


        public abstract bool HitTest(Point p);

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
                DeviceCursor.SaveCursor();
                moving = HitTest(e.Location);
                if (hitAnchor != Anchor.None) {
                    Anchor anchor = AdjacentAnchor(hitAnchor);
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
            DeviceCursor.RestoreCursor();
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
                throw new CheckFailedException(this.GetType(), typeof(IDeviceCursor));
            }
            return true;
        }

        #endregion
    }
}