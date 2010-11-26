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
    /// base class for mouse handling with dragging 
    /// </summary>
    public abstract class MouseDragActionBase : MouseActionBase {
        public MouseDragActionBase():base() {
            Priority = ActionPriorities.DragActionPriority;
        }
        private Point _lastMousePos = new Point();
        protected virtual Point LastMousePos {
            get { return _lastMousePos; }
            set { _lastMousePos = value; }
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
            LastMousePos = e.Location;          
        }

        /// <summary>
        /// sets the initial mousePos and the dragging rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(MouseEventArgs e) {
            BaseMouseDown (e);

        }
        // TODO: here is something wrong; resolved is always true if mousebutton == left
        protected void BaseMouseMoveold(MouseEventArgs e) {
            if (!Resolved) {
                Resolved = ( ( e.Button & MouseButtons.Left ) == MouseButtons.Left );
                if (Resolved)
                                 // If the mouse moves outside the Rectangle, start the drag
                    Resolved = ( dragBoxFromMouseDown != Rectangle.Empty && 
                                 !dragBoxFromMouseDown.Contains (e.X, e.Y) );

            } else {
                Resolved = ((e.Button & MouseButtons.Left) == MouseButtons.Left);
            }            
        }
        protected virtual void BaseMouseMove( MouseEventArgs e ) {
            if (!Resolved) {
                if (( ( e.Button & MouseButtons.Left ) == MouseButtons.Left ))
                    // If the mouse moves outside the Rectangle, start the drag
                    Resolved = ( dragBoxFromMouseDown != Rectangle.Empty &&
                                 !dragBoxFromMouseDown.Contains(e.X, e.Y) );

            } else {
                Resolved = ( ( e.Button & MouseButtons.Left ) == MouseButtons.Left );
            }   
        }
        /// <summary>
        /// sets resolved = true if the mouse is outside of the dragging rectangle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(MouseEventArgs e) {
            BaseMouseMove (e);
        }

        protected override void EndAction() {
            base.EndAction();
            dragBoxFromMouseDown = Rectangle.Empty;            
        }
        
        public override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp (e);
        }
   }
}