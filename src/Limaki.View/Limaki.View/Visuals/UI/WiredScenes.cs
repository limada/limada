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


using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.View.Layout;
using Limaki.View.UI.GraphScene;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.Visuals.UI {
    /// <summary>
    /// encaspulates some operations on Scenes wich are linked to each other
    /// used to build Graph.DataChanged and Graph.GraphChanged events
    /// </summary>
    public class WiredScenes {
        public WiredScenes(IGraphScene<IVisual, IVisualEdge> source, IGraphScene<IVisual, IVisualEdge> target) {
            this.Source = source;
            this.Target = target;
        }

        
        public IGraphScene<IVisual, IVisualEdge> Source { get; protected set;}
        public IGraphScene<IVisual, IVisualEdge> Target { get; protected set; }

        public virtual IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> SourceGraph {
            get { return Source.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>; }
        }

        public virtual IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> TargetGraph {
            get { return Target.Graph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>; }
        }

        public virtual IVisual LookUp(IVisual sourceitem) {
            return GraphMapping.Mapping.LookUp<IVisual, IVisualEdge>(SourceGraph, TargetGraph, sourceitem);
        }

        public virtual void DataChanged(IVisual sourceItem) {
            IVisual item = LookUp(sourceItem);
            if (item != null && !item.Data.Equals(sourceItem.Data)) {
                item.Data = sourceItem.Data;
                Target.Requests.Add(new LayoutCommand<IVisual>(item, LayoutActionType.Perform));
            }
        }

        protected virtual void Remove(IVisual item) {
            ICollection<IVisualEdge> deleteQueue = 
                new List<IVisualEdge>(TargetGraph.Source.PostorderTwig(item));

            foreach (var delete in deleteQueue) {
                if (Target.Contains (delete)) {
                    // remark: it is not allowed to use DeleteCommand here as it calls
                    // OnDataChanged and 
                    Target.Requests.Add(new RemoveBoundsCommand<IVisual,IVisualEdge>(delete, Target));
                    Target.Remove (delete);
                } else {
                    TargetGraph.Source.Remove (delete);
                }
            }

            if (Target.Contains(item)) {
                if (Target.Focused == item) {
                    Target.Focused = null;
                }
                Target.Selected.Remove(item);

                Target.Requests.Add(new RemoveBoundsCommand<IVisual, IVisualEdge>(item, Target));
                TargetGraph.Remove(item);
            } else {
                TargetGraph.Source.Remove (item);
            }
        }

        protected virtual void Add(IVisualEdge edge) {
            if (Target.Contains(edge.Root) && (Target.Contains(edge.Leaf))) {
                Target.Graph.Add(edge);
                if (edge.Shape == null) {
                    Target.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Invoke));
                }
                Target.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
            }
        }

        protected virtual void ChangeEdge(IVisualEdge sourceEdge, IVisualEdge targetEdge) {
            IVisual root = LookUp(sourceEdge.Root);
            IVisual leaf = LookUp(sourceEdge.Leaf);
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

                        bool doAdd = ( edge == targetEdge && !isEdgeVisible ) ||
                                     ( !TargetGraph.Sink.Contains (edge) && showTwig );

                        if (doAdd) {
                            TargetGraph.Sink.Add(edge);
                            if (edge.Shape == null) {
                                Target.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Invoke));
                            } else {
                                edge.Size = Size.Zero;
                            }
                        }
                        if (showTwig || edge==targetEdge)
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

        public virtual void GraphChanged(IVisual sourceItem, GraphChangeType changeType) {
            IVisual item = LookUp(sourceItem);
            if (item != null) {
                if (changeType == GraphChangeType.Remove) {
                    Remove(item);
                    return;
                }
                if (changeType == GraphChangeType.Add && item is IVisualEdge) {
                    Add((IVisualEdge)item);
                    return;
                }
                if (changeType == GraphChangeType.Update && item is IVisualEdge) {
                    ChangeEdge((IVisualEdge)sourceItem, (IVisualEdge)item);
                    return;
                }
            }
        }
    }
}