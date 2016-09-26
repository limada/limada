using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Visuals;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.Modelling;
using Xwt;

namespace Limaki.View.Viz.Visuals {
    public class VisualGraphSceneMeshEvents0 : IGraphSceneDisplayEvents<IVisual, IVisualEdge> {
        public void GraphChanged (
            object sender,
            GraphChangeArgs<IVisual, IVisualEdge> args,
            IVisual sinkItem, IGraphScene<IVisual, IVisualEdge> sinkScene,
            IGraphSceneDisplay<IVisual, IVisualEdge> sinkDisplay) {
            // TODO: remove the rest:
            var sourceGraph = args.Graph;
            var sourceItem = args.Item;
            var eventType = args.EventType;

            if (eventType == GraphEventType.Update && sinkItem is IVisualEdge) {
                VisualGraphUpdateChangedEdge (sourceGraph, sourceItem as IVisualEdge, sinkScene, (IVisualEdge) sinkItem);
            } else if (eventType == GraphEventType.Update) {
                sinkItem.Data = sourceItem.Data;
                sinkScene.Requests.Add (new LayoutCommand<IVisual> (sinkItem, LayoutActionType.Perform));
            } else if (eventType == GraphEventType.Remove) {
                VisualGraphItemRemove (sinkScene, sinkItem);
            } else if (eventType == GraphEventType.Add && sinkItem is IVisualEdge) {
                VisualGraphEdgeAdd (sinkScene, (IVisualEdge) sinkItem);
            }
            if (sinkDisplay != null)
                sinkDisplay.Perform ();
        }

        [Obsolete]
        protected virtual void VisualGraphEdgeAdd (IGraphScene<IVisual, IVisualEdge> sinkScene, IVisualEdge sinkEdge) {
            if (sinkScene.Contains (sinkEdge.Root) && (sinkScene.Contains (sinkEdge.Leaf))) {
                sinkScene.Graph.Add (sinkEdge);
                if (sinkEdge.Shape == null) {
                    sinkScene.Requests.Add (new LayoutCommand<IVisual> (sinkEdge, LayoutActionType.Invoke));
                }
                sinkScene.Requests.Add (new LayoutCommand<IVisual> (sinkEdge, LayoutActionType.Justify));
            }
        }
        [Obsolete]
        protected virtual void VisualGraphItemRemove (IGraphScene<IVisual, IVisualEdge> sinkScene, IVisual sinkItem) {
            
            if (sinkScene.Contains (sinkItem)) {
                if (sinkScene.Focused == sinkItem) {
                    sinkScene.Focused = null;
                }
                sinkScene.Selected.Remove (sinkItem);

                sinkScene.Requests.Add (new RemoveBoundsCommand<IVisual, IVisualEdge> (sinkItem, sinkScene));
            }

            var graphs = new Stack<IGraph<IVisual, IVisualEdge>> ();
            graphs.Push (sinkScene.Graph);
            while (graphs.Count > 0) {
                var graph = graphs.Pop ();
                var sinkGraph = graph as ISinkGraph<IVisual, IVisualEdge>;
                if (graph.Contains (sinkItem)) {
                    if (sinkGraph != null)
                        sinkGraph.RemoveSinkItem (sinkItem);
                    else
                        graph.Remove (sinkItem);
                }
                var graphPair = graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;
                if (graphPair != null)
                    graphs.Push (graphPair.Source);
            }

        }
        [Obsolete]
        protected virtual IVisual LookUp (IGraph<IVisual, IVisualEdge> sourceGraph, IGraph<IVisual, IVisualEdge> sinkGraph, IVisual sourceItem) {
            return GraphMapping.Mapping.LookUp<IVisual, IVisualEdge> (sourceGraph, sinkGraph, sourceItem);
        }
        [Obsolete]
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