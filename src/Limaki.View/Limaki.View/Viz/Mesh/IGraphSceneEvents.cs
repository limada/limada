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

using Limaki.Graphs;

namespace Limaki.View.Viz.Mesh {

    /// <summary>
    /// events handling the changes of the 
    /// sink in a meshed GraphScene
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IGraphSceneEvents<TItem, TEdge> where TEdge : IEdge<TItem>, TItem {

        void GraphChanged (
            IGraph<TItem, TEdge> sourceGraph, TItem sourceItem,
            TItem sinkItem, IGraphScene<TItem, TEdge> sinkScene,
            IGraphSceneDisplay<TItem, TEdge> sinkDisplay, GraphEventType eventType);
    }
}