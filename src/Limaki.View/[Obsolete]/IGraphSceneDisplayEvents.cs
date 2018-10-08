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

namespace Limaki.View.Viz.Mapping {

    /// <summary>
    /// events handling the changes of the 
    /// sink in a meshed GraphScene
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    [Obsolete]
    public interface IGraphSceneDisplayEvents<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        void GraphChanged (
            object sender,
            GraphChangeArgs<TItem,TEdge> args,
            TItem sinkItem, IGraphScene<TItem, TEdge> sinkScene,
            IGraphSceneDisplay<TItem, TEdge> sinkDisplay);
    }
}