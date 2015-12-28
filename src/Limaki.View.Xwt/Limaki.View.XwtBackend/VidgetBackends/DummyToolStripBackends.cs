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

using Limada.View.Vidgets;
using Limaki.Usecases.Vidgets;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.XwtBackend {

    public class ToolStripBackendDummy : VidgetBackend<Xwt.Canvas>, IToolStripBackend {
        
        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
        }

        public void InsertItem (int index, IToolStripItemBackend toolStripItemBackend) { }

        public void RemoveItem (IToolStripItemBackend toolStripItemBackend) { }

        public void SetVisibility (Visibility value) { }

        
    }

    public class ArrangerToolStripBackend : ToolStripBackendDummy, IArrangerToolStripBackend { }

    public class DisplayModeToolStripBackend : ToolStripBackendDummy, IDisplayModeToolStripBackend { }

    public class MarkerToolStripBackend : ToolStripBackendDummy, IMarkerToolStripBackend { }

    public class SplitViewToolStripBackend : ToolStripBackendDummy, ISplitViewToolStripBackend { }

    public class LayoutToolStripBackend : ToolStripBackendDummy, ILayoutToolStripBackend { }
}