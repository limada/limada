/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View {
    /// <summary>
    /// intermediate marker interface for Widgets
    /// will be replaced by Xwt.Widget in the future
    /// is like a controller for a concrete backend
    /// </summary>
    public interface IVidget : IDisposable {

        IVidgetBackend Backend { get; }
        Size Size { get; }

        void SetFocus ();
        event EventHandler GotFocus;

        event EventHandler ButtonReleased;

        void Update ();
        void QueueDraw ();
        void QueueDraw (Rectangle rect);

    }

    public interface IVidgetEventSink {
        void OnEvent<T> (string name, T args) where T:EventArgs;
    }
}
