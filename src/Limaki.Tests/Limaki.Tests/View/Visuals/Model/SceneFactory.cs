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

using Limaki.Model;
using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.Collections.Generic;
using Limaki.Drawing;

namespace Limaki.Tests.Visuals {

    public class SceneFactory<T> : SceneFactory<IGraphEntity, IGraphEdge, T> where T : ISampleGraphFactory<IGraphEntity, IGraphEdge>, new() { }

    public class SceneFactory<Item, TEdge, T> : GraphSceneFactory<IVisual, Item, IVisualEdge, TEdge,T>, ISceneFactory
        where TEdge : IEdge<Item>, Item
        where T : ISampleGraphFactory<Item, TEdge>, new() {

        protected override IGraphScene<IVisual, IVisualEdge> CreateScene () {
            return new Scene();
        }
        }
}