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
using Limaki.Drawing;
using Limaki.Common;


namespace Limaki.Presenter.UI {
    /// <summary>
    /// Scrolls with left mouse down according to mouse move
    /// </summary>
    public class MouseScrollAction:MouseDragActionBase, ICheckable {

        public virtual Get<IViewport> Viewport { get; set; }

        public MouseScrollAction(): base() {
            Priority = ActionPriorities.ScrollActionPriority;
            ModifierKeys = ModifierKeys.Control;
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
                    PointI scrollPosition = scrollTarget.ClipOrigin;

                    int dx = e.X - LastMousePos.X;
                    int dy = e.Y - LastMousePos.Y;
                    scrollPosition = new PointI (
                        Math.Max (scrollPosition.X - dx, 0),
                        Math.Max (scrollPosition.Y - dy, 0)
                        );
                    this.LastMousePos = new PointI (e.X, e.Y);
                    scrollTarget.ClipOrigin = scrollPosition;
                    scrollTarget.Update ();
                }
            }
        }
        public override void OnMouseUp(MouseActionEventArgs e) {
            base.OnMouseUp(e);
            Resolved = false;
        }


        #region ICheckable Member

        public bool Check() {
            if (this.Viewport == null) {
                throw new CheckFailedException(this.GetType(), typeof(IViewport));
            }
            return true;
        }

        #endregion
    }
}