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


using System;
using Limaki.Graphs;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.UI.GraphScene;
using Xwt;
using Limaki.Actions;

namespace Limaki.View.Viz.Visualizers {

    public class GraphSceneDisplayComposer<TItem, TEdge> : DisplayComposer<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {

        public virtual Func<IGraphScene<TItem, TEdge>> GraphScene { get; set; }
        public virtual Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        public virtual Func<ICommandModeller<TItem>> ModelReceiver { get; set; }

        public override void Dispose() {
            base.Dispose();
            GraphScene = null;
            Layout = null;
            ModelReceiver = null;
        }

        public override void Factor(Display<IGraphScene<TItem, TEdge>> aDisplay) {
            var display = aDisplay as GraphSceneDisplay<TItem, TEdge>;
            
            base.Factor(display);

            this.GraphScene = () => display.Data;

            display.CommandModeller = new GraphItemModeller<TItem, TEdge>();
            
            display.GraphSceneModeller = new GraphSceneModeller<TItem, TEdge>();

            display.DataRenderer = new GraphSceneRenderer<TItem, TEdge>();
            
        }

        public override void Compose(Display<IGraphScene<TItem, TEdge>> aDisplay) {
            var display = aDisplay as GraphSceneDisplay<TItem, TEdge>;
            
            this.Layout = () => display.Layout;
            this.DataOrigin = () => {
                if (display.Data != null && display.Data.Shape != null) {
                    var result =  display.Data.Shape.Location;
                    var border = display.Layout.Border;
                    if (result.X < 0) {
                        result.X -= border.Width;
                    }
                    if (result.Y < 0) {
                        result.Y -= border.Height;
                    }
                    return result;
                } else
                    return Point.Zero;
            };

            this.DataSize = () => {
                if (display.Data != null && display.Data.Shape != null) {
                    var result = display.Data.Shape.Size;
                    var border = display.Layout.Border;
                    result += border;
                    var offset = display.Data.Shape.Location;
                    if (offset.X < 0) {
                        result.Width += border.Width;
                    }
                    if (offset.Y < 0) {
                        result.Height += border.Height;
                    }
                    return result;
                } else
                    return Size.Zero;
            };


            base.Compose(display);

            this.ModelReceiver = () => display.CommandModeller;

            display.Layout.ShapeFactory = display.ShapeFactory;
            display.Layout.StyleSheet = display.StyleSheet;
            display.Layout.PainterFactory = display.PainterFactory;

            display.GraphSceneModeller.GraphScene = this.GraphScene;
            display.GraphSceneModeller.Layout = this.Layout;
            display.GraphSceneModeller.Camera = this.Camera;
            display.GraphSceneModeller.Clipper = this.Clipper;
            display.GraphSceneModeller.Modeller = this.ModelReceiver;
            display.ActionDispatcher.Add(display.GraphSceneModeller);

            var renderer = display.DataRenderer as IGraphSceneRenderer<TItem, TEdge>;
            renderer.ItemRenderer = display.GraphItemRenderer;
            renderer.Layout = this.Layout;
            renderer.Camera = this.Camera;

            var selector = new GraphSceneFocusAction<TItem, TEdge> {
                SceneHandler = this.GraphScene,
                CameraHandler = this.Camera,
            };
            display.ActionDispatcher.Add(selector);
            display.ActionDispatcher.Add(new DragDropCatcher<GraphSceneFocusAction<TItem, TEdge>>(selector, display.Backend));

            Compose(display, display.SelectionRenderer);
            Compose(display, display.MoveResizeRenderer);

            var graphItemChanger = Compose(display,
                new GraphItemMoveResizeAction<TItem, TEdge> {
                    SceneHandler = this.GraphScene
                }, false);
            display.ActionDispatcher.Add(graphItemChanger);

            display.ActionDispatcher.Add(Compose(display,
                new GraphEdgeChangeAction<TItem, TEdge> {
                    SceneHandler = this.GraphScene,
                    HitSize = graphItemChanger.HitSize + 1
                }, false));

            display.SelectAction = Compose(display,
                new GraphItemMultiSelector<TItem, TEdge> {
                    SceneHandler = this.GraphScene,
                    ShowGrips = false,
                    Enabled = true,
                }, true);
            display.ActionDispatcher.Add(display.SelectAction);

            display.ActionDispatcher.Add(Compose(display,
                new GraphItemAddAction<TItem, TEdge> {
                    SceneHandler = this.GraphScene,
                    ModelFactory = display.ModelFactory,
                    Enabled = false
                }, false));

            display.ActionDispatcher.Add(
                new GraphItemDeleteAction<TItem, TEdge> {
                    SceneHandler = this.GraphScene,
                    MoveResizeRenderer = display.MoveResizeRenderer,
                });

            display.ActionDispatcher.Add(
                new DelegatingMouseAction {
                    MouseDown = e => display.OnSceneFocusChanged()
                });

			IAction action = display.ActionDispatcher.GetAction<ZoomAction> ();
            display.ActionDispatcher.Remove(action);
			action = display.ActionDispatcher.GetAction<MouseScrollAction> ();
			display.ActionDispatcher.Remove(action);

            display.ActionDispatcher.Add(
                new GraphItemZoomAction<TItem, TEdge> {
                    Viewport = this.Viewport,
                    SceneHandler = this.GraphScene
                });

			action = new GraphItemMouseScrollAction<TItem, TEdge> {
				Viewport = this.Viewport,
				SceneHandler = this.GraphScene
			};
			action.Enabled = false;

			display.ActionDispatcher.Add(action);
            display.LayoutChanged ();
        }



    }
}