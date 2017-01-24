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
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visualizers.Toolbars;
using Limaki.View.Viz.Visuals;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class SwfVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {
            
            base.InitializeBackends();

            RegisterBackend<ISdImageDisplayBackend, SdImageDisplayBackend>();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend>();

            RegisterBackend<ITextViewerVidgetBackend, TextViewerBackend>();
            RegisterBackend<ITextViewerWithToolstripVidgetBackend0, TextViewerWithToolstripBackend0>();
            RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();
            RegisterBackend<IMarkdownEditBackend, MarkdownEditBackend> ();
            RegisterBackend<IPlainTextBoxVidgetBackend, PlainTextBoxBackend> ();

            RegisterBackend<ISplitViewBackend, SplitViewBackend>();
            RegisterBackend<ITextOkCancelBoxBackend, TextOkCancelBoxBackend> ();
            RegisterBackend<IDigidocViewerBackend, DigidocViewerBackend>();

            RegisterBackend<IToolbarPanelBackend, XwtBackend.ToolbarPanelBackendDummy> ();
            RegisterBackend<IToolbarBackend, ToolbarBackend> ();
            RegisterBackend<IToolbarButtonBackend, ToolbarButtonBackend> ();
            RegisterBackend<IToolbarDropDownButtonBackend, ToolbarDropDownButtonBackend> ();
            RegisterBackend<IToolbarSeparatorBackend, ToolbarSeparatorBackend> ();

            RegisterBackend<IToolbarItemHostBackend, ToolbarItemHostBackend> ();
            RegisterBackend<IComboBoxBackend, ComboBoxBackend> ();

            RegisterBackend<IArrangerToolbarBackend, ArrangerToolbarBackend>();
            RegisterBackend<IDisplayModeToolbarBackend, DisplayModeToolbarBackend>();
            RegisterBackend<IMarkerToolbarBackend, MarkerToolbarBackend> ();
            RegisterBackend<ISplitViewToolbarBackend, SplitViewToolbarBackend>();
            RegisterBackend<ILayoutToolbarBackend, LayoutToolbarBackend> ();

            RegisterBackend<ICanvasVidgetBackend, CanvasVidgetBackend>();

            RegisterBackend<IOpenfileDialogBackend, OpenFileDialogBackend>();
            RegisterBackend<ISavefileDialogBackend, SaveFileDialogBackend>();

            RegisterBackend<IVindowBackend, VindowBackend> ();
        }
    }
}