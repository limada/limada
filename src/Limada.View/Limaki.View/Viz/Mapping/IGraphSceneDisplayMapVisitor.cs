/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Graphs;
using Limaki.View;

namespace Limaki.View.Viz.Mapping {

    public interface IGraphSceneDisplayMapVisitor<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem {

        void ChangeDataVisit (
            Action<TSinkItem, TSinkItem, IGraphScene<TSinkItem, TSinkEdge>, IGraphSceneDisplay<TSinkItem, TSinkEdge>> visit);

        void GraphChangedVisit (
            Action<
                object,
                GraphChangeArgs<TSinkItem, TSinkEdge>,
                TSinkItem, IGraphScene<TSinkItem, TSinkEdge>,
                IGraphSceneDisplay<TSinkItem, TSinkEdge>
                >
                visit, object sender, GraphChangeArgs<TSinkItem, TSinkEdge> args);
    }
}