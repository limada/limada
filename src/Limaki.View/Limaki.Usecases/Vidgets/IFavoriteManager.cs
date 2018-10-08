/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Graphs;
using Limaki.View.Visuals;
using Limaki.View.Viz;


namespace Limaki.View.Vidgets {

    public interface IFavoriteManager {
        Int64 HomeId { get; set; }
        IVisualSceneStoreInteractor SceneManager { get; set; }
        VisualsDisplayHistory VisualsDisplayHistory { get; set; }
        void AddToFavorites (IGraphScene<IVisual, IVisualEdge> scene);
        void SetAutoView (IGraphScene<IVisual, IVisualEdge> scene);
        void ResetHomeId ();
        void GoHome (IGraphSceneDisplay<IVisual, IVisualEdge> display, bool initialize);
        bool AddToSheets (IGraph<IVisual, IVisualEdge> graph, Int64 sheetId);
        void SaveChanges (IEnumerable<IGraphSceneDisplay<IVisual, IVisualEdge>> displays);
        void Clear ();
    }
    
}