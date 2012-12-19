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
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {
    public interface ICamera:IDisposable {
        /// <summary>
        /// The transformation matrix
        /// </summary>
        Matrix Matrix { get; set; }

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

        void FromSource(IShape s);
        void ToSource(IShape s);

    }

}
