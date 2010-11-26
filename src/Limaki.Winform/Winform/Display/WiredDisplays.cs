/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Drawing.UI;
using Limaki.Widgets;

namespace Limaki.Winform.Displays {
    /// <summary>
    /// Methods to make Displays aware of each other
    /// </summary>
    public class WiredDisplays {
        /// <summary>
        /// makes a new Scene in sourceDisplay
        /// with a GraphView which Data is same as targetDisplay.Graph(View).Data
        /// copies Properites
        /// crosslinks the sourceDisplay and targetDisplay graphs with each other
        /// </summary>
        /// <param name="sourceDisplay"></param>
        /// <param name="targetDisplay"></param>
        public virtual void MakeSideDisplay(WidgetDisplay sourceDisplay, WidgetDisplay targetDisplay) {
            CopyDisplayProperties(sourceDisplay, targetDisplay);

            targetDisplay.Data = CreateTargetScene(sourceDisplay.Data);

            WireScene(targetDisplay, sourceDisplay.Data, targetDisplay.Data);
            WireScene(sourceDisplay, targetDisplay.Data, sourceDisplay.Data);
        }

        /// <summary>
        /// copies some propterties of sourceDisplay into targetDisplay
        /// BackColor, ZoomState
        /// sets Action.Enabled of SelectAction and ScrollAction
        /// </summary>
        /// <param name="sourceDisplay"></param>
        /// <param name="targetDisplay"></param>
        public virtual void CopyDisplayProperties(WidgetDisplay sourceDisplay, WidgetDisplay targetDisplay) {
            targetDisplay.BackColor = sourceDisplay.BackColor;
            targetDisplay.ZoomState = sourceDisplay.ZoomState;
            targetDisplay.SelectAction.Enabled = sourceDisplay.SelectAction.Enabled;
            targetDisplay.ScrollAction.Enabled = sourceDisplay.ScrollAction.Enabled;
        }


        public Scene CreateTargetScene(Scene sourceScene) {
            IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget> sourceGraph =
                sourceScene.Graph as GraphView<IWidget, IEdgeWidget>;

            Scene targetScene = new Scene ();

            if (sourceGraph != null) {
                sourceGraph = new GraphPairFacade<IWidget, IEdgeWidget> ().Source (sourceGraph);

                var data = sourceGraph.Two;
                var targetGraph =
                    GraphMapping.Mapping.CloneGraphPair<IWidget, IEdgeWidget>(data);

                if (targetGraph != null) {
                    var targetView = 
                        new GraphView<IWidget, IEdgeWidget>(targetGraph, new WidgetGraph());
                    targetScene.Graph = targetView;
                }
            }
            return targetScene;
        }

        public virtual void WireScene(IControl targetDisplay, Scene sourceScene, Scene targetScene) {
            
            Action<IGraph<IWidget, IEdgeWidget>, IWidget, GraphChangeType> graphChanged =
               delegate(IGraph<IWidget, IEdgeWidget> sender, IWidget widget, GraphChangeType changeType) {
                   new WiredScenes(sourceScene, targetScene).GraphChanged(widget, changeType);
                   targetDisplay.CommandsExecute();
               };

            Action<IGraph<IWidget, IEdgeWidget>, IWidget> dataChanged =
                delegate(IGraph<IWidget, IEdgeWidget> sender, IWidget widget) {
                    new WiredScenes(sourceScene, targetScene).DataChanged(widget);
                    targetDisplay.CommandsExecute();
                };

            sourceScene.Graph.GraphChanged += graphChanged;
            sourceScene.Graph.DataChanged += dataChanged;

        }

    }
}
