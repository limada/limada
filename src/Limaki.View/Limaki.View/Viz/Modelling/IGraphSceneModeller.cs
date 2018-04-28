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


using Limaki.Drawing;
using Limaki.Graphs;
using System;
using Limaki.Actions;

namespace Limaki.View.Viz.Modelling {

    /// <summary>
    /// updates the data with the Modeller
    /// feeds the Clipper with TItems hulls
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface IGraphSceneModeller<TItem, TEdge> : IReceiver
        where TEdge : TItem, IEdge<TItem> { // "Realizer"; == SceneController
       
        Func<IGraphScene<TItem, TEdge>> GraphScene { get; set; }
        Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        Func<ICamera> Camera { get; set; }
        Func<IClipper> Clipper { get; set; }
        Func<ICommandModeller<TItem>> Modeller { get; set; }

    }
}