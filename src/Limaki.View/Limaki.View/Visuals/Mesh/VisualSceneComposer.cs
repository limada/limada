/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.Visualizers;
using Limaki.Visuals;

namespace Limaki.View.Visuals.Visualizers {

    public class MeshSceneComposer {
        /// <summary>
        /// copies some propterties of sourceDisplay into targetDisplay
        /// they will have the same SourceGraph
        /// BackColor, ZoomState
        /// sets Action.Enabled of SelectAction and ScrollAction
        /// </summary>
        /// <param name="sourceDisplay"></param>
        /// <param name="targetDisplay"></param>
        public virtual void CopyDisplayProperties (IGraphSceneDisplay<IVisual, IVisualEdge> sourceDisplay, IGraphSceneDisplay<IVisual, IVisualEdge> targetDisplay) {
            targetDisplay.BackColor = sourceDisplay.BackColor;
            targetDisplay.ZoomState = sourceDisplay.ZoomState;
            targetDisplay.SelectAction.Enabled = sourceDisplay.SelectAction.Enabled;
            targetDisplay.MouseScrollAction.Enabled = sourceDisplay.MouseScrollAction.Enabled;
        }

        public IGraphScene<IVisual, IVisualEdge> CreateTargetScene (IGraph<IVisual, IVisualEdge> sourceGraph) {
            var result = new Scene();
            var targetGraph = CreateTargetGraph(sourceGraph);
            if (targetGraph != null)
                result.Graph = targetGraph;

            return result;
        }

        public IGraph<IVisual, IVisualEdge> CreateTargetGraph (IGraph<IVisual, IVisualEdge> source) {
            IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> sourceGraph =
                source as SubGraph<IVisual, IVisualEdge>;

            if (sourceGraph != null) {
                sourceGraph = sourceGraph.RootSource();

                var result = GraphMapping.Mapping.CloneGraphPair<IVisual, IVisualEdge>(sourceGraph.Source);

                if (result != null) {
                    // souround with a view
                    return new SubGraph<IVisual, IVisualEdge>(result, new VisualGraph());
                }
            }
            return null;
        }
    }
}