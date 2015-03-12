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
 */

using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.View.Vidgets;
using Xwt;
using System.Diagnostics;

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// base class for mouse action
    /// activated if mouse pressed for a certain amount of time
    /// </summary>
    public abstract class MouseTimerActionBase : MouseActionBase {
        public MouseTimerActionBase()
            : base() {
            Priority = ActionPriorities.DragActionPriority;

        }

        protected virtual Point LastMousePos { get; set; }

        public virtual int LastMouseTime { get; set; }

        static IUISystemInformation _drawingUtils = null;
        protected static IUISystemInformation SystemInformation {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IUISystemInformation>();
                }
                return _drawingUtils;
            }
        }

        protected Rectangle DragBoxFromMouseDown { get; set; }

        protected void BaseMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);

            // The DragSize indicates the size that the mouse can move 
            // before a drag event should be started.                
            var dragSize = SystemInformation.DragSize;

            // Create a Rectangle using the DragSize, with the mouse position being
            // at the center of the Rectangle.
            DragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
                                                           e.Y - (dragSize.Height / 2)), dragSize);

            // Remember the point where the mouse down occurred
            LastMousePos = new Point (e.X, e.Y);

            LastMouseTime = Environment.TickCount;
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
            if (!Resolved && LastMouseTime != 0) {

                int now = Environment.TickCount;
                var dragTime = SystemInformation.DoubleClickTime;

                // If the mouse NOT moves outside the Rectangle
                Resolved = !(dragBoxFromMouseDown != Rectangle.Zero &&
                              !dragBoxFromMouseDown.Contains(e.X, e.Y))
                    // if more than 
                           && ((now - LastMouseTime) > dragTime);

                // this is for debugging:
                if (Resolved) {
                    Trace.WriteLine ("Start/Elapsed MouseTimer:\t" + LastMouseTime + "/" + (now - LastMouseTime));
                    dragTime = 0;
                    now = 0;
                }

            }
            DragBoxFromMouseDown = Rectangle.Zero;
            LastMouseTime = 0;

        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            BaseMouseUp(e);
        }
    }
}