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

using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Limaki.Visuals.GraphScene;
using Xwt.Backends;

namespace Limaki.Viewers.ToolStripViewers {

    [BackendType(typeof(IMarkerToolStripBackend))]
    public class MarkerToolStrip : ToolStripViewer<IGraphSceneDisplay<IVisual, IVisualEdge>, IMarkerToolStripBackend> {

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

    public interface IMarkerToolStripBackend : IToolStripViewerBackend {
        void Attach (IGraphScene<IVisual, IVisualEdge> scene);
        void Detach (IGraphScene<IVisual, IVisualEdge> oldScene);
    }
}