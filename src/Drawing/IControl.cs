/*
 * Limaki 
 * Version 0.064
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


using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Limaki.Drawing {
    public interface IControl {
        void Invalidate();
        void Invalidate(Rectangle rect);
        void Invalidate(Region region);
        void Invalidate(GraphicsPath path);
        Rectangle ClientRectangle { get;}
        void Update();
        void CommandsExecute();
    }
}
