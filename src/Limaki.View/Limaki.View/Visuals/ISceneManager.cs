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
using System.IO;
using Limaki.Contents;
using Limaki.Graphs;

namespace Limaki.View.Visuals {
    
    /// <summary>
    /// replaces ISheetManager
    /// replaces all methods where to load and save scenes
    /// </summary>
    public interface ISceneManager {

        SheetStore SheetStore { get; set; }

        bool IsSaveable (IGraphScene<IVisual, IVisualEdge> scene);
        bool SaveStreamInGraph (Stream source, IGraph<IVisual, IVisualEdge> target, SceneInfo info);
        void SaveInGraph (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, SceneInfo info);
        bool SaveInStore (IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout, long id);

        Stream StreamFromStore (long id);

        IGraphScene<IVisual, IVisualEdge> Load (IGraph<IVisual, IVisualEdge> source, IGraphSceneLayout<IVisual, IVisualEdge> layout, Int64 id);
        IGraphScene<IVisual, IVisualEdge> LoadFromStore (IGraph<IVisual, IVisualEdge> source, IGraphSceneLayout<IVisual, IVisualEdge> layout, Int64 id);
        IGraphScene<IVisual, IVisualEdge> LoadFromContent (Content<Stream> source, IGraph<IVisual, IVisualEdge> sourceGraph, IGraphSceneLayout<IVisual, IVisualEdge> layout);
        void Clear ();
    }
   
}