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

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Xwt.Drawing;
using Xwt.Backends;

namespace Limaki.View {

    public interface IDisplay : IVidget {

        Color BackColor { get; set; }
        ZoomState ZoomState { get; set; }

        IMoveResizeAction SelectAction { get; set; }
        MouseScrollAction MouseScrollAction { get; set; }
        IEventControler EventControler { get; set; }

        IClipper Clipper { get; set; }
        IStyleSheet StyleSheet { get; set; }
        IViewport Viewport { get; set; }
        IBackendRenderer BackendRenderer { get; set; }

        void Reset ();
        void Perform ();

        IDisplayBackend Backend { get; }

        object ActiveVidget { get; set; }

    }

    public interface IDisplay<T> : IDisplay {

        T Data { get; set; }
        Int64 DataId { get; set; }
        string Text { get; set; }
        new IDisplayBackend<T> Backend { get; set; }
        State State { get; }
        //ILayout<T> Layout { get; set; }

    }
}