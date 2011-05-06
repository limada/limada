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
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Presenter.Widgets.UI;
using Limaki.Widgets;


namespace Limaki.Presenter.Widgets {
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


        public IGraph<IWidget, IEdgeWidget> CreateTargetGraph(IGraph<IWidget, IEdgeWidget> source) {
            IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget> sourceGraph =
                source as GraphView<IWidget, IEdgeWidget>;
            if (sourceGraph != null) {
                sourceGraph = GraphPairExtension<IWidget, IEdgeWidget>.Source(sourceGraph);

                var data = sourceGraph.Two;
                var result = GraphMapping.Mapping.CloneGraphPair<IWidget, IEdgeWidget>(data);

                if (result != null) {
                    return new GraphView<IWidget, IEdgeWidget>(result, new WidgetGraph());
                }
            }
            return null;
        }

        public Scene CreateTargetScene(IGraphScene<IWidget, IEdgeWidget> source) {
            var result = new Scene ();
            var targetGraph = CreateTargetGraph(source.Graph);
            if (targetGraph != null)
                result.Graph = targetGraph;
            
            return result;
        }

        public virtual void WireScene(WidgetDisplay targetDisplay, IGraphScene<IWidget, IEdgeWidget> sourceScene, IGraphScene<IWidget, IEdgeWidget> targetScene) {
            
            Action<IGraph<IWidget, IEdgeWidget>, IWidget, GraphChangeType> graphChanged =
               delegate(IGraph<IWidget, IEdgeWidget> sender, IWidget widget, GraphChangeType changeType) {
                   new WiredScenes(sourceScene, targetScene).GraphChanged(widget, changeType);
                   targetDisplay.Execute();
               };

            Action<IGraph<IWidget, IEdgeWidget>, IWidget> dataChanged =
                delegate(IGraph<IWidget, IEdgeWidget> sender, IWidget widget) {
                    new WiredScenes(sourceScene, targetScene).DataChanged(widget);
                    targetDisplay.Execute();
                };

            sourceScene.Graph.GraphChanged += graphChanged;
            sourceScene.Graph.DataChanged += dataChanged;

        }

    }
}
