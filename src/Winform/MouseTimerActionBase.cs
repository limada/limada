/*
 * Limaki 
 * Version 0.064
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
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using Limaki.Actions;

namespace Limaki.Winform {
    /// <summary>
    /// base class for mouse action
    /// activated if mouse pressed for a certain amount of time
    /// </summary>
    public abstract class MouseTimerActionBase : MouseActionBase {
        public MouseTimerActionBase()
            : base() {
            Priority = ActionPriorities.DragActionPriority;
        }
        private Point _lastMousePos = new Point();
        protected virtual Point lastMousePos {
            get { return _lastMousePos; }
            set { _lastMousePos = value; }
        }

        private int _lastMouseTime = 0;
        protected virtual int lastMouseTime {
            get { return _lastMouseTime; }
            set { _lastMouseTime = value; }
        }

        protected Rectangle dragBoxFromMouseDown = Rectangle.Empty;

        protected void BaseMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);

            // The DragSize indicates the size that the mouse can move 
            // before a drag event should be started.                
            Size dragSize = SystemInformation.DragSize;

            // Create a Rectangle using the DragSize, with the mouse position being
            // at the center of the Rectangle.
            dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2),
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
        public override void OnMouseDown(MouseEventArgs e) {
            BaseMouseDown(e);
        }

        protected void BaseMouseMove(MouseEventArgs e) {
            if (!Resolved) {
                Resolved = ((e.Button & MouseButtons.Left) == MouseButtons.Left);
            } else {
                Resolved = ((e.Button & MouseButtons.Left) == MouseButtons.Left);
            }
        }

        /// <summary>
        /// sets resolved = true if the mouse is outside of the dragging rectangle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(MouseEventArgs e) {
            BaseMouseMove(e);
        }


        protected virtual void BaseMouseUp(MouseEventArgs e) {
            if (!Resolved) {

                int dragTime = SystemInformation.DoubleClickTime;
                int now = Environment.TickCount;
                // If the mouse NOT moves outside the Rectangle
                Resolved = !(dragBoxFromMouseDown != Rectangle.Empty &&
                              !dragBoxFromMouseDown.Contains(e.X, e.Y))
                    // if more than 
                           && ((now - _lastMouseTime) > dragTime);
                // this is for debugging:
                if (Resolved) {
                    dragTime = 0;
                    now = 0;
                }

            }
            dragBoxFromMouseDown = Rectangle.Empty;
            _lastMouseTime = Environment.TickCount;

        }

        public override void OnMouseUp(MouseEventArgs e) {
            BaseMouseUp(e);
        }
    }
}