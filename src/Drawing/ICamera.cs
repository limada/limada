/*
 * Limaki 
 * Version 0.07
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.Drawing {
    public interface ICamera:IDisposable {
        /// <summary>
        /// The transformation matrix
        /// </summary>
        Matrice Matrice { get; set; }

        /// <summary>
        /// convert a transformed point into a source point (matrix.inverseTransform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Point ToSource ( Point s );
        /// <summary>
        /// convert a source point into a transformed point (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Point FromSource ( Point s );
        /// <summary>
        /// convert a transformed rectangle into a source rectangle (matrix.inverseTransform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Rectangle ToSource(Rectangle s);
        /// <summary>
        /// convert a source rectangle into a transformed rectangle (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        Rectangle FromSource(Rectangle s);


        Size FromSource ( Size s );
        Size ToSource ( Size s );

        /// <summary>
        /// convert a transformed point into a source point (matrix.inverseTransform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        PointF ToSource(PointF s);
        /// <summary>
        /// convert a source point into a transformed point (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        PointF FromSource(PointF s);
        /// <summary>
        /// convert a transformed rectangle into a source rectangle (matrix.inverseTransform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        RectangleF ToSource(RectangleF s);
        /// <summary>
        /// convert a source rectangle into a transformed rectangle (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        RectangleF FromSource(RectangleF s);

        SizeF FromSource(SizeF s);
        SizeF ToSource(SizeF s);

        void FromSource(IShape s);
        void ToSource(IShape s);


    }

}
