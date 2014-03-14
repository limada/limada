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
using Limaki.View.Modelling;
using Limaki.View.Rendering;
using Limaki.View.UI.GraphScene;
using Limaki.View.XwtBackend;
using Limaki.View.XwtBackend.Visualizers;
using Xwt.Drawing;

namespace Limaki.View.Visualizers {

    public class GraphSceneContextVisualizer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public IGraphScene<TItem, TEdge> Scene { get; set; }
        public IGraphSceneLayout<TItem, TEdge> Layout { get; set; }
        public IStyleSheet StyleSheet { get; set; }
        public GraphSceneContextPainter<TItem, TEdge> Painter { get; set; }

        public GraphSceneFacade<TItem, TEdge> Folder { get; set; }
        public IGraphSceneReceiver<TItem, TEdge> Receiver { get; set; }

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

            if (Receiver == null) {
                var modelReceiver = new GraphItemReceiver<TItem, TEdge>();
                Receiver = new GraphSceneReceiver<TItem, TEdge>();
                Receiver.GraphScene = fScene;
                Receiver.Layout = () => Layout;
                Receiver.Camera = () => Painter.Viewport.Camera;
                Receiver.Clipper = () => Painter.Clipper;
                Receiver.ModelReceiver = () => modelReceiver;
            }

            if (Folder == null)
                Folder = new GraphSceneFacade<TItem, TEdge>(fScene, Layout);

        }
    }
}