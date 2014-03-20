/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Drawing;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View {

    public interface IVidgetBackend:IDisposable {

        Size Size { get; }
        
        // Renderer:
        void Update();
        void Invalidate();
        void Invalidate(Rectangle rect);

        void InitializeBackend (IVidget frontend, VidgetApplicationContext context);

        void SetFocus ();

    }
}