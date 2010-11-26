/*
 * Limaki 
 * Version 0.08
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

namespace Limaki.Drawing {
    public interface IRectangleShape : IShape<RectangleI> { }

    public interface IRoundedRectangleShape : IShape<RectangleI> { }

    public interface IVectorShape : IShape<Vector> { }

    public interface IBezierShape : IShape<RectangleI> {
        PointS[] BezierPoints { get; }
    }
}