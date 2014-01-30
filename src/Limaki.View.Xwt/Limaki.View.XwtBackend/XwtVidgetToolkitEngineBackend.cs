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

using Limaki.View.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.Viewers;
using Limaki.Viewers.StreamViewers;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Viewers.Vidgets;
using Xwt.Drawing;
using Limaki.View.XwtBackend;

namespace Limaki.View.XwtBackend {

    public class XwtVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {

            base.InitializeBackends();

            RegisterBackend<IImageDisplayBackend, ImageDisplayBackend>();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend>();

            RegisterBackend<IOpenfileDialogBackend, OpenFileDialogBackend>();
            RegisterBackend<ISavefileDialogBackend, SaveFileDialogBackend>();

            RegisterBackend<ISplitViewBackend, SplitViewBackend>();

            RegisterBackend<ITextViewerBackend, TextViewerBackend>();
            RegisterBackend<ITextViewerWithToolstripBackend, TextViewerWithToolstripBackend>();
            RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();

            RegisterBackend<IDigidocViewerBackend, DigidocViewerBackend>();

            RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend>();
            RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend>();
            RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend>();
            RegisterBackend<ILayoutToolStripBackend, LayoutToolStripBackend>();
            RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend>();


        }
    }

   
}