/*
 * Limaki 
 * Version 0.063
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
using System.Windows.Forms;
using Limaki.Actions;

namespace Limaki.Winform {
    /// <summary>
    /// Scrolls with left mouse down according to mouse move
    /// </summary>
    public class ScrollAction:MouseDragActionBase {
        public ScrollAction(): base() {
            Priority = ActionPriorities.ScrollActionPriority;
        }

		///<directed>True</directed>
		IScrollTarget scrollTarget = null;
        public ScrollAction(IScrollTarget scrollTarget):this() {
            this.scrollTarget = scrollTarget;
        }
        public override void OnMouseDown( MouseEventArgs e ) {
            base.OnMouseDown(e);
        }
        /// <summary>
        /// sets controls ScrollPosition according to mouse position 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (Resolved) {
                if (scrollTarget != null) {
                    Point scrollPosition = scrollTarget.ScrollPosition;

                    int dx = e.X - LastMousePos.X;
                    int dy = e.Y - LastMousePos.Y;
                    scrollPosition = new Point (
                        Math.Max (scrollPosition.X - dx, 0),
                        Math.Max (scrollPosition.Y - dy, 0)
                        );
                    this.LastMousePos = new Point (e.X, e.Y);
                    scrollTarget.ScrollPosition = scrollPosition;
                }
            }
        }
        public override void OnMouseUp( MouseEventArgs e ) {
            base.OnMouseUp(e);
            Resolved = false;
        }

    }
}