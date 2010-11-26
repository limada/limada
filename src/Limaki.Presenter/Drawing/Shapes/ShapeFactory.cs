/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

namespace Limaki.Drawing.Shapes {
    public class ShapeFactory : ShapeFactoryBase {
        protected override void InstrumentClazzes() {
            Clazzes.Add(typeof(RectangleI), typeof(RectangleShape));
            Clazzes.Add(typeof(Vector), typeof(VectorShape));

            Clazzes.Add(typeof(IRectangleShape), typeof(RectangleShape));
            Clazzes.Add(typeof(IRoundedRectangleShape), typeof(RoundedRectangleShape));
            Clazzes.Add(typeof(IBezierShape), typeof(BezierShape));
            Clazzes.Add(typeof(IVectorShape), typeof(VectorShape));
        }
    }
}
