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

using System;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View {
    public interface IControl:IDisposable {
        
        RectangleD ClientRectangle { get;}
        Size Size {get;}
        
        // Renderer:
        void Update();
        void Invalidate();
        void Invalidate(RectangleD rect);

        // Helper Functions:
        Point PointToClient(Point source);

    }
}