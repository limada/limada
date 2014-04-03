/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Limaki.Drawing.XwtBackend;
using System;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    [Obsolete ("use ContextSurface")]
    public class XwtSurface : ContextSurface {

        public override Matrix Matrix {
            get { return base.Matrix ?? (base.Matrix = new Matrix()); } 
            set { base.Matrix = value; }
        }
    }
}