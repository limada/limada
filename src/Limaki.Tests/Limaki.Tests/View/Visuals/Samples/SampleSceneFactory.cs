/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
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

namespace Limaki.Tests.View.Visuals {

    public class SampleSceneFactory<Item, TEdge, T> : SampleGraphSceneFactory<IVisual, Item, IVisualEdge, TEdge, T>, ISampleGraphSceneFactory
        where TEdge : IEdge<Item>, Item
        where T : ISampleGraphFactory<Item, TEdge>, new () {

        protected override IGraphScene<IVisual, IVisualEdge> CreateScene () {
            return new Scene ();
        }
    }
}