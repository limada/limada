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
 * http://limada.sourceforge.net
 */


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Modelling;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Limaki.View.UI.GraphScene;
using Xwt;

namespace Limaki.View.Display {
    public class GraphSceneDisplayComposer<TItem, TEdge> : DisplayComposer<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {

        public virtual Get<IGraphScene<TItem, TEdge>> GraphScene { get; set; }
        public virtual Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }
        public virtual Get<IModelReceiver<TItem>> ModelReceiver { get; set; }

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
            
            display.SceneReceiver = new GraphSceneReceiver<TItem, TEdge>();

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

            display.SceneReceiver.GraphScene = this.GraphScene;
            display.SceneReceiver.Layout = this.Layout;
            display.SceneReceiver.Camera = this.Camera;
            display.SceneReceiver.Clipper = this.Clipper;
            display.SceneReceiver.ModelReceiver = this.ModelReceiver;
            display.EventControler.Add(display.SceneReceiver);

            var renderer = display.DataRenderer as IGraphSceneRenderer<TItem, TEdge>;
            renderer.ItemRenderer = display.GraphItemRenderer;
            renderer.Layout = this.Layout;
            renderer.Camera = this.Camera;

            var graphItemSelector = new GraphSceneFocusAction<TItem, TEdge> ();
            graphItemSelector.SceneHandler = this.GraphScene;
            graphItemSelector.CameraHandler = this.Camera;
            display.EventControler.Add(graphItemSelector);

            Compose(display, display.SelectionRenderer);
            Compose(display, display.MoveResizeRenderer);

            var graphItemChanger = new GraphItemMoveResizeAction<TItem,TEdge>();
            Compose(display, graphItemChanger, false);
            graphItemChanger.SceneHandler = this.GraphScene;
            display.EventControler.Add (graphItemChanger);

            var selectAction = new GraphItemMultiSelector<TItem,TEdge>();
            Compose (display, selectAction,true);
            selectAction.SceneHandler = this.GraphScene;
            selectAction.ShowGrips = false;
            selectAction.Enabled = true;

            display.SelectAction = selectAction;
            display.EventControler.Add(selectAction);
            
            var graphEdgeChangeAction = new GraphEdgeChangeAction<TItem,TEdge>();
            Compose(display, graphEdgeChangeAction,false);
            graphEdgeChangeAction.SceneHandler = this.GraphScene;
            graphEdgeChangeAction.HitSize = graphItemChanger.HitSize + 1;
            display.EventControler.Add (graphEdgeChangeAction);

            var addGraphItemAction = new GraphItemAddAction<TItem, TEdge>();
            Compose(display, addGraphItemAction,false);
            addGraphItemAction.SceneHandler = this.GraphScene;
            addGraphItemAction.ModelFactory = display.ModelFactory;
            addGraphItemAction.Enabled = false;
            display.EventControler.Add(addGraphItemAction);

            var deleter = new GraphItemDeleteAction<TItem, TEdge>();
            deleter.SceneHandler = this.GraphScene;
            deleter.MoveResizeRenderer = display.MoveResizeRenderer;
            display.EventControler.Add(deleter);


            var oldZoomAction = display.EventControler.GetAction<ZoomAction> ();
            display.EventControler.Remove(oldZoomAction);

            var zoomAction = new GraphItemZoomAction<TItem, TEdge> ();
            zoomAction.Viewport = this.Viewport;
            zoomAction.SceneHandler = this.GraphScene;
            display.EventControler.Add(zoomAction);

            
            display.LayoutChanged ();
        }



    }
}