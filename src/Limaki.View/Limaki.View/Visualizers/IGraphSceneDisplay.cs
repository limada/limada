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
 */

using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.Modelling;
using Limaki.View.Rendering;
using Limaki.View.UI.GraphScene;

namespace Limaki.View.Visualizers {

    public interface IGraphSceneDisplay<TItem, TEdge>:IDisplay<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {

        /// <summary>
        ///
        /// <remarks> set this after Data; DataChanged sets this to null</remarks>
        /// </summary>
        SceneInfo Info { get; set; }
        IGraphSceneLayout<TItem, TEdge> Layout { get; set; }
        IGraphSceneReceiver<TItem, TEdge> GraphSceneReceiver { get; set; }
        IModelReceiver<TItem> ModelReceiver { get; set; }
        IGraphItemRenderer<TItem, TEdge> GraphItemRenderer { get; set; }

        event EventHandler<GraphSceneEventArgs<TItem, TEdge>> SceneFocusChanged;
        void OnSceneFocusChanged();

        bool Check();
        
    }
  
}