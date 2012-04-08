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


using System.Drawing.Drawing2D;

namespace Limaki.Drawing.Gdi {
    public class GdiMatrice:Matrice {
        public Matrix Matrix {
            get { return new Matrix((float)m11, (float)m12, (float)m21, (float)m22, (float)dx, (float)dy); }
            set {
                var elements = value.Elements;
                this.m11 = elements[0];
                this.m12 = elements[1];
                this.m21 = elements[2];
                this.m22 = elements[3];
                this.dx = elements[4];
                this.dy = elements[5];
            }
        }
    }
}