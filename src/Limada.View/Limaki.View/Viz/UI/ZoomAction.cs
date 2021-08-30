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

using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Vidgets;
using Xwt;
using System;
using Limaki.View.Common;

namespace Limaki.View.Viz.UI {
    /// <summary>
    /// Zooms in or out with left or right mouse click
    /// </summary>
    public class ZoomAction:MouseActionBase, ICheckable {
        public ZoomAction(): base() {
            Priority = ActionPriorities.ZoomActionPriority;
        }

        public virtual Func<IViewport> Viewport { get; set; }
        
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

        public void Zoom(Point location, bool zoomIn) {
            var zoomTarget = Viewport();
            var camera = zoomTarget.Camera;

            // get the mouse position as source coordinates
            var mousePosSource = camera.ToSource(location);

            if (zoomIn)
                ZoomIn();
            else
                ZoomOut();

            zoomTarget.UpdateCamera();
            // get the transformed mouse position as transformed coordinates
            var mousePosTransformed = camera.FromSource(mousePosSource);

            var clipOrigin = zoomTarget.ClipOrigin;
            zoomTarget.ClipOrigin =
                new Point(
                    mousePosTransformed.X - location.X + clipOrigin.X,
                    mousePosTransformed.Y - location.Y + clipOrigin.Y);

            zoomTarget.UpdateZoom();
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
                Zoom(e.Location, e.Button == MouseActionButtons.Left);
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