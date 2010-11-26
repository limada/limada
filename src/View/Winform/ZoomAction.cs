/*
 * Limaki 
 * Version 0.071
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
using Limaki.Drawing;

namespace Limaki.Winform {
    /// <summary>
    /// Zooms in or out with left or right mouse click
    /// </summary>
    public class ZoomAction:MouseActionBase {
        public ZoomAction(): base() {
            Priority = ActionPriorities.ZoomActionPriority;
        }
		///<directed>True</directed>
		protected IZoomTarget zoomTarget = null;
		///<directed>True</directed>
        protected IScrollTarget scrollTarget = null;
		///<directed>True</directed>
        protected ICamera camera = null;
        public ZoomAction(IZoomTarget zoomTarget, IScrollTarget scrollTarget, ICamera camera):this() {
            this.zoomTarget = zoomTarget;
            this.camera = camera;
            this.scrollTarget = scrollTarget;
        }

        // ZoomState In 
        public virtual void ZoomIn() {
            float z = zoomTarget.ZoomFactor * 1.5f;
            zoomTarget.ZoomState = ZoomState.Custom;
            if (z <= 10) {
                zoomTarget.ZoomFactor = z;
            }
        }

        // ZoomState Out 
        public virtual void ZoomOut() {
            float z = zoomTarget.ZoomFactor / 1.5f;
            zoomTarget.ZoomState = ZoomState.Custom;
            if (z >= 0.05) {
                zoomTarget.ZoomFactor = z;
            }
        }

        public override void OnMouseMove(MouseEventArgs e) {
            Resolved = false;
        }

        /// <summary>
        /// Zooms in with left click
        /// Zooms out with right click 
        /// if posible remains on same mouse position 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(MouseEventArgs e) {
            bool doZoomInOut = (e.Button == MouseButtons.Left) || (e.Button == MouseButtons.Right);
            if (doZoomInOut) {
                // get the mouse position as source coordinates
                Point mousePosSource = camera.ToSource(e.Location);

                if (e.Button == MouseButtons.Left)
                    ZoomIn();
                else
                    ZoomOut();

                zoomTarget.UpdateZoom();

                // get the transformed mouse position as transformed coordinates
                Point mousePosTransformed = camera.FromSource(mousePosSource);

                scrollTarget.ScrollPosition =
                    new Point(
                        mousePosTransformed.X - e.X + scrollTarget.ScrollPosition.X,
                        mousePosTransformed.Y - e.Y + scrollTarget.ScrollPosition.Y);

            }
            Resolved = doZoomInOut;
        }


    }
}