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
 * 
 */


using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.UI;
using Limaki.Visuals;

namespace Limaki.View.Visuals.Visualizers {
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
        public virtual void MakeSideDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> sourceDisplay, IGraphSceneDisplay<IVisual, IVisualEdge> targetDisplay) {
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
        public virtual void CopyDisplayProperties(IGraphSceneDisplay<IVisual, IVisualEdge> sourceDisplay, IGraphSceneDisplay<IVisual, IVisualEdge> targetDisplay) {
            targetDisplay.BackColor = sourceDisplay.BackColor;
            targetDisplay.ZoomState = sourceDisplay.ZoomState;
            targetDisplay.SelectAction.Enabled = sourceDisplay.SelectAction.Enabled;
            targetDisplay.MouseScrollAction.Enabled = sourceDisplay.MouseScrollAction.Enabled;
        }


        public IGraph<IVisual, IVisualEdge> CreateTargetGraph(IGraph<IVisual, IVisualEdge> source) {
            IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> sourceGraph =
                source as GraphView<IVisual, IVisualEdge>;
            if (sourceGraph != null) {
                sourceGraph = sourceGraph.RootSource();

                var data = sourceGraph.Two;
                var result = GraphMapping.Mapping.CloneGraphPair<IVisual, IVisualEdge>(data);

                if (result != null) {
                    return new GraphView<IVisual, IVisualEdge>(result, new VisualGraph());
                }
            }
            return null;
        }

        public IGraphScene<IVisual, IVisualEdge> CreateTargetScene (IGraphScene<IVisual, IVisualEdge> source) {
            var result = new Scene ();
            var targetGraph = CreateTargetGraph(source.Graph);
            if (targetGraph != null)
                result.Graph = targetGraph;
            
            return result;
        }

        public virtual void WireScene(IGraphSceneDisplay<IVisual, IVisualEdge> targetDisplay, IGraphScene<IVisual, IVisualEdge> sourceScene, IGraphScene<IVisual, IVisualEdge> targetScene) {
            
            Action<IGraph<IVisual, IVisualEdge>, IVisual, GraphChangeType> graphChanged =
               delegate(IGraph<IVisual, IVisualEdge> sender, IVisual visual, GraphChangeType changeType) {
                   new WiredScenes(sourceScene, targetScene).GraphChanged(visual, changeType);
                   targetDisplay.Execute();
               };

            Action<IGraph<IVisual, IVisualEdge>, IVisual> dataChanged =
                delegate(IGraph<IVisual, IVisualEdge> sender, IVisual visual) {
                    new WiredScenes(sourceScene, targetScene).DataChanged(visual);
                    targetDisplay.Execute();
                };

            sourceScene.Graph.GraphChanged += graphChanged;
            sourceScene.Graph.DataChanged += dataChanged;

        }

    }
}
