/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Graphs;
using Limaki.Visuals;

namespace Limaki.Viewers {

    /// <summary>
    /// a ViewerController that shows a special Content behind a Visual
    /// </summary>
    public abstract class ContentVisualViewer:ContentViewer {
        public abstract bool Supports(IGraph<IVisual, IVisualEdge> graph, IVisual visual);
        public abstract void SetContent(IGraph<IVisual, IVisualEdge> graph, IVisual visual);
    }
}