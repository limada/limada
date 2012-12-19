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


using System.Drawing.Drawing2D;
using Xwt.Drawing;
using Matrix = Xwt.Drawing.Matrix;

namespace Limaki.Drawing.Gdi {
    public class GdiMatrice:Matrix {
        public System.Drawing.Drawing2D.Matrix Matrix {
            get {
                #if UseMatrice
                return new Matrix((float)m11, (float)m12, (float)m21, (float)m22, (float)dx, (float)dy);
#else 
                return new System.Drawing.Drawing2D.Matrix((float) M11, (float) M12, (float) M21, (float) M22, 
                    (float) OffsetX, (float) OffsetY); 

#endif
                }
            set {
                var elements = value.Elements;
#if UseMatrice
                   this.m11 = elements[0];
                this.m12 = elements[1];
                this.m21 = elements[2];
                this.m22 = elements[3];
                this.dx = elements[4];
                this.dy = elements[5];
                
#else
                this.M11 = elements[0];
                this.M12 = elements[1];
                this.M21 = elements[2];
                this.M22 = elements[3];
                this.OffsetX = elements[4];
                this.OffsetY = elements[5];
#endif
            }
        }
    }
}