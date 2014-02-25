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

using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Drawing.WpfBackend;
using Limaki.Usecases;
using Limaki.Usecases.Concept;
using Limaki.View.UI;
using Limaki.View.WpfBackends;
using Limaki.View.XwtContext;
using Limaki.Viewers;
using Limaki.Viewers.ToolStripViewers;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.WpfBackend {

    public class WpfContextRecourceLoader : ContextRecourceLoader, IToolkitAware {

        public Xwt.ToolkitType ToolkitType {
            get { return Xwt.ToolkitType.Wpf; }
        }

        public override void ApplyResources(IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<SystemColorsBackend, WpfSystemColorsBackend>();
            tk.RegisterBackend<SystemFontBackend, WpfSystemFontBackend> ();
            context.Factory.Add<IUISystemInformation, WpfSystemInformation>();

            var factories = context.Pool.TryGetCreate<UsecaseFactories<ConceptUsecase>>();
            factories.Add(new WpfUsecaseFactory());

            // register special IVidgetBackends here, eg. webbrowser
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ILayoutToolStripBackend, LayoutToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend>();
        }


        
    }
}