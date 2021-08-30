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
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using System.Collections.Generic;
using Limaki.Drawing;
using Limaki.Common.Linqish;
using System.Linq;
using Limaki.View;
using Limaki.View.Visuals;

namespace Limaki.Tests.View.Visuals {

    public class SampleSceneFactory<TItem, TEdge, T> : SampleGraphSceneFactory<IVisual, TItem, IVisualEdge, TEdge, T>, ISampleGraphSceneFactory
        where TEdge : IEdge<TItem>, TItem
        where T : ISampleGraphFactory<TItem, TEdge>, new () {

        protected override IGraphScene<IVisual, IVisualEdge> CreateScene () {
            return new Scene ();
        }

        
    }
}