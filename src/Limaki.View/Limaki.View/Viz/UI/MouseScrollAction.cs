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

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// Scrolls with left mouse down according to mouse move
    /// </summary>
    public class MouseScrollAction:MouseDragActionBase, ICheckable {

        public virtual Func<IViewport> Viewport { get; set; }

        public MouseScrollAction(): base() {
            Priority = ActionPriorities.ScrollActionPriority;
            this.ModifierKeys = ModifierKeys.Control;
        }


        public override void OnMouseDown(MouseActionEventArgs e) {
            base.OnMouseDown(e);
        }
        /// <summary>
        /// sets controls ScrollPosition according to mouse position 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(MouseActionEventArgs e) {
            base.OnMouseMove(e);
            if (Resolved) {
                var scrollTarget = this.Viewport ();
                if (scrollTarget != null) {
                    var oldScroll = scrollTarget.ClipOrigin;

                    var dx = e.X - LastMousePos.X;
                    var dy = e.Y - LastMousePos.Y;
                    var scrollPosition = new Point (
                        Math.Max (oldScroll.X - dx, 0),
                        Math.Max (oldScroll.Y - dy, 0)
                        );
                    this.LastMousePos = new Point (e.X, e.Y);
                    if (oldScroll != scrollPosition) {
                        scrollTarget.ClipOrigin = scrollPosition;
                        scrollTarget.Refresh ();
                    }
                }
            }
        }
        public override void OnMouseUp(MouseActionEventArgs e) {
            base.OnMouseUp(e);
            Resolved = false;
        }


        #region ICheckable Member

        public virtual bool Check() {
            if (this.Viewport == null) {
                throw new CheckFailedException(this.GetType(), typeof(IViewport));
            }
            return true;
        }

        #endregion
    }
}