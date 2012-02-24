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
 */


using Limaki.Drawing;
using Limaki.Graphs;
using System;
using Limaki.Presenter.UI.GraphScene;

namespace Limaki.Presenter.Display {
    public interface IGraphSceneDisplay<TItem, TEdge>:IDisplay<IGraphScene<TItem, TEdge>>
    where TEdge : TItem, IEdge<TItem> {
        IGraphLayout<TItem, TEdge> Layout { get; set; }
        event EventHandler<GraphSceneEventArgs<TItem, TEdge>> SceneFocusChanged;
        void OnSceneFocusChanged();

        bool Check();

        SceneInfo Info { get; set; }
    }

   
}