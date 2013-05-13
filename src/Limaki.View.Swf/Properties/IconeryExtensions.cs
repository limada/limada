/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Drawing;
using Limaki.Iconerias;
using Xwt.Drawing;
using Xwt.Engine;
using Image = System.Drawing.Image;
using Xwt.Gdi.Backend;

namespace Limaki.View.Properties {
    public static class IconeryExtensions {
        public static Image ToGdiImage (this Iconeria iconeria, Action<Context> icon, int size) {
            var img = iconeria.AsImage(GdiEngine.Registry, icon, size);
            return (img as Xwt.Backends.IFrontend).Backend as System.Drawing.Image;
        }
    }
}