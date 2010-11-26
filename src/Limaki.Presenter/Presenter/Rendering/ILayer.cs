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
 */


using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.Presenter.UI {
    public interface ILayer<T> : IRenderAction {
        Get<T> Data { get; set; }
        void DataChanged();

        PointI Origin { get; set; }
        SizeI Size { get; set;}
        
        Get<ICamera> Camera { get; set; }
        Get<IContentRenderer<T>> Renderer { get; set; }

        void OnPaint ( IRenderEventArgs e );
    }
}