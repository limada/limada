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

using Xwt;

namespace Limaki.Drawing.Shapes {
    public interface IRectangleShape : IShape<RectangleD> { }

    public interface IRoundedRectangleShape : IShape<RectangleD> { }

    public interface IVectorShape : IShape<Vector> { }

    public interface IBezierShape : IShape<RectangleD> {
        Point[] BezierPoints { get; }
    }
}