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


using System.Collections.Generic;
using Limaki.Graphs;

namespace Limaki.View.Viz.Mesh {
    /// <summary>
    /// a central place to register Displays and Scenes
    /// registered scenes and their backing 
    /// Graphs are notified of changes
    /// </summary>
    public interface IGraphSceneMesh<TItem, TEdge>  where TEdge : TItem, IEdge<TItem> {

        void AddScene (IGraphScene<TItem, TEdge> scene);
        void RemoveScene (IGraphScene<TItem, TEdge> scene);
        void AddDisplay (IGraphSceneDisplay<TItem, TEdge> display);
        void RemoveDisplay (IGraphSceneDisplay<TItem, TEdge> display);

        ICollection<IGraphScene<TItem, TEdge>> Scenes { get; }
        ICollection<IGraphSceneDisplay<TItem, TEdge>> Displays { get; }

        IGraphSceneMeshBackHandler<TItem, TEdge> BackHandler (IGraph<TItem, TEdge> graph);
        IGraphSceneMeshBackHandler<TItem, TEdge> BackHandler<TSourceItem, TSourceEdge> ();

        void CopyDisplayProperties (IGraphSceneDisplay<TItem, TEdge> sourceDisplay, IGraphSceneDisplay<TItem, TEdge> targetDisplay);
        IGraph<TItem, TEdge> CreateSinkGraph (IGraph<TItem, TEdge> source);
        IGraphScene<TItem, TEdge> CreateSinkScene (IGraph<TItem, TEdge> sourceGraph);

        TItem LookUp (IGraph<TItem, TEdge> sourceGraph, IGraph<TItem, TEdge> sinkGraph, TItem lookItem);
    }
}