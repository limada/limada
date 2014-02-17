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
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Visuals;
using Limada.Model;
using System.Linq;
using Limaki.View.Visualizers;
using Limaki.Common;

namespace Limaki.View.Visuals.UI {

    public interface IGraphSceneMesh<TItem, TEdge>  where TEdge : TItem, IEdge<TItem> {

        void AddScene (IGraphScene<TItem, TEdge> scene);
        void RemoveScene (IGraphScene<TItem, TEdge> scene);
        void AddDisplay (IGraphSceneDisplay<TItem, TEdge> display);
        void RemoveDisplay (IGraphSceneDisplay<TItem, TEdge> display);
        ICollection<IGraphScene<TItem, TEdge>> Scenes { get; }
        ICollection<IGraphSceneDisplay<TItem, TEdge>> Displays { get; }

        void CopyDisplayProperties (IGraphSceneDisplay<TItem, TEdge> sourceDisplay, IGraphSceneDisplay<TItem, TEdge> targetDisplay);
        IGraph<TItem, TEdge> CreateTargetGraph (IGraph<TItem, TEdge> source);
        IGraphScene<TItem, TEdge> CreateTargetScene (IGraph<TItem, TEdge> sourceGraph);
    }
}