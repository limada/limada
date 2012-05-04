/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.View.Display;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;

namespace Limaki.Viewers.ToolStripViewers {
    public class MarkerToolController : ToolController<IGraphSceneDisplay<IVisual, IVisualEdge>, IMarkerTool> {
        public override void Attach(object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                this.CurrentDisplay = display;
                Tool.Attach(display.Data);
            }
        }

        public override void Detach(object sender) {
            this.CurrentDisplay = null;
        }
        public virtual void ChangeMarkers(string marker) {
            var display = CurrentDisplay;
            if (display != null) {
                var scene = display.Data;
                if (scene.Markers != null) {
                    SceneExtensions.ChangeMarkers(scene, scene.Selected.Elements, marker);
                }
                display.Execute();
            }
        }
    }
}