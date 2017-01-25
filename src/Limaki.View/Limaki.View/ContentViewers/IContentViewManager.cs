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
using System.IO;
using System.Linq;
using Limada.Model;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.UI.GraphScene;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.ContentViewers {

    public interface IContentViewManager : IDisposable {
        
        ContentViewer CurrentViewer { get; set; }
        IGraphSceneDisplay<IVisual, IVisualEdge> SheetViewer { get; set; }
        ISceneManager SceneManager { get; set; }
        Color BackColor { get; set; }
        Action<IVidget, Action> AttachViewer { get; set; }
        Action<IVidgetBackend> AttachViewerBackend { get; set; }
        Action<IVidgetBackend> ViewersDetachBackend { get; set; }
        bool IsStreamOwner { get; set; }
        bool IsProviderOwner { get; set; }

        bool IsContent (IGraph<IVisual, IVisualEdge> visualGraph, IVisual visual);
        void SaveStream (IGraph<IVisual, IVisualEdge> graph, ContentStreamViewer viewer);
        void SaveContentOfViewers ();
        void ShowViewer (object sender, GraphSceneEventArgs<IVisual, IVisualEdge> e);
        void Clear ();
    }
    
}