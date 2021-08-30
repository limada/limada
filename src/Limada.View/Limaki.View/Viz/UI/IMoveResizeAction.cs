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
using Xwt;

namespace Limaki.View.Viz.UI {
    public interface IMoveResizeAction:IMouseAction {
        int HitSize { get; set; }
        ICursorHandler CursorHandler { get; }

        Anchor HitAnchor ( Point p );
        bool HitTest ( Point p );

        bool ShowGrips { get; set; }
        void Clear();
    }
}