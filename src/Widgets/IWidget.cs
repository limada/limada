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

using System.Drawing;
using Limaki.Drawing;

namespace Limaki.Widgets {

    public interface IWidget {
        IStyle Style { get; set; }
        IShape Shape { get; set; }
        Point Location { get;set;}
        Size Size { get;set;}
        object Data { get;set;}
    }

    public interface IToolWidget:IWidget {
        
    }
}