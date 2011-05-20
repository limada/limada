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
 */

//using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Common;

namespace Limaki.Presenter.UI {
    /// <summary>
    /// base class for mouse handling with dragging 
    /// </summary>
    public abstract class MouseDragActionBase : MouseActionBase {
        public MouseDragActionBase():base() {
            Priority = ActionPriorities.DragActionPriority;
        }

        private PointI _lastMousePos = new PointI();
        protected virtual PointI LastMousePos {
            get { return _lastMousePos; }
            set { _lastMousePos = value; }
        }

		private ModifierKeys _modifierKeys = 0;
        public ModifierKeys ModifierKeys {
            get { return _modifierKeys; }
            set { _modifierKeys = value; }
        }

        static IUISystemInformation _drawingUtils = null;
        protected static IUISystemInformation systemInformation {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.Create<IUISystemInformation>();
                }
                return _drawingUtils;
            }
        }

        protected RectangleI dragBoxFromMouseDown = RectangleI.Empty;

        protected void BaseMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);

            //if (Form.ModifierKeys != _modifierKeys) {
            //    return;
            //}

            // The DragSize indicates the size that the mouse can move 
            // before a drag event should be started.                
            SizeI dragSize = systemInformation.DragSize;
               

            // Create a Rectangle using the DragSize, with the mouse position being
            // at the center of the Rectangle.
            dragBoxFromMouseDown = new RectangleI(new PointI(e.X - (dragSize.Width / 2),
                                                           e.Y - (dragSize.Height / 2)), dragSize);

            // Remember the point where the mouse down occurred
            LastMousePos = e.Location;          
        }

        /// <summary>
        /// sets the initial mousePos and the dragging rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(MouseActionEventArgs e) {
            BaseMouseDown (e);

        }

        protected virtual void BaseMouseMove(MouseActionEventArgs e) {
            //if ((Form.ModifierKeys & _modifierKeys) != _modifierKeys) {
            //    return;
            //}
            if (!Resolved) {
                if (((e.Button & MouseActionButtons.Left) == MouseActionButtons.Left))
                    // If the mouse moves outside the Rectangle, start the drag
                    Resolved = ( dragBoxFromMouseDown != RectangleI.Empty &&
                                 !dragBoxFromMouseDown.Contains(e.X, e.Y) );

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
            BaseMouseMove (e);
        }

        protected override void EndAction() {
            base.EndAction();
            dragBoxFromMouseDown = RectangleI.Empty;            
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            base.OnMouseUp (e);
        }
   }
}