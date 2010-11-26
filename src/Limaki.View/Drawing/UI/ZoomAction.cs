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

using Limaki.Actions;

namespace Limaki.Drawing.UI {
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

        public override void OnMouseMove(MouseActionEventArgs e) {
            Resolved = false;
        }

        /// <summary>
        /// Zooms in with left click
        /// Zooms out with right click 
        /// if posible remains on same mouse position 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnMouseUp(MouseActionEventArgs e) {
            bool doZoomInOut =
                ((e.Button == MouseActionButtons.Left) || 
                 (e.Button == MouseActionButtons.Right))
                && ( e.Modifiers== ModifierKeys.Control);
            if (doZoomInOut) {
                // get the mouse position as source coordinates
                PointI mousePosSource = camera.ToSource(e.Location);

                if (e.Button == MouseActionButtons.Left)
                    ZoomIn();
                else
                    ZoomOut();

                scrollTarget.UpdateCamera();
                // get the transformed mouse position as transformed coordinates
                PointI mousePosTransformed = camera.FromSource(mousePosSource);

                scrollTarget.ScrollPosition =
                    new PointI(
                        mousePosTransformed.X - e.X + scrollTarget.ScrollPosition.X,
                        mousePosTransformed.Y - e.Y + scrollTarget.ScrollPosition.Y);

                zoomTarget.UpdateZoom();

            }
            Resolved = doZoomInOut;
        }


    }
}