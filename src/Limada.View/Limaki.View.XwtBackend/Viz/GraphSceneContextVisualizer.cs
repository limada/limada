/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Rendering;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend.Viz {

    public class GraphSceneContextVisualizer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public IGraphScene<TItem, TEdge> Scene { get; set; }
        public IGraphSceneLayout<TItem, TEdge> Layout { get; set; }
        public IStyleSheet StyleSheet { get; set; }
        public GraphSceneContextPainter<TItem, TEdge> Painter { get; set; }

        public GraphSceneFacade<TItem, TEdge> Folder { get; set; }
        public IGraphSceneModeller<TItem, TEdge> Modeller { get; set; }

        public virtual void Compose (IGraphScene<TItem, TEdge> scene, IGraphItemRenderer<TItem, TEdge> itemRenderer) {

            this.Scene = scene;

            Func<IGraphScene<TItem, TEdge>> fScene = () => this.Scene;

            if (StyleSheet == null) {
                StyleSheet = new StyleSheets().Compose().DefaultStyleSheet;
                StyleSheet.BackColor = Colors.WhiteSmoke;
                // maybe created with wrong toolkit! Registry.Pooled<StyleSheets>().DefaultStyleSheet;
            }

            if (Layout == null) {
                Layout = Registry.Factory.Create<IGraphSceneLayout<TItem, TEdge>>(fScene, StyleSheet);
                Layout.Dimension = Limaki.Drawing.Dimension.X;
            }

            if (Painter == null)
                Painter = new GraphSceneContextPainter<TItem, TEdge>(Scene, Layout, itemRenderer);

            if (Modeller == null) {
                var modelReceiver = new GraphItemModeller<TItem, TEdge>();
                Modeller = new GraphSceneModeller<TItem, TEdge>();
                Modeller.GraphScene = fScene;
                Modeller.Layout = () => Layout;
                Modeller.Camera = () => Painter.Viewport.Camera;
                Modeller.Clipper = () => Painter.Clipper;
                Modeller.Modeller = () => modelReceiver;
            }

            if (Folder == null)
                Folder = new GraphSceneFacade<TItem, TEdge>(fScene, Layout);

        }
    }
}