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

using System.Collections.Generic;
using Xwt;

namespace Limaki.Drawing.Shapes {

    public class ShapeFactory : ShapeFactoryBase {

        protected override void InstrumentClazzes() {
            Add<IShape<Rectangle>>(() => new RectangleShape());
            Add<IShape<Vector>>(() => new VectorShape());

            Add<IRectangleShape> (() => new RectangleShape());
            Add<IRoundedRectangleShape> (() => new RoundedRectangleShape());
            Add<IBezierRectangleShape> (() => new BezierRectangleShape());
            Add<IVectorShape> (() => new VectorShape());
        }

        public static IEnumerable<IShape> Shapes () {
            yield return new RectangleShape ();
            yield return new RoundedRectangleShape ();
            yield return new VectorShape ();
            yield return new BezierRectangleShape { Jitter = 0 };
        }
    }
}
