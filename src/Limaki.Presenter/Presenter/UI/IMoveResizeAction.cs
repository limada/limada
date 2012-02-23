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


using Limaki.Drawing;
using Xwt;

namespace Limaki.Presenter.UI {
    public interface IMoveResizeAction:IMouseAction {
        int HitSize { get; set; }
        IDeviceCursor DeviceCursor { get; }

        Anchor HitAnchor ( Point p );
        bool HitTest ( Point p );

        bool ShowGrips { get; set; }
        void Clear();
    }
}