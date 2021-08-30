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

using Xwt;

namespace Limaki.Drawing.Shapes {
    public class RectangleShape : RectangleShapeBase, IRectangleShape {
        public RectangleShape():base() {}
        public RectangleShape(Rectangle data) : base(data) {}
        
        public override object Clone() {
            return new RectangleShape(_data);
        }
    }
}