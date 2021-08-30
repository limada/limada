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
using Limaki.View.GraphScene;

namespace Limaki.View.Viz.Mapping {
    /// <summary>
    /// a central place to register Displays and Scenes
    /// registered scenes and their backing 
    /// Graphs are notified of changes
    /// </summary>
    public interface IGraphSceneMapDisplayOrganizer<TItem, TEdge> : IGraphSceneMapOrganizer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        new IGraphSceneDisplayMapInteractor<TItem, TEdge> MapInteractor (IGraph<TItem, TEdge> graph);
		new IGraphSceneMapInteractor<TItem, TSourceItem, TEdge, TSourceEdge> MapInteractor<TSourceItem, TSourceEdge> () where TSourceEdge : TSourceItem, IEdge<TSourceItem>; 

        void AddDisplay (IGraphSceneDisplay<TItem, TEdge> display);
        void RemoveDisplay (IGraphSceneDisplay<TItem, TEdge> display);

        ICollection<IGraphSceneDisplay<TItem, TEdge>> Displays { get; }

        void CopyDisplayProperties (IGraphSceneDisplay<TItem, TEdge> sourceDisplay, IGraphSceneDisplay<TItem, TEdge> sinkDisplay);

        void Clear ();
    }
}