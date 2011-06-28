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
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Presenter.Visuals;
using Limaki.Presenter.Visuals.UI;
using Limaki.Visuals;

namespace Limaki.UseCases.Viewers.ToolStripViewers {
    public class MarkerToolController : ToolController<VisualsDisplay, IMarkerTool> {
        public override void Attach(object sender) {
            var display = sender as VisualsDisplay;
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
                    SceneTools.ChangeMarkers(scene, scene.Selected.Elements, marker);
                }
                display.Execute();
            }
        }
    }
}