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


using System.Drawing;
using System.Drawing.Drawing2D;


namespace Limaki.Presenter.GDI.UI {
    public interface IGDIControl:IControl {
        void Invalidate(Region region);
        void Invalidate(GraphicsPath path);
    }
}