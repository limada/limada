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

using Limaki.Common;
using Limaki.View.Viz.Rendering;

namespace Limaki.View.Viz.Visualizers {
    /// <summary>
    /// instruments the device specific 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class BackendComposer<TData, TBackend> : IComposer<Display<TData>> {

        public virtual TBackend Backend { get; set; }

        public virtual ILayer<TData> DataLayer { get; set; }
        public virtual IBackendRenderer BackendRenderer { get; set; }
        public virtual IViewport ViewPort { get; set; }
        public virtual ICursorHandler CursorHandler { get; set; }

        public virtual ISelectionRenderer MoveResizeRenderer { get; set; }
        public virtual ISelectionRenderer SelectionRenderer { get; set; }

        public abstract void Factor(Display<TData> display);
        public abstract void Compose(Display<TData> display);

    }
}