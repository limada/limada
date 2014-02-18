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
    [Obsolete ("use Mesh instead")]
    public class WiredDisplays {
        /// <summary>
        /// makes a new Scene in sourceDisplay
        /// with a SubGraph which Source is same as targetDisplay.Graph(View).Source
        /// copies Properites
        /// crosslinks the sourceDisplay and targetDisplay graphs with each other
        /// </summary>
        /// <param name="sourceDisplay"></param>
        /// <param name="targetDisplay"></param>
        public virtual void MakeSideDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> sourceDisplay, IGraphSceneDisplay<IVisual, IVisualEdge> targetDisplay) {
            CopyDisplayProperties(sourceDisplay, targetDisplay);

            targetDisplay.Data = CreateTargetScene(sourceDisplay.Data.Graph);

            WireScene(targetDisplay, sourceDisplay.Data.Graph, targetDisplay.Data);
            WireScene(sourceDisplay, targetDisplay.Data.Graph, sourceDisplay.Data);
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
                source as SubGraph<IVisual, IVisualEdge>;
            if (sourceGraph != null) {
                sourceGraph = sourceGraph.RootSource();

                var so = sourceGraph.Source;
                var result = GraphMapping.Mapping.CloneGraphPair<IVisual, IVisualEdge>(so);

                if (result != null) {
                    return new SubGraph<IVisual, IVisualEdge>(result, new VisualGraph());
                }
            }
            return null;
        }

        public IGraphScene<IVisual, IVisualEdge> CreateTargetScene (IGraph<IVisual, IVisualEdge> sourceGraph) {
            var result = new Scene ();
            var targetGraph = CreateTargetGraph(sourceGraph);
            if (targetGraph != null)
                result.Graph = targetGraph;
            
            return result;
        }

        public virtual void WireScene(IGraphSceneDisplay<IVisual, IVisualEdge> targetDisplay, IGraph<IVisual, IVisualEdge> sourceGraph, IGraphScene<IVisual, IVisualEdge> targetScene) {

            Action<IGraph<IVisual, IVisualEdge>, IVisual, GraphEventType> graphChanged = (sender, visual, changeType) => {
                   new WiredScenes(sourceGraph, targetScene).GraphChanged(visual, changeType);
                   targetDisplay.Perform();
               };

            Action<IGraph<IVisual, IVisualEdge>, IVisual> dataChanged = (sender, visual) => {
                    new WiredScenes(sourceGraph, targetScene).DataChanged(visual);
                    targetDisplay.Perform();
                };

            sourceGraph.GraphChanged += graphChanged;
            sourceGraph.DataChanged += dataChanged;

        }

    }
}
