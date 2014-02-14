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

    public interface IGraphSceneMesh { }

    public interface IGraphSceneMesh<TItem, TEdge> : IGraphSceneMesh where TEdge : TItem, IEdge<TItem> {
        void AddScene (IGraphScene<TItem, TEdge> scene);
        void RemoveScene (IGraphScene<TItem, TEdge> scene);
        void AddDisplay (IGraphSceneDisplay<TItem, TEdge> display);
        void RemoveDisplay (IGraphSceneDisplay<TItem, TEdge> display);
    }


    public interface IGraphSceneMesh<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> : IGraphSceneMesh<TSinkItem, TSinkEdge>
        where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        ICollection<IGraphScene<TSinkItem, TSinkEdge>> Scenes { get; }
        ICollection<IGraphSceneDisplay<TSinkItem, TSinkEdge>> Displays { get; }
        ICollection<IGraph<TSourceItem, TSourceEdge>> BackGraphs { get; }

        IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfBackGraph (IGraph<TSourceItem, TSourceEdge> backGraph);

    }
}