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

using System;
using System.Linq;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.Mesh;
using Limaki.View.Layout;
using Limaki.View.UI.GraphScene;
using Limaki.View.Visualizers;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Visuals.UI {

    public class VisualGraphSceneEvents : IGraphSceneEvents<IVisual, IVisualEdge> {

        public void GraphDataChanged (IVisual sourceItem, IVisual sinkItem, IGraphScene<IVisual, IVisualEdge> sinkScene, IGraphSceneDisplay<IVisual, IVisualEdge> sinkDisplay) {
            sinkItem.Data = sourceItem.Data;
            sinkScene.Requests.Add (new LayoutCommand<IVisual> (sinkItem, LayoutActionType.Perform));
            if (sinkDisplay != null)
                sinkDisplay.Perform ();
        }

        public void GraphChanged (
            IGraph<IVisual, IVisualEdge> sourceGraph, IVisual sourceItem, 
            IVisual sinkItem, IGraphScene<IVisual, IVisualEdge> sinkScene,
            IGraphSceneDisplay<IVisual, IVisualEdge> sinkDisplay, GraphEventType eventType) {

            if (eventType == GraphEventType.Update && sinkItem is IVisualEdge) {
                VisualGraphUpdateChangedEdge (sourceGraph, sourceItem as IVisualEdge, sinkScene, (IVisualEdge) sinkItem);
            } else if (eventType == GraphEventType.Remove) {
                VisualGraphItemRemove (sinkScene, sinkItem);
            } else if (eventType == GraphEventType.Add && sinkItem is IVisualEdge) {
                VisualGraphEdgeAdd (sinkScene, (IVisualEdge) sinkItem);
            }
            if (sinkDisplay != null)
                sinkDisplay.Perform ();
        }

        protected virtual void VisualGraphEdgeAdd (IGraphScene<IVisual, IVisualEdge> sinkScene, IVisualEdge sinkEdge) {
            if (sinkScene.Contains (sinkEdge.Root) && (sinkScene.Contains (sinkEdge.Leaf))) {
                sinkScene.Graph.Add (sinkEdge);
                if (sinkEdge.Shape == null) {
                    sinkScene.Requests.Add (new LayoutCommand<IVisual> (sinkEdge, LayoutActionType.Invoke));
                }
                sinkScene.Requests.Add (new LayoutCommand<IVisual> (sinkEdge, LayoutActionType.Justify));
            }
        }

        protected virtual void VisualGraphItemRemove (IGraphScene<IVisual, IVisualEdge> sinkScene, IVisual sinkItem) {
            var sinkGraph = sinkScene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;
            if (sinkScene.Contains (sinkItem)) {
                if (sinkScene.Focused == sinkItem) {
                    sinkScene.Focused = null;
                }
                sinkScene.Selected.Remove (sinkItem);

                sinkScene.Requests.Add (new RemoveBoundsCommand<IVisual, IVisualEdge> (sinkItem, sinkScene));
                sinkGraph.Remove (sinkItem);
            } else {
                // remove invisible items:
                if (sinkGraph.Source.Contains (sinkItem)) {
                    sinkGraph.Source.Remove (sinkItem);
                }
            }
        }

        protected virtual IVisual LookUp (IGraph<IVisual, IVisualEdge> sourceGraph, IGraph<IVisual, IVisualEdge> sinkGraph, IVisual sourceItem) {
            return GraphMapping.Mapping.LookUp<IVisual, IVisualEdge> (sourceGraph, sinkGraph, sourceItem);
        }

        protected virtual void VisualGraphUpdateChangedEdge (IGraph<IVisual, IVisualEdge> sourceGraph, IVisualEdge sourceEdge, IGraphScene<IVisual, IVisualEdge> sinkScene, IVisualEdge sinkEdge) {
            var sinkGraph = sinkScene.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;
            var root = LookUp (sourceGraph, sinkGraph, sourceEdge.Root);
            var leaf = LookUp (sourceGraph, sinkGraph, sourceEdge.Leaf);

            if (sinkEdge.Root != root || sinkEdge.Leaf != leaf) {

                bool isEdgeVisible = sinkGraph.Sink.Contains (sinkEdge);
                bool makeEdgeVisible = sinkGraph.Sink.Contains (root) && sinkGraph.Sink.Contains (leaf);

                Action<IVisual, bool> changeEdge = (item, isRoot) => {
                    if (makeEdgeVisible)
                        sinkGraph.ChangeEdge (sinkEdge, item, isRoot);
                    else
                        sinkGraph.Source.ChangeEdge (sinkEdge, item, isRoot);
                };

                if (sinkEdge.Root != root) changeEdge (root, true);
                if (sinkEdge.Leaf != leaf) changeEdge (leaf, false);


                if (makeEdgeVisible) {
                    var changeList = new IVisualEdge[] { sinkEdge }.Union (sinkGraph.Source.Twig (sinkEdge));
                    foreach (var edge in changeList) {
                        var showTwig = (sinkScene.Contains (edge.Root) && sinkScene.Contains (edge.Leaf));

                        var doAdd = (edge == sinkEdge && !isEdgeVisible) ||
                                    (!sinkGraph.Sink.Contains (edge) && showTwig);

                        if (doAdd) {
                            sinkGraph.Sink.Add (edge);
                            if (edge.Shape == null) {
                                sinkScene.Requests.Add (new LayoutCommand<IVisual> (edge, LayoutActionType.Invoke));
                            } else {
                                edge.Size = Size.Zero;
                            }
                        }
                        if (showTwig || edge == sinkEdge)
                            sinkScene.Requests.Add (new LayoutCommand<IVisual> (edge, LayoutActionType.Justify));
                    }
                } else {
                    var changeList = new IVisualEdge[] { sinkEdge }.Union (sinkGraph.Sink.Twig (sinkEdge));
                    foreach (var edge in changeList) {
                        sinkGraph.Sink.Remove (edge);
                        sinkScene.Requests.Add (new RemoveBoundsCommand<IVisual, IVisualEdge> (edge, sinkScene));
                    }
                }
            }
        }

    }
}