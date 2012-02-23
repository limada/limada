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

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Presenter.Rendering;
using Limaki.Presenter.UI;
using Xwt.Drawing;

namespace Limaki.Presenter {
    public interface IDisplay {

        Color BackColor { get; set; }
        ZoomState ZoomState { get; set; }

        IMoveResizeAction SelectAction { get; set; }
        MouseScrollAction MouseScrollAction { get; set; }
        IEventControler EventControler { get; set; }

        IClipper Clipper { get; set; }
        IStyleSheet StyleSheet { get; set; }
        IViewport Viewport { get; set; }
        IDeviceRenderer DeviceRenderer { get; set; }

        void Invoke();
        void Execute();

        object ActiveControl { get; set; }
        
    }

    public interface IDisplay<T>:IDisplay {
        T Data { get; set; }
        Int64 DataId { get; set; }
        string Text { get; set; }
        IDisplayDevice<T> Device { get; set; }
        State State { get; }
        //ILayout<T> Layout { get; set; }
        
    }
}