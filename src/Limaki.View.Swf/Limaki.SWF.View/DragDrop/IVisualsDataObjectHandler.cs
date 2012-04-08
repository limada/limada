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

using Limaki.Graphs;
using Limaki.Visuals;

namespace Limaki.View.Swf.DragDrop {
    public interface IVisualsDataObjectHandler:
        IDataObjectHandler<IGraph<IVisual,IVisualEdge>,IVisual> {
        
    }
}