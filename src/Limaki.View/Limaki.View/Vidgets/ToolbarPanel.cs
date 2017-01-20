/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.ObjectModel;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolbarPanelBackend))]
    public class ToolbarPanel : ContainerVidget<ToolStrip, IToolStripBackend> {
    }

    public interface IToolbarPanelBackend : IVidgetBackend, IContainerVidgetBackend<IToolStripBackend> { }
}
