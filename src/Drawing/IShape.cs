/*
 * Limaki 
 * Version 0.063
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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.Drawing {
    public interface IShape : ICloneable, IDisposable {
        Type ShapeDataType { get;}
        Point this[Anchor i] { get;set;}
        Anchor IsAnchorHit(Point p, int hitSize);
        bool IsBorderHit(Point p, int hitSize);
        bool IsHit(Point p, int hitSize);
        void Transform(Matrice matrice);
        Rectangle BoundsRect { get; }
        Point Location { get; set; }
        Size Size { get; set;}
        IEnumerable<Anchor> Grips { get; }
    }

    public interface IShape<T> : IShape {
        T Data { get;set; }
        Point SetShapeGetAnchor(T shape, Anchor i);
    }
}