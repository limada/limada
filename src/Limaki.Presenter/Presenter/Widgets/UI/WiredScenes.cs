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


using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Presenter.Layout;
using Limaki.Presenter.UI;
using Limaki.Widgets;

namespace Limaki.Presenter.Widgets.UI {
    /// <summary>
    /// encaspulates some operations on Scenes wich are linked to each other
    /// used to build Graph.DataChanged and Graph.GraphChanged events
    /// </summary>
    public class WiredScenes {
        public WiredScenes(IGraphScene<IWidget, IEdgeWidget> source, IGraphScene<IWidget, IEdgeWidget> target) {
            this.Source = source;
            this.Target = target;
        }

        
        public IGraphScene<IWidget, IEdgeWidget> Source { get; protected set;}
        public IGraphScene<IWidget, IEdgeWidget> Target { get; protected set; }

        public virtual IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget> SourceGraph {
            get { return Source.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>; }
        }

        public virtual IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget> TargetGraph {
            get { return Target.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>; }
        }

        public virtual IWidget LookUp(IWidget sourceitem) {
            return GraphMapping.Mapping.LookUp<IWidget, IEdgeWidget>(SourceGraph, TargetGraph, sourceitem);
        }

        public virtual void DataChanged(IWidget sourceItem) {
            IWidget item = LookUp(sourceItem);
            if (item != null && !item.Data.Equals(sourceItem.Data)) {
                item.Data = sourceItem.Data;
                Target.Requests.Add(new LayoutCommand<IWidget>(item, LayoutActionType.Perform));
            }
        }

        protected virtual void Remove(IWidget item) {
            ICollection<IEdgeWidget> deleteQueue = 
                new List<IEdgeWidget>(TargetGraph.Two.PostorderTwig(item));

            foreach (IWidget delete in deleteQueue) {
                if (Target.Contains (delete)) {
                    // remark: it is not allowed to use DeleteCommand here as it calls
                    // OnDataChanged and 
                    Target.Requests.Add(new RemoveBoundsCommand<IWidget,IEdgeWidget>(delete, Target));
                    Target.Remove (delete);
                } else {
                    TargetGraph.Two.Remove (delete);
                }
            }

            if (Target.Contains(item)) {
                if (Target.Focused == item) {
                    Target.Focused = null;
                }
                Target.Selected.Remove(item);

                Target.Requests.Add(new RemoveBoundsCommand<IWidget, IEdgeWidget>(item, Target));
                TargetGraph.Remove(item);
            } else {
                TargetGraph.Two.Remove (item);
            }
        }

        protected virtual void Add(IEdgeWidget edge) {
            if (Target.Contains(edge.Root) && (Target.Contains(edge.Leaf))) {
                Target.Graph.Add(edge);
                if (edge.Shape == null) {
                    Target.Requests.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Invoke));
                }
                Target.Requests.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
            }
        }

        protected virtual void ChangeEdge(IEdgeWidget sourceEdge, IEdgeWidget targetEdge) {
            IWidget root = LookUp(sourceEdge.Root);
            IWidget leaf = LookUp(sourceEdge.Leaf);
            if (targetEdge.Root != root || targetEdge.Leaf != leaf) {
                
                bool isEdgeVisible = TargetGraph.One.Contains(targetEdge);
                bool makeEdgeVisible = TargetGraph.One.Contains(root) && TargetGraph.One.Contains(leaf);

                if (targetEdge.Root != root) {
                    if (makeEdgeVisible)
                        TargetGraph.ChangeEdge(targetEdge, root, true);
                    else
                        TargetGraph.Two.ChangeEdge(targetEdge, root, true);
                }

                if (targetEdge.Leaf != leaf) {
                    if (makeEdgeVisible)
                        TargetGraph.ChangeEdge(targetEdge, leaf, false);
                    else
                        TargetGraph.Two.ChangeEdge(targetEdge, leaf, false);
                }


                if (makeEdgeVisible) {
                    List<IEdgeWidget> changeList = new List<IEdgeWidget>();
                    changeList.Add(targetEdge);
                    changeList.AddRange(TargetGraph.Two.Twig(targetEdge));

                    foreach (IEdgeWidget edge in changeList) {
                        bool showTwig = (Target.Contains(edge.Root) && Target.Contains(edge.Leaf));

                        bool doAdd = ( edge == targetEdge && !isEdgeVisible ) ||
                                     ( !TargetGraph.One.Contains (edge) && showTwig );

                        if (doAdd) {
                            TargetGraph.One.Add(edge);
                            if (edge.Shape == null) {
                                Target.Requests.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Invoke));
                            } else {
                                edge.Size = SizeI.Empty;
                            }
                        }
                        if (showTwig || edge==targetEdge)
                            Target.Requests.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
                    }
                } else {
                    ICollection<IEdgeWidget> changeList = new List<IEdgeWidget>(TargetGraph.One.Twig(targetEdge));
                    changeList.Add(targetEdge);
                    foreach (IEdgeWidget edge in changeList) {
                        TargetGraph.One.Remove(edge);
                        Target.Requests.Add(new RemoveBoundsCommand<IWidget, IEdgeWidget>(edge, Target));
                    }
                }
                //foreach (IEdgeWidget edge in target.Twig(targetEdge)) {
                //    if (makeEdgeVisible) {
                //        target.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
                //    } else {
                //        targetGraph.One.Remove(edge);
                //        target.Commands.Add(new RemoveBoundsCommand(edge, target));
                //    }
                //}
            }
        }

        public virtual void GraphChanged(IWidget sourceItem, GraphChangeType changeType) {
            IWidget item = LookUp(sourceItem);
            if (item != null) {
                if (changeType == GraphChangeType.Remove) {
                    Remove(item);
                    return;
                }
                if (changeType == GraphChangeType.Add && item is IEdgeWidget) {
                    Add((IEdgeWidget)item);
                    return;
                }
                if (changeType == GraphChangeType.Update && item is IEdgeWidget) {
                    ChangeEdge((IEdgeWidget)sourceItem, (IEdgeWidget)item);
                    return;
                }
            }
        }
    }
}