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
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Presenter.Modelling {
    /// <summary>
    /// updates the data with the modeller
    /// feeds the Clipper with TItems hulls
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public interface ISceneReceiver<TItem, TEdge> : IReceiver
        where TEdge : TItem, IEdge<TItem> { // "Realizer"; == SceneController
        Get<IGraphScene<TItem, TEdge>> GraphScene { get; set; }
        Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }
        Get<ICamera> Camera { get; set; }
        Get<IClipper> Clipper { get; set; }
        Get<IModelReceiver<TItem>> ModelReceiver { get; set; }
    }
}