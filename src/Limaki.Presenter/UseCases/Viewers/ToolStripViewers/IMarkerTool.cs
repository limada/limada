/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Visuals;
using Limaki.Drawing;

namespace Limaki.UseCases.Viewers.ToolStripViewers {
    public interface IMarkerTool {
        void Attach(IGraphScene<IVisual, IVisualEdge> scene);
        void Detach(IGraphScene<IVisual, IVisualEdge> oldScene);
    }
}