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

using System;

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
        PointI ToSource ( PointI s );
        /// <summary>
        /// convert a source point into a transformed point (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        PointI FromSource ( PointI s );
        /// <summary>
        /// convert a transformed rectangle into a source rectangle (matrix.inverseTransform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        RectangleI ToSource(RectangleI s);
        /// <summary>
        /// convert a source rectangle into a transformed rectangle (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        RectangleI FromSource(RectangleI s);


        SizeI FromSource ( SizeI s );
        SizeI ToSource ( SizeI s );

        /// <summary>
        /// convert a transformed point into a source point (matrix.inverseTransform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        PointS ToSource(PointS s);
        /// <summary>
        /// convert a source point into a transformed point (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        PointS FromSource(PointS s);
        /// <summary>
        /// convert a transformed rectangle into a source rectangle (matrix.inverseTransform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        RectangleS ToSource(RectangleS s);
        /// <summary>
        /// convert a source rectangle into a transformed rectangle (matrix.transform)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        RectangleS FromSource(RectangleS s);

        SizeS FromSource(SizeS s);
        SizeS ToSource(SizeS s);

        void FromSource(IShape s);
        void ToSource(IShape s);


    }

}
