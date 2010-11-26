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
 * 
 */

using System;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Widgets.UI {
    /// <summary>
    /// Overrides Zooming; if a widget is hit, no zooming is performed
    /// </summary>
    public class WidgetLayerZoomAction:ZoomAction {
        public WidgetLayerZoomAction():base() {}

        public WidgetLayerZoomAction(Func<Scene> sceneHandler, IZoomTarget zoomTarget, IScrollTarget scrollTarget, ICamera camera)
            : this() {
            this.zoomTarget = zoomTarget;
            this.camera = camera;
            this.scrollTarget = scrollTarget;
            this.SceneHandler = sceneHandler;
        }

        Func<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        private int _hitSize = 5;
        /// <summary>
        /// has to be the same as in WidgetResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        public override void OnMouseUp(MouseActionEventArgs e) {
            PointI p = camera.ToSource(e.Location);
            IWidget widget = Scene.Hit(p, HitSize);
            if (widget==null) {
                base.OnMouseUp (e);
            }
        }
    }
}