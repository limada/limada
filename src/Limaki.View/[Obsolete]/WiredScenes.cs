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
using System;

namespace Limaki.View.Visuals.UI {
    /// <summary>
    /// encapsulates some operations on Scenes wich are linked to each other
    /// used to build Graph.DataChanged and Graph.GraphChanged events
    /// </summary>
    [Obsolete ("use Mesh instead")]
    public class WiredScenes : SceneChanger {
        public WiredScenes (IGraph<IVisual, IVisualEdge> sourceGraph, IGraphScene<IVisual, IVisualEdge> target)
            : base(target) {
            this.SourceGraph = sourceGraph as IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge>;
        }

        public virtual IGraphPair<IVisual, IVisual, IVisualEdge, IVisualEdge> SourceGraph { get; protected set; }


        public virtual IVisual LookUp (IVisual sourceitem) {
            return GraphMapping.Mapping.LookUp<IVisual, IVisualEdge>(SourceGraph, TargetGraph, sourceitem);
        }

        public override void DataChanged (IVisual sourceItem) {
            var item = LookUp(sourceItem);
            if (item != null && !item.Data.Equals(sourceItem.Data)) {
                item.Data = sourceItem.Data;
                base.DataChanged(item);
            }
        }

        protected virtual void ChangeEdge (IVisualEdge sourceEdge, IVisualEdge targetEdge) {
            var root = LookUp(sourceEdge.Root);
            var leaf = LookUp(sourceEdge.Leaf);
            base.ChangeEdge(root, leaf, targetEdge);
        }

        public override void GraphChanged (IVisual sourceItem, GraphEventType eventType) {
            var item = LookUp(sourceItem);
            if (item != null) {
                if (eventType == GraphEventType.Update && item is IVisualEdge) {
                    ChangeEdge((IVisualEdge) sourceItem, (IVisualEdge) item);
                    return;
                } else {
                    base.GraphChanged(item, eventType);
                }
            }

        }
    }
}