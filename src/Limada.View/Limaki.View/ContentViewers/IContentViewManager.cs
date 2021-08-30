/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 */

using System;
using Limaki.Graphs;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.UI.GraphScene;
using Xwt.Drawing;

namespace Limaki.View.ContentViewers {

    public interface IContentViewManager : IDisposable {
        
        ContentViewer CurrentViewer { get; set; }
        IGraphSceneDisplay<IVisual, IVisualEdge> SheetViewer { get; set; }
        IVisualSceneStoreInteractor StoreInteractor { get; set; }
        Color BackColor { get; set; }

        Action<ContentViewer> AttachCurrentViewer { get; set; }
        Action<ContentViewer> DetachCurrentViewer { get; set; }

        bool IsStreamOwner { get; set; }
        bool IsProviderOwner { get; set; }

        bool IsContent (IGraph<IVisual, IVisualEdge> visualGraph, IVisual visual);
        void SaveStream (IGraph<IVisual, IVisualEdge> graph, ContentStreamViewer viewer);
        void SaveContentOfViewers ();
        void ShowViewer (object sender, GraphSceneEventArgs<IVisual, IVisualEdge> e);
        void Clear ();
    }
    
}