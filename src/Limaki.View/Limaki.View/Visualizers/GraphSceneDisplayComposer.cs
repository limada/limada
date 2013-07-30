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


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Modelling;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Xwt;
using System;

namespace Limaki.View.Visualizers {

    public class GraphSceneDisplayComposer<TItem, TEdge> : DisplayComposer<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {

        public virtual Func<IGraphScene<TItem, TEdge>> GraphScene { get; set; }
        public virtual Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        public virtual Func<IModelReceiver<TItem>> ModelReceiver { get; set; }

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

            display.ModelReceiver = new GraphItemReceiver<TItem, TEdge>();
            
            display.GraphSceneReceiver = new GraphSceneReceiver<TItem, TEdge>();

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

            this.ModelReceiver = () => display.ModelReceiver;

            display.Layout.ShapeFactory = display.ShapeFactory;
            display.Layout.StyleSheet = display.StyleSheet;
            display.Layout.PainterFactory = display.PainterFactory;

            display.GraphSceneReceiver.GraphScene = this.GraphScene;
            display.GraphSceneReceiver.Layout = this.Layout;
            display.GraphSceneReceiver.Camera = this.Camera;
            display.GraphSceneReceiver.Clipper = this.Clipper;
            display.GraphSceneReceiver.ModelReceiver = this.ModelReceiver;
            display.EventControler.Add(display.GraphSceneReceiver);

            var renderer = display.DataRenderer as IGraphSceneRenderer<TItem, TEdge>;
            renderer.ItemRenderer = display.GraphItemRenderer;
            renderer.Layout = this.Layout;
            renderer.Camera = this.Camera;

            var selector = new GraphSceneFocusAction<TItem, TEdge> {
                SceneHandler = this.GraphScene,
                CameraHandler = this.Camera,
            };
            display.EventControler.Add(selector);
            display.EventControler.Add(new DragDropCatcher<GraphSceneFocusAction<TItem, TEdge>>(selector, display.Backend));

            Compose(display, display.SelectionRenderer);
            Compose(display, display.MoveResizeRenderer);

            var graphItemChanger = Compose(display,
                new GraphItemMoveResizeAction<TItem, TEdge> {
                    SceneHandler = this.GraphScene
                }, false);
            display.EventControler.Add(graphItemChanger);

            display.EventControler.Add(Compose(display,
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
            display.EventControler.Add(display.SelectAction);

            display.EventControler.Add(Compose(display,
                new GraphItemAddAction<TItem, TEdge> {
                    SceneHandler = this.GraphScene,
                    ModelFactory = display.ModelFactory,
                    Enabled = false
                }, false));

            display.EventControler.Add(
                new GraphItemDeleteAction<TItem, TEdge> {
                    SceneHandler = this.GraphScene,
                    MoveResizeRenderer = display.MoveResizeRenderer,
                });

            display.EventControler.Add(
                new DelegatingMouseAction {
                    MouseDown = e => display.OnSceneFocusChanged()
                });

            var oldZoomAction = display.EventControler.GetAction<ZoomAction> ();
            display.EventControler.Remove(oldZoomAction);

            display.EventControler.Add(
                new GraphItemZoomAction<TItem, TEdge> {
                    Viewport = this.Viewport,
                    SceneHandler = this.GraphScene
                });

            display.LayoutChanged ();
        }



    }
}