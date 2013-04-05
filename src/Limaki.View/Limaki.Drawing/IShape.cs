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

using System;
using System.Collections.Generic;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {
    /// <summary>
    /// a shape holds the information about a geometric figure
    /// it stores the location and the size
    /// </summary>
    public interface IShape : ICloneable, IDisposable {
        /// <summary>
        /// System.Type of the underlying geometric data
        /// </summary>
        Type ShapeDataType { get;}

        /// <summary>
        /// get or set the Anchor-Points
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        Point this[Anchor i] { get;set;}

        /// <summary>
        /// gives back the Anchor which is hit with point
        /// within a tolerance
        /// </summary>
        /// <param name="p">the point to test</param>
        /// <param name="hitSize">the tolerance</param>
        /// <returns></returns>
        Anchor IsAnchorHit(Point p, int hitSize);

        /// <summary>
        /// tests if the border of the shape is hit
        /// </summary>
        /// <param name="p"></param>
        /// <param name="hitSize"></param>
        /// <returns></returns>
        bool IsBorderHit(Point p, int hitSize);

        /// <summary>
        /// tests if the shape is hit (including border)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="hitSize"></param>
        /// <returns></returns>
        bool IsHit(Point p, int hitSize);

        /// <summary>
        /// transforms the shape
        /// </summary>
        /// <param name="matrix"></param>
        void Transform(Matrix matrix);

        /// <summary>
        /// the clipping rectangle of the shape
        /// </summary>
        Rectangle BoundsRect { get; }

        /// <summary>
        /// location
        /// </summary>
        Point Location { get; set; }

        /// <summary>
        /// size of the shape
        /// </summary>
        Size Size { get; set;}

        /// <summary>
        /// size of the Data; could be smaller than Size, so data.Size + some offset gives shape.Size
        /// </summary>
        Size DataSize { get; set; }

        /// <summary>
        /// a enumeration of anchors to be used as 
        /// grippoints for moving, resizing
        /// </summary>
        IEnumerable<Anchor> Grips { get; }

        Point[] Hull ( int delta, bool extend );

        Point[] Hull ( Matrix matrix, int delta, bool extend );


    }

    public interface IShape<T> : IShape {
        /// <summary>
        /// underlying data structure of the shape
        /// </summary>
        T Data { get;set; }

        /// <summary>
        /// sets the data and gives back the point of anchor i
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        Point SetShapeGetAnchor(T shape, Anchor i);
    }
}