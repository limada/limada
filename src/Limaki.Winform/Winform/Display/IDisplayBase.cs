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

using System;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Winform.Displays {
    public interface IDisplayBase : IDragDopControl,IZoomTarget,IScrollTarget {
        void Clear();

		///<directed>True</directed>
		ILayer DataLayer { get;set;}
        SizeI DataSize { get; }
        TAction GetAction<TAction>();
        ScrollAction ScrollAction { get; set; }
        ZoomAction ZoomAction { get; set; }
        SelectionBase SelectAction { get; set; }
        event EventHandler ZoomChanged;

    }

    public interface IDisplayBase<T>:IDisplayBase, IDataDisplay<T> {
        DisplayKit<T> DisplayKit { get; }
    }


}
