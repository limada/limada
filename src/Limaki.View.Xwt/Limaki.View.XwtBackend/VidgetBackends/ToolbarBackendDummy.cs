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
using Limaki.View.Viz.Visualizers.Toolbars;

namespace Limaki.View.XwtBackend {

    public class ToolbarBackendDummy : VidgetBackend<Xwt.HBox>, IToolbarBackend {
        
        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
        }

        public void InsertItem (int index, IToolbarItemBackend backend) { }

        public void RemoveItem (IToolbarItemBackend backend) { }

        public void SetVisibility (Visibility value) { }
    }

    public class ArrangerToolbarBackend : ToolbarBackendDummy, IArrangerToolbarBackend { }

    public class DisplayModeToolbarBackend : ToolbarBackendDummy, IDisplayModeToolbarBackend { }

    public class MarkerToolbarBackend : ToolbarBackendDummy, IMarkerToolbarBackend { }

    public class SplitViewToolbarBackend : ToolbarBackendDummy, ISplitViewToolbarBackend { }

    public class LayoutToolbarBackend : ToolbarBackendDummy, ILayoutToolbarBackend { }
}