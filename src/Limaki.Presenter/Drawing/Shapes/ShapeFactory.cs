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
            Add<IShape<RectangleI>>(() => new RectangleShape());
            Add<IShape<Vector>>(() => new VectorShape());

            Add<IRectangleShape> (() => new RectangleShape());
            Add<IRoundedRectangleShape> (() => new RoundedRectangleShape());
            Add<IBezierShape> (() => new BezierShape());
            Add<IVectorShape> (() => new VectorShape());
        }
    }
}
