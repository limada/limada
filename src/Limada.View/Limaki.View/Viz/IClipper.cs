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

namespace Limaki.View.Viz {
    /// <summary>
    /// calculates the clipping area
    /// does not know the GraphicsModel (Visuals)
    /// </summary>
    public interface IClipper {
        IEnumerable<Point> Hull { get; }
        Rectangle Bounds { get; }
        bool RenderAll { get; set; }
        void Add ( IEnumerable<Point> hull );
        void Clear();
        bool IsEmpty { get; }
    }
}