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

using Limaki.Presenter.UI;
using Limaki.Drawing;
using Id = System.Int64;
using Limaki.Common;

namespace Limaki.Presenter {
    public interface IDisplay {

        Color BackColor { get; set; }
        ZoomState ZoomState { get; set; }

        IMoveResizeAction SelectAction { get; set; }
        ScrollAction ScrollAction { get; set; }
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
        Id DataId { get; set; }
        string Text { get; set; }
        IDisplayDevice<T> Device { get; set; }
        State State { get; }
        //ILayout<T> Layout { get; set; }
        
    }
}