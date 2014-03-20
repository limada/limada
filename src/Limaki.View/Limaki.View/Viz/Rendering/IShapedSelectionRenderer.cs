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

namespace Limaki.View.Viz.Rendering {
    public interface IShapedSelectionRenderer:ISelectionRenderer {
        IPainter Painter { get; set; }
        RenderType RenderType { get; set; }
    }
}