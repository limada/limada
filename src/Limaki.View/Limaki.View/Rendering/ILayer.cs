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
 */


using Limaki.Common;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.Rendering {
    public interface ILayer<T> : IRenderAction {
        Get<T> Data { get; set; }
        void DataChanged();

        Point Origin { get; set; }
        Size Size { get; set;}
        
        Get<ICamera> Camera { get; set; }
        Get<IContentRenderer<T>> Renderer { get; set; }

        void OnPaint ( IRenderEventArgs e );
    }
}