using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Layout;
using Limaki.View.UI.GraphScene;
using Limaki.Visuals;
using Xwt;
using System;

namespace Limaki.View.Visuals.UI {

    [Obsolete ("use Mesh instead")]
    public class SceneChanger {

        public SceneChanger (IGraphScene<IVisual, IVisualEdge> target) {
            this.Target = target;
        }

        public IGraphScene<IVisual, IVisualEdge> Target { get; protected set; }


        public virtual IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> TargetGraph {
            get { return Target.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>; }
        }

        public virtual void DataChanged (IVisual item) {
            Target.Requests.Add(new LayoutCommand<IVisual>(item, LayoutActionType.Perform));
        }

        protected virtual void Remove (IVisual item) {

            if (Target.Contains(item)) {
                if (Target.Focused == item) {
                    Target.Focused = null;
                }
                Target.Selected.Remove(item);

                Target.Requests.Add(new RemoveBoundsCommand<IVisual, IVisualEdge>(item, Target));
                TargetGraph.Remove(item);
            } else {
                if (TargetGraph.Source.Contains(item)) {
                    //var sinkGraph = TargetGraph.Source as ISinkGraph<IVisual, IVisualEdge>;
                    //if (sinkGraph != null)
                    //    sinkGraph.RemoveInSink(item);
                    //else
                        TargetGraph.Source.Remove(item);
                }
            }
        }

        protected virtual void Add (IVisualEdge edge) {
            if (Target.Contains(edge.Root) && (Target.Contains(edge.Leaf))) {
                Target.Graph.Add(edge);
                if (edge.Shape == null) {
                    Target.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Invoke));
                }
                Target.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
            }
        }

        protected virtual void ChangeEdge (IVisual root, IVisual leaf, IVisualEdge targetEdge) {

            if (targetEdge.Root != root || targetEdge.Leaf != leaf) {

                bool isEdgeVisible = TargetGraph.Sink.Contains(targetEdge);
                bool makeEdgeVisible = TargetGraph.Sink.Contains(root) && TargetGraph.Sink.Contains(leaf);

                if (targetEdge.Root != root) {
                    if (makeEdgeVisible)
                        TargetGraph.ChangeEdge(targetEdge, root, true);
                    else
                        TargetGraph.Source.ChangeEdge(targetEdge, root, true);
                }

                if (targetEdge.Leaf != leaf) {
                    if (makeEdgeVisible)
                        TargetGraph.ChangeEdge(targetEdge, leaf, false);
                    else
                        TargetGraph.Source.ChangeEdge(targetEdge, leaf, false);
                }


                if (makeEdgeVisible) {
                    List<IVisualEdge> changeList = new List<IVisualEdge>();
                    changeList.Add(targetEdge);
                    changeList.AddRange(TargetGraph.Source.Twig(targetEdge));

                    foreach (IVisualEdge edge in changeList) {
                        bool showTwig = (Target.Contains(edge.Root) && Target.Contains(edge.Leaf));

                        bool doAdd = (edge == targetEdge && !isEdgeVisible) ||
                                     (!TargetGraph.Sink.Contains(edge) && showTwig);

                        if (doAdd) {
                            TargetGraph.Sink.Add(edge);
                            if (edge.Shape == null) {
                                Target.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Invoke));
                            } else {
                                edge.Size = Size.Zero;
                            }
                        }
                        if (showTwig || edge == targetEdge)
                            Target.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
                    }
                } else {
                    ICollection<IVisualEdge> changeList = new List<IVisualEdge>(TargetGraph.Sink.Twig(targetEdge));
                    changeList.Add(targetEdge);
                    foreach (IVisualEdge edge in changeList) {
                        TargetGraph.Sink.Remove(edge);
                        Target.Requests.Add(new RemoveBoundsCommand<IVisual, IVisualEdge>(edge, Target));
                    }
                }
                //foreach (var edge in target.Twig(targetEdge)) {
                //    if (makeEdgeVisible) {
                //        target.Commands.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
                //    } else {
                //        targetGraph.Sink.Remove(edge);
                //        target.Commands.Add(new RemoveBoundsCommand(edge, target));
                //    }
                //}
            }
        }

        public virtual void GraphChanged (IVisual item, GraphEventType eventType) {
            if (item != null) {
                if (eventType == GraphEventType.Remove) {
                    Remove(item);
                    return;
                }
                if (eventType == GraphEventType.Add && item is IVisualEdge) {
                    Add((IVisualEdge) item);
                    return;
                }

            }
        }
    }
}