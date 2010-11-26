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
using System.Text;
using System.Drawing;
using Limaki.Drawing;

namespace Limaki.Drawing.Painters {
    public interface IPainter:IDisposable {

        RenderType RenderType { get; set; }

		IStyle Style { get;set; }

		IShape Shape { get;set; }
        void Render(Graphics g);
    }

    public interface IPainter<T>:IPainter {

		new IShape<T> Shape { get;set; }
    }

    [Flags]
    public enum RenderType {
        None = 0,
        Draw = 1,
        Fill = 2,
        DrawAndFill = Draw | Fill
    }
}
