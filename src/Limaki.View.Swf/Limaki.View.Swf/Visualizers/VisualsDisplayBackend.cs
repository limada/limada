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
 */

using Limaki.Drawing;
using Limaki.View.DragDrop;
using Limaki.View.Swf.Visuals;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.UI;
using Limaki.View.Visuals.Visualizers;
using Limaki.Visuals;
using System.Windows.Forms;

namespace Limaki.View.Swf.Visualizers {

    public class VisualsDisplayBackend : GraphSceneDisplayBackend<IVisual, IVisualEdge>, IVisualsDisplayBackend {

        public override DisplayFactory<IGraphScene<IVisual, IVisualEdge>> CreateDisplayFactory (DisplayBackend<IGraphScene<IVisual, IVisualEdge>> backend) {
           return new VisualsDisplayFactory {
                BackendComposer = new VisualsDisplayBackendComposer { Backend = backend },
                DisplayComposer = new SwfVisualsDisplayComposer()
            };
        }
    }

    public class SwfVisualsDisplayComposer:VisualsDisplayComposer {

        public override void Compose(Display<IGraphScene<IVisual, IVisualEdge>> aDisplay) {
            var display = aDisplay;
            base.Compose(display);

#if SwfDragDrop
                var dragDrop = new VisualsDragDrop(
                    this.GraphScene,
                    display.Backend as IDragDropControl,
                    this.Camera(),
                    this.Layout());
                dragDrop.Enabled = true;
                display.EventControler.Add(dragDrop);

                var selector = display.EventControler.GetAction<GraphSceneFocusAction<IVisual, IVisualEdge>>();
                if (selector != null) {
                    var catcher = new DragDropCatcher<GraphSceneFocusAction<IVisual, IVisualEdge>>(selector, display.Backend as IDragDropControl);
                    display.EventControler.Add(catcher);
                }

                var addEdgeAction = display.EventControler.GetAction<AddEdgeAction>();
                if (addEdgeAction != null) {
                    var catcher = new DragDropCatcher<AddEdgeAction>(addEdgeAction, display.Backend as IDragDropControl);
                    display.EventControler.Add(catcher);
                }
#else
                var dragDrop = new VisualsDragDropAction(
                    () => display.Data,
                    display.Backend,
                    display.Viewport.Camera,
                    ((IGraphSceneDisplay<IVisual, IVisualEdge>)display).Layout) {
                        Enabled = true
                    };

                display.EventControler.Add(dragDrop);
#endif            

            var editor = new VisualsTextEditor (
                this.GraphScene,
                display,
                this.Camera (),
                this.Layout()
                );

            display.EventControler.Add(editor);
        }
    }

    public class VisualsDisplayBackendComposer : GraphSceneDisplayBackendComposer<IVisual, IVisualEdge> {}
}