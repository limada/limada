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

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Xwt;

namespace Limaki.Presenter.UI {
    /// <summary>
    /// Zooms in or out with left or right mouse click
    /// </summary>
    public class ZoomAction:MouseActionBase, ICheckable {
        public ZoomAction(): base() {
            Priority = ActionPriorities.ZoomActionPriority;
        }

        public virtual Get<IViewport> Viewport { get; set; }
        
        
        // ZoomState In 
        public virtual void ZoomIn() {
            var zoomTarget = Viewport ();
            var z = zoomTarget.ZoomFactor * 1.5d;
            zoomTarget.ZoomState = ZoomState.Custom;
            if (z <= 10) {
                zoomTarget.ZoomFactor = z;
            }
        }

        // ZoomState Out 
        public virtual void ZoomOut() {
            var zoomTarget = Viewport();
            var z = zoomTarget.ZoomFactor / 1.5d;
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
                var zoomTarget = Viewport();
                var camera = zoomTarget.Camera;

                // get the mouse position as source coordinates
                var mousePosSource = camera.ToSource(e.Location);

                if (e.Button == MouseActionButtons.Left)
                    ZoomIn();
                else
                    ZoomOut();

                zoomTarget.UpdateCamera();
                // get the transformed mouse position as transformed coordinates
                var mousePosTransformed = camera.FromSource(mousePosSource);

                var clipOrigin = zoomTarget.ClipOrigin;
                zoomTarget.ClipOrigin = 
                    new Point(
                        mousePosTransformed.X - e.X + clipOrigin.X,
                        mousePosTransformed.Y - e.Y + clipOrigin.Y);

                zoomTarget.UpdateZoom();

            }
            Resolved = doZoomInOut;
        }

        public virtual bool Check() {
            if (this.Viewport == null) {
                throw new CheckFailedException(this.GetType(),typeof(IViewport));
            }
            return true;
        }

    }
}