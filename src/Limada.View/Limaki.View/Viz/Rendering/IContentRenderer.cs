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

namespace Limaki.View.Viz.Rendering {
    /// <summary>
    /// this class 
    /// encapsulates the content-specific rendering
    /// it is called by the layers OnPaint-Method
    /// it should have no dependencies on a specific backend
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContentRenderer<T>  {
        void Render(T data, IRenderEventArgs e);
        Func<ICamera> Camera { get; set; }
    }

    public interface IGraphItemRenderer<TItem, TEdge> : IContentRenderer<TItem>
    where TEdge : TItem, IEdge<TItem> {
        Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
    }

    public interface IGraphSceneRenderer<TItem, TEdge> : IContentRenderer<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {
        
        IGraphItemRenderer<TItem, TEdge> ItemRenderer { get; set; }

        Func<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }
        

    }
}