/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.View.Visuals;
using Xwt.Backends;

namespace Limaki.View.Viz.Visualizers.ToolStrips {

    [BackendType(typeof(IMarkerToolStripBackend))]
    public class MarkerToolStrip0 : DisplayToolStrip0<IGraphSceneDisplay<IVisual, IVisualEdge>, IMarkerToolStripBackend> {

        public override void Attach (object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                this.CurrentDisplay = display;
                Backend.Attach(display.Data);
            }
        }

        public override void Detach (object sender) {
            this.CurrentDisplay = null;
        }

        public virtual void ChangeMarkers (string marker) {
            var display = CurrentDisplay;
            if (display != null) {
                var scene = display.Data;
                if (scene.Markers != null) {
                    SceneExtensions.ChangeMarkers(scene, scene.Selected.Elements, marker);
                }
                display.Perform();
            }
        }
    }

    public interface IMarkerToolStripBackend : IDisplayToolStripBackend {
        void Attach (IGraphScene<IVisual, IVisualEdge> scene);
        void Detach (IGraphScene<IVisual, IVisualEdge> oldScene);
    }
}