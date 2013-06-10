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

using System;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;
using Limaki.View.Visuals.Rendering;
using Xwt.Backends;

namespace Limaki.View.Visuals.Visualizers {

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

            IGraphSceneFolding<IVisual, IVisualEdge> folding = new GraphSceneFolding<IVisual, IVisualEdge>();
            folding.SceneHandler = this.GraphScene;
            folding.Layout = this.Layout;
            folding.DeviceRenderer = display.BackendRenderer;
            folding.MoveResizeRenderer = display.MoveResizeRenderer;
            folding.OrderBy = new VisualComparer();
            display.EventControler.Add(folding);

            var addGraphEdgeAction = new AddEdgeAction();
            addGraphEdgeAction.SceneHandler = this.GraphScene;
            Compose(display, addGraphEdgeAction, false);
            addGraphEdgeAction.LayoutHandler = this.Layout;
            addGraphEdgeAction.Enabled = false;
            display.EventControler.Add(addGraphEdgeAction);

            var focusCatcher = new FocusCatcher();
            focusCatcher.OnSceneFocusChanged = display.OnSceneFocusChanged;
            display.EventControler.Add(focusCatcher);

        }
    }

    public class FocusCatcher : MouseActionBase, ICheckable {
        public FocusCatcher ()
            : base() {
            this.Priority = ActionPriorities.SelectionPriority;
        }

        public Action OnSceneFocusChanged { get; set; }
        public override void OnMouseDown (MouseActionEventArgs e) {
            base.OnMouseDown(e);
            OnSceneFocusChanged();
        }

        public override void OnMouseMove (MouseActionEventArgs e) { }

        public bool Check () {
            if (this.OnSceneFocusChanged == null) {
                throw new CheckFailedException(this.GetType(), typeof(Action));
            }
            return true;
        }


    }
}