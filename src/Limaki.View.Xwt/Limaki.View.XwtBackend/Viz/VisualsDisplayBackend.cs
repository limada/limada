/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend;
using Limaki.View.DragDrop;
using Limaki.View.XwtBackend.Viz;

namespace Limaki.View.XwtBackend {

    public class VisualsDisplayBackend : GraphSceneDisplayBackend<IVisual, IVisualEdge>, IVisualsDisplayBackend {

        public override DisplayFactory<IGraphScene<IVisual, IVisualEdge>> CreateDisplayFactory (DisplayBackend<IGraphScene<IVisual, IVisualEdge>> backend) {
            return new VisualsDisplayFactory {
                BackendComposer = new VisualsDisplayBackendComposer { Backend = backend },
                DisplayComposer = new XwtVisualsDisplayComposer()
            };
        }
    }


    public class XwtVisualsDisplayComposer : VisualsDisplayComposer {

        public override void Compose (Display<IGraphScene<IVisual, IVisualEdge>> display) {
            base.Compose(display);

            var dragDrop = new VisualsDragDropAction(
                () => display.Data,
                display.Backend,
                display.Viewport.Camera,
                ((IGraphSceneDisplay<IVisual, IVisualEdge>) display).Layout) {
                    Enabled = true
                };

            display.ActionDispatcher.Add(dragDrop);

            var editor = new VisualsTextEditAction(
                () => display.Data,
                display,
                this.Camera(),
                ((IGraphSceneDisplay<IVisual, IVisualEdge>)display).Layout
                );

            display.ActionDispatcher.Add(editor);
        }
    }

    public class VisualsDisplayBackendComposer : GraphSceneDisplayBackendComposer<IVisual, IVisualEdge> { }

}