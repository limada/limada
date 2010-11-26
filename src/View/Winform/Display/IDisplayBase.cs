/*
 * Limaki 
 * Version 0.071
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Winform;

namespace Limaki.Winform.Displays {
    public interface IDisplayBase : IDragDopControl,IZoomTarget,IScrollTarget {
        void Clear();

		///<directed>True</directed>
		ILayer DataLayer { get;set;}
        Size DataSize { get; }
        TAction GetAction<TAction>();
        ScrollAction ScrollAction { get; set; }
        ZoomAction ZoomAction { get; set; }
        SelectionBase SelectAction { get; set; }
        event EventHandler ZoomChanged;
        //Rectangle ClientRectangle { get;}
        event PaintEventHandler Paint;
    }
}
