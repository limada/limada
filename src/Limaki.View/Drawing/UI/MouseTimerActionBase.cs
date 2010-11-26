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
 */

using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Drawing.UI {
    /// <summary>
    /// base class for mouse action
    /// activated if mouse pressed for a certain amount of time
    /// </summary>
    public abstract class MouseTimerActionBase : MouseActionBase {
        public MouseTimerActionBase()
            : base() {
            Priority = ActionPriorities.DragActionPriority;
        }
        private PointI _lastMousePos = new PointI();
        protected virtual PointI lastMousePos {
            get { return _lastMousePos; }
            set { _lastMousePos = value; }
        }

        private int _lastMouseTime = 0;
        public virtual int LastMouseTime {
            get { return _lastMouseTime; }
            set { _lastMouseTime = value; }
        }

        static IUISystemInformation _drawingUtils = null;
        protected static IUISystemInformation systemInformation {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.One<IUISystemInformation>();
                }
                return _drawingUtils;
            }
        }

        protected RectangleI dragBoxFromMouseDown = RectangleI.Empty;

        protected void BaseMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);

            // The DragSize indicates the size that the mouse can move 
            // before a drag event should be started.                
            SizeI dragSize = systemInformation.DragSize;

            // Create a Rectangle using the DragSize, with the mouse position being
            // at the center of the Rectangle.
            dragBoxFromMouseDown = new RectangleI(new PointI(e.X - (dragSize.Width / 2),
                                                           e.Y - (dragSize.Height / 2)), dragSize);

            // Remember the point where the mouse down occurred
            _lastMousePos.X = e.X;
            _lastMousePos.Y = e.Y;

            _lastMouseTime = Environment.TickCount;
        }

        /// <summary>
        /// sets the initial mousePos and the dragging rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(MouseActionEventArgs e) {
            BaseMouseDown(e);
        }

        protected void BaseMouseMove(MouseActionEventArgs e) {
            if (!Resolved) {
                Resolved = ((e.Button & MouseActionButtons.Left) == MouseActionButtons.Left);
            } else {
                Resolved = ((e.Button & MouseActionButtons.Left) == MouseActionButtons.Left);
            }
        }

        /// <summary>
        /// sets resolved = true if the mouse is outside of the dragging rectangle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(MouseActionEventArgs e) {
            BaseMouseMove(e);
        }


        protected virtual void BaseMouseUp(MouseActionEventArgs e) {
            if (!Resolved && _lastMouseTime != 0) {

                int dragTime = systemInformation.DoubleClickTime;
                int now = Environment.TickCount;
                // If the mouse NOT moves outside the Rectangle
                Resolved = !(dragBoxFromMouseDown != RectangleI.Empty &&
                              !dragBoxFromMouseDown.Contains(e.X, e.Y))
                    // if more than 
                           && ((now - _lastMouseTime) > dragTime);
                
                // this is for debugging:
                if (Resolved) {
                    System.Diagnostics.Trace.WriteLine("Start/Elapsed MouseTimer:\t" + _lastMouseTime + "/" + (now - _lastMouseTime));
                    dragTime = 0;
                    now = 0;
                }

            }
            dragBoxFromMouseDown = RectangleI.Empty;
            _lastMouseTime = 0;

        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            BaseMouseUp(e);
        }
    }
}