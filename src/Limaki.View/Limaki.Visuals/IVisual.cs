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

using Limaki.Drawing;
using Xwt;

namespace Limaki.Visuals {
   public interface IVisual {
        IStyleGroup Style { get; set; }
        IShape Shape { get; set; }
        Point Location { get;set;}
        Size Size { get;set;}
        object Data { get;set;}
    }

    public interface IVisualTool:IVisual {
        
    }
}