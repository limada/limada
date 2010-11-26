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

namespace Limaki.Presenter.Display {
    /// <summary>
    /// instruments the device specific 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class DeviceComposer<TData, TDevice> : IComposer<Display<TData>>
         {

        public virtual TDevice Device { get; set; }

        public virtual ILayer<TData> DataLayer { get; set; }
        public virtual IDeviceRenderer DeviceRenderer { get; set; }
        public virtual IViewport ViewPort { get; set; }
        public virtual IDeviceCursor DeviceCursor { get; set; }

        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }
        public virtual ISelectionRenderer SelectionRenderer { get; set; }

        public abstract void Factor(Display<TData> display);
        public abstract void Compose(Display<TData> display);

    }
}