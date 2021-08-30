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

//using System.Windows.Forms;

using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// base class for mouse handling with dragging 
    /// </summary>
    public abstract class MouseDragActionBase : MouseActionBase {

        public MouseDragActionBase():base() {
            Priority = ActionPriorities.DragActionPriority;
            Behaviour = DragBehaviour.Dragger;
		}

        public virtual Point LastMousePos { get; protected set; }
        protected virtual long LastMouseTime { get; set; }

        public enum DragBehaviour {
            Dragger,
            DoubleClick
        }

        /// <summary>
        /// mouse action should behave as DragAction
        /// </summary>
        public virtual DragBehaviour Behaviour { get; set; }

        public ModifierKeys ModifierKeys { get; set; }

        static IUISystemInformation _systemInformation = null;
        protected static IUISystemInformation SystemInformation {
            get {
                if (_systemInformation == null) {
                    _systemInformation = Registry.Factory.Create<IUISystemInformation>();
                }
                return _systemInformation;
            }
        }

        protected Rectangle DragBoxFromMouseDown { get; set; }
        protected int HitCount  { get; set; }

        Size dragSize = SystemInformation.DragSize;
        
        protected void BaseMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
        
            if (DragBoxFromMouseDown == Rectangle.Zero)
                DragBoxFromMouseDown = new Rectangle (new Point (e.X - (dragSize.Width / 2),
                    e.Y - (dragSize.Height / 2)), dragSize);

            LastMousePos = e.Location;

            if (Behaviour == DragBehaviour.DoubleClick) {
                var hit = CheckDoubleClickHit (e.X, e.Y);
                if (e.Clicks > 1 && hit)
                    HitCount++;
                else
                    HitCount = 0;
            }
            LastMouseTime = Environment.TickCount;
        }

        /// <summary>
        /// sets the initial mousePos and the dragging rectangle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseDown(MouseActionEventArgs e) {
            BaseMouseDown (e);

        }

        protected virtual bool CheckDoubleClickHit (double x, double y) {
            return DragBoxFromMouseDown.Contains (x, y);
        }

        /// <summary>
        /// if dragger: sets resolved = true if the mouse is outside of the dragging rectangle 
        /// if doubleclick: sets resolved = true if the mouse is outside of the dragging rectangle 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void BaseMouseMove(MouseActionEventArgs e) {
            if (Behaviour == DragBehaviour.Dragger) {
                if (!Resolved) {
                    if (((e.Button & MouseActionButtons.Left) == MouseActionButtons.Left))
                        // If the mouse moves outside the Rectangle, start the drag
                        Resolved = (DragBoxFromMouseDown != Rectangle.Zero &&
                                    !DragBoxFromMouseDown.Contains (e.X, e.Y));

                } else {
                    Resolved = ((e.Button & MouseActionButtons.Left) == MouseActionButtons.Left);
                }
            } else if (Behaviour == DragBehaviour.DoubleClick) {
                Resolved = Resolved && CheckDoubleClickHit (e.X, e.Y);
            }
        }

        public override void OnMouseMove (MouseActionEventArgs e) {
            BaseMouseMove (e);
        }

        public override void OnMouseUp (MouseActionEventArgs e) {
            base.OnMouseUp (e);
        }

        protected override void EndAction () {
            if (Behaviour == DragBehaviour.Dragger) {
                base.EndAction ();
                DragBoxFromMouseDown = Rectangle.Zero;
            } else if (Behaviour == DragBehaviour.DoubleClick) {
                if (Resolved)
                    DragBoxFromMouseDown = Rectangle.Zero;
                Resolved = false;
                if (AfterActionEnd != null)
                    AfterActionEnd ();
            }
        }

   }
}