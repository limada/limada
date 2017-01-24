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
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visualizers.Toolbars;
using Limaki.View.Viz.Visuals;
using Xwt.Drawing;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {

            base.InitializeBackends();

            RegisterBackend<IVindowBackend, VindowBackend> ();

            RegisterBackend<IComboBoxBackend, ComboBoxBackend> ();

            RegisterBackend<IImageDisplayBackend, ImageDisplayBackend>();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend>();

            RegisterBackend<IOpenfileDialogBackend, OpenFileDialogBackend>();
            RegisterBackend<ISavefileDialogBackend, SaveFileDialogBackend>();

            RegisterBackend<ISplitViewBackend, SplitViewBackend>();
            RegisterBackend<ITextOkCancelBoxBackend, TextOkCancelBoxBackend> ();

            RegisterBackend<ITextViewerVidgetBackend, TextViewerBackend>();
            RegisterBackend<ITextViewerWithToolstripVidgetBackend0, TextViewerWithToolstripBackend0>();
            RegisterBackend<IWebBrowserBackend, WebViewBackend>();
            RegisterBackend<IMarkdownEditBackend, MarkdownEditBackend> ();
            RegisterBackend<IPlainTextBoxVidgetBackend, PlainTextBoxBackend> ();

            RegisterBackend<IDigidocViewerBackend, DigidocViewerBackend> ();

            RegisterBackend<IArrangerToolbarBackend, ArrangerToolbarBackend>();
            RegisterBackend<IDisplayModeToolbarBackend, DisplayModeToolbarBackend>();
            RegisterBackend<ISplitViewToolbarBackend, SplitViewToolbarBackend>();
            RegisterBackend<ILayoutToolbarBackend, LayoutToolbarBackend> ();
            RegisterBackend<IMarkerToolbarBackend, MarkerToolbarBackend>();

            RegisterBackend<IToolbarPanelBackend,ToolbarPanelBackendDummy> ();
        }
    }

   
}