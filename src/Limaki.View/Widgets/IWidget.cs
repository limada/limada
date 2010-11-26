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

using Limaki.Drawing;
using System;

namespace Limaki.Widgets {
   public interface IWidget {
        IStyle Style { get; set; }
        IShape Shape { get; set; }
        PointI Location { get;set;}
        SizeI Size { get;set;}
        object Data { get;set;}
    }

    public interface IToolWidget:IWidget {
        
    }
}