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

namespace Limaki.Drawing.Shapes {
    public class ShapeFactory : ShapeFactoryBase {
        protected override void InstrumentClazzes() {
            Clazzes[typeof(RectangleI)] =  typeof(RectangleShape);
            Clazzes[typeof(Vector)] =  typeof(VectorShape);

            Clazzes[typeof(IRectangleShape)] =  typeof(RectangleShape);
            Clazzes[typeof(IRoundedRectangleShape)] =  typeof(RoundedRectangleShape);
            Clazzes[typeof(IBezierShape)] =  typeof(BezierShape);
            Clazzes[typeof(IVectorShape)] =  typeof(VectorShape);
        }
    }
}
