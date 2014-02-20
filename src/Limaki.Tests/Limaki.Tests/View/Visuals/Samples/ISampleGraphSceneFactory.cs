/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Visuals;
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using Limaki.Drawing;

namespace Limaki.Tests.View.Visuals {

    /// <summary>
    /// a <see cref="ISampleGraphFactory{IVisual, IVisualEdge}"/>
    /// that fills the GraphScene with SampleData
    /// </summary>
    public interface ISampleGraphSceneFactory : ISampleGraphFactory<IVisual, IVisualEdge> {

        IGraphScene<IVisual, IVisualEdge> Scene { get; }

        void PopulateScene (IGraphScene<IVisual, IVisualEdge> scene);
    }

}