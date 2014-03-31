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

using Limada.Usecases;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Drawing.WpfBackend;
using Limada.UseCases;
using Limada.View.Vidgets;
using Limaki.Usecases;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.WpfBackend;
using Limaki.View.XwtBackend;
using Xwt;
using Xwt.Backends;
using Limaki.View.Properties;

namespace Limaki.View.WpfBackend {

    public class WpfContextResourceLoader : ContextResourceLoader, IToolkitAware {

        public Xwt.ToolkitType ToolkitType {
            get { return Xwt.ToolkitType.Wpf; }
        }

        public override void ApplyResources(IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<SystemColorsBackend, WpfSystemColorsBackend>();
            tk.RegisterBackend<SystemFontBackend, WpfSystemFontBackend> ();
            context.Factory.Add<IUISystemInformation, WpfSystemInformation>();

            Iconery.DefaultSize = new Size (21,21);
            Iconery.Compose ();

            var factories = context.Pooled<UsecaseFactories<ConceptUsecase>>();
            factories.Add(new WpfUsecaseFactory());

            // register special IVidgetBackends here, eg. webbrowser
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ILayoutToolStripBackend, LayoutToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend>();

            WpfBackendHelper.ListenClipboard();
        }


        
    }
}