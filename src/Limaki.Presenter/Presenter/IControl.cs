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
using System;

namespace Limaki.Presenter {
    public interface IControl:IDisposable {
        
        RectangleI ClientRectangle { get;}
        SizeI Size {get;}
        
        // Renderer:
        void Update();
        void Invalidate();
        void Invalidate(RectangleI rect);

        // Helper Functions:
        PointI PointToClient(PointI source);

    }
}