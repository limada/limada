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


using System.Collections.Generic;
using Limaki.Drawing;

namespace Limaki.Presenter {
    /// <summary>
    /// calculates the clipping area
    /// does not know the GraphicsModel (Widgets)
    /// </summary>
    public interface IClipper {
        IEnumerable<PointI> Hull { get; }
        Drawing.RectangleI Bounds { get; }
        bool RenderAll { get; set; }
        void Add ( IEnumerable<PointI> hull );
        void Clear();
        bool IsEmpty { get; }
    }
}