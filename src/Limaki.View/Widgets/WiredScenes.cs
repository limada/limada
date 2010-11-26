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
using Limaki.Drawing.UI;

namespace Limaki.Widgets {
    /// <summary>
    /// encaspulates some operations on Scenes wich are linked to each other
    /// used to build Graph.DataChanged and Graph.GraphChanged events
    /// </summary>
    public class WiredScenes {
        public WiredScenes(Scene source, Scene target) {
            this.source = source;
            this.target = target;
        }

        private Scene _source = null;
        public Scene source {
            get { return _source; }
            set { _source = value; }
        }

        private Scene _target = null;
        public Scene target {
            get { return _target; }
            set { _target = value; }
        }

        IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget> sourceGraph {
            get { return source.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>; }
        }

        IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget> targetGraph {
            get { return target.Graph as IGraphPair<IWidget, IWidget, IEdgeWidget, IEdgeWidget>; }
        }

        IWidget lookUp(IWidget sourceitem) {
            return GraphMapping.Mapping.LookUp<IWidget, IEdgeWidget>(sourceGraph, targetGraph, sourceitem);
        }

        public virtual void DataChanged(IWidget sourceItem) {
            IWidget item = lookUp(sourceItem);
            if (item != null && !item.Data.Equals(sourceItem.Data)) {
                item.Data = sourceItem.Data;
                target.Commands.Add(new LayoutCommand<IWidget>(item, LayoutActionType.Perform));
            }
        }

        protected virtual void Remove(IWidget item) {
            ICollection<IEdgeWidget> deleteQueue = 
                new List<IEdgeWidget>(targetGraph.Two.PostorderTwig(item));

            foreach (IWidget delete in deleteQueue) {
                if (target.Contains (delete)) {
                    // remark: it is not allowed to use DeleteCommand here as it calls
                    // OnDataChanged and 
                    target.Commands.Add(new RemoveBoundsCommand(delete, target));
                    target.Remove (delete);
                } else {
                    targetGraph.Two.Remove (delete);
                }
            }

            if (target.Contains(item)) {
                if (target.Focused == item) {
                    target.Focused = null;
                }
                target.Selected.Remove(item);

                target.Commands.Add(new RemoveBoundsCommand(item, target));
                targetGraph.Remove(item);
            } else {
                targetGraph.Two.Remove (item);
            }
        }

        protected virtual void Add(IEdgeWidget edge) {
            if (target.Contains(edge.Root) && (target.Contains(edge.Leaf))) {
                target.Graph.Add(edge);
                if (edge.Shape == null) {
                    target.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Invoke));
                }
                target.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
            }
        }

        protected virtual void ChangeEdge(IEdgeWidget sourceEdge, IEdgeWidget targetEdge) {
            IWidget root = lookUp(sourceEdge.Root);
            IWidget leaf = lookUp(sourceEdge.Leaf);
            if (targetEdge.Root != root || targetEdge.Leaf != leaf) {
                
                bool isEdgeVisible = targetGraph.One.Contains(targetEdge);
                bool makeEdgeVisible = targetGraph.One.Contains(root) && targetGraph.One.Contains(leaf);

                if (targetEdge.Root != root) {
                    if (makeEdgeVisible)
                        targetGraph.ChangeEdge(targetEdge, root, true);
                    else
                        targetGraph.Two.ChangeEdge(targetEdge, root, true);
                }

                if (targetEdge.Leaf != leaf) {
                    if (makeEdgeVisible)
                        targetGraph.ChangeEdge(targetEdge, leaf, false);
                    else
                        targetGraph.Two.ChangeEdge(targetEdge, leaf, false);
                }


                if (makeEdgeVisible) {
                    List<IEdgeWidget> changeList = new List<IEdgeWidget>();
                    changeList.Add(targetEdge);
                    changeList.AddRange(targetGraph.Two.Twig(targetEdge));

                    foreach (IEdgeWidget edge in changeList) {
                        bool showTwig = (target.Contains(edge.Root) && target.Contains(edge.Leaf));

                        bool doAdd = ( edge == targetEdge && !isEdgeVisible ) ||
                                     ( !targetGraph.One.Contains (edge) && showTwig );

                        if (doAdd) {
                            targetGraph.One.Add(edge);
                            if (edge.Shape == null) {
                                target.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Invoke));
                            } else {
                                edge.Size = SizeI.Empty;
                            }
                        }
                        if (showTwig || edge==targetEdge)
                            target.Commands.Add(new LayoutCommand<IWidget>(edge, LayoutActionType.Justify));
                    }
                } else {
                    ICollection<IEdgeWidget> changeList = new List<IEdgeWidget>(targetGraph.One.Twig(targetEdge));
                    changeList.Add(targetEdge);
                    foreach (IEdgeWidget edge in changeList) {
                        targetGraph.One.Remove(edge);
                        target.Commands.Add(new RemoveBoundsCommand(edge, target));
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
            IWidget item = lookUp(sourceItem);
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