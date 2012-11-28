/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing;
using Xwt;

namespace Limaki.View.Layout {
    /// <summary>
    /// manipulates Size and Location
    /// of an Item
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public interface ILocator<TItem> {
        
        Point GetLocation(TItem item);
        void SetLocation(TItem item, Point location);
        bool HasLocation (TItem item);

        Size GetSize(TItem item);
        void SetSize(TItem item, Size value);
        bool HasSize (TItem item);
    }

}