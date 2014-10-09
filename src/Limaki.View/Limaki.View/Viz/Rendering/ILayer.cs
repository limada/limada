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
 */


using Limaki.Drawing;
using Xwt;
using System;

namespace Limaki.View.Viz.Rendering {

    public interface ILayer<T> : IRenderAction {
        Func<T> Data { get; set; }
        void DataChanged();

        Point Origin { get; set; }
        Size Size { get; set;}

        double Alpha { get; set; }

        Func<ICamera> Camera { get; set; }
        Func<IContentRenderer<T>> Renderer { get; set; }

    }
}