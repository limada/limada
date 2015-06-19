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

using Limaki.Common;
using Limaki.Graphs;
using Limaki.View.Visuals;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.UI.GraphScene;
using Limaki.View.Viz.Visualizers;
using Xwt.Backends;

namespace Limaki.View.Viz.Visuals {

    [BackendType(typeof(IVisualsDisplayBackend))]
    public class VisualsDisplay : GraphSceneDisplay<IVisual, IVisualEdge> { }

    public interface IVisualsDisplayBackend : IVidgetBackend { }

    public class VisualsDisplayFactory : GraphSceneDisplayFactory<IVisual, IVisualEdge> {

        public override Display<IGraphScene<IVisual, IVisualEdge>> Create () { return new VisualsDisplay(); }

        public override IComposer<Display<IGraphScene<IVisual, IVisualEdge>>> DisplayComposer {
            get { return _displayComposer ?? (_displayComposer = new VisualsDisplayComposer()); }
            set { base.DisplayComposer = value; }
        }
    }

    public class VisualsDisplayComposer : GraphSceneDisplayComposer<IVisual, IVisualEdge> {

        public override void Factor (Display<IGraphScene<IVisual, IVisualEdge>> aDisplay) {
            var display = aDisplay as GraphSceneDisplay<IVisual, IVisualEdge>;

            base.Factor(display);

            display.ModelFactory = Registry.Factory.Create<IGraphModelFactory<IVisual, IVisualEdge>>();
            display.GraphItemRenderer = new VisualsRenderer();

            display.Layout = Registry.Factory.Create<IGraphSceneLayout<IVisual, IVisualEdge>>(this.GraphScene, display.StyleSheet);
        }

        protected IMouseAction AddGraphEdgeAction { get; set; }

        public override void Compose (Display<IGraphScene<IVisual, IVisualEdge>> aDisplay) {
            var display = aDisplay as GraphSceneDisplay<IVisual, IVisualEdge>;

            base.Compose(display);
            var folding = new GraphSceneFolding<IVisual, IVisualEdge> {
                SceneHandler = this.GraphScene,
                Layout = this.Layout,
                BackendRenderer = display.BackendRenderer,
                MoveResizeRenderer = display.MoveResizeRenderer,
                OrderBy = new VisualComparer(),
                RemoveOrphans = false
            };

            display.ActionDispatcher.Add (new GraphSceneKeyFolding<IVisual, IVisualEdge> (folding));
            display.ActionDispatcher.Add (new GraphSceneMouseFolding<IVisual, IVisualEdge> (folding, () => display.Viewport.Camera));

            var addGraphEdgeAction = new AddVisualEdgeAction {
                SceneHandler = this.GraphScene,
                LayoutHandler = this.Layout,
                Enabled = false
            };

            Compose(display, addGraphEdgeAction, false);

            display.ActionDispatcher.Add(addGraphEdgeAction);
            display.ActionDispatcher.Add(new DragDropCatcher<AddVisualEdgeAction>(addGraphEdgeAction, display.Backend));

           

        }
    }
}