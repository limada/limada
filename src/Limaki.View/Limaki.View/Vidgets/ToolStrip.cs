/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Xwt;
using Xwt.Backends;

namespace Limaki.View.Vidgets {
    
    [BackendType (typeof (IToolStripBackend))]
    public class ToolStrip : ContainerVidget<ToolStripItem, IToolStripItemBackend> {
        public Size DefaultSize { get; set; } = new Size (36, 36);
    }

    public interface IToolStripItemBackendContainer : IItemContainer<IToolStripItemBackend> { }

    public interface IToolStripBackend : IVidgetBackend, IContainerVidgetBackend<IToolStripItemBackend> {}

}