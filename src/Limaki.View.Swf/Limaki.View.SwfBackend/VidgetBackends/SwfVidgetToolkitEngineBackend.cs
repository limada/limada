/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.View.Vidgets;
using Limada.View.SwfBackend;
using Limaki.Usecases.Vidgets;
using Limaki.View.SwfBackend.Viz;
using Limaki.View.SwfBackend.Viz.ToolStrips;
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.Viz.Visuals;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class SwfVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {
            
            base.InitializeBackends();

            RegisterBackend<ISdImageDisplayBackend, SdImageDisplayBackend>();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend>();

            RegisterBackend<ITextViewerBackend, TextViewerBackend>();
            RegisterBackend<ITextViewerWithToolstripBackend, TextViewerWithToolstripBackend>();
            RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();

            RegisterBackend<ISplitViewBackend, SplitViewBackend>();
            RegisterBackend<ITextOkCancelBoxBackend, TextOkCancelBoxBackend> ();
            RegisterBackend<IDigidocViewerBackend, DigidocViewerBackend>();

            RegisterBackend<IToolStripBackend, ToolStripBackend> ();
            RegisterBackend<IToolStripButtonBackend, ToolStripButtonBackend> ();
            RegisterBackend<IToolStripDropDownButtonBackend, ToolStripDropDownButtonBackend> ();
            RegisterBackend<IToolStripSeparatorBackend, ToolStripSeparatorBackend> ();

            RegisterBackend<IToolStripItemHostBackend, ToolStripItemHostBackend> ();
            RegisterBackend<IComboBoxBackend, ComboBoxBackend> ();

            RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend>();
            RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend>();
            RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend> ();
            RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend>();

            RegisterBackend<ILayoutToolStripBackend0, LayoutToolStripBackend0>();
            

            RegisterBackend<ICanvasVidgetBackend, CanvasVidgetBackend>();

            RegisterBackend<IOpenfileDialogBackend, OpenFileDialogBackend>();
            RegisterBackend<ISavefileDialogBackend, SaveFileDialogBackend>();

            RegisterBackend<IVindowBackend, VindowBackend> ();
        }
    }
}