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

using Limada.UseCases;
using Limada.View.Vidgets;
using Limaki.Common.IOC;
using Limaki.Drawing.WpfBackend;
using Limaki.Iconerias;
using Limaki.Usecases;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.XwtBackend;
using Xwt;
using Xwt.Backends;

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

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripBackend, ToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripButtonBackend, ToolStripButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripDropDownButtonBackend, ToolStripDropDownButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripItemHostBackend, ToolStripItemHostBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripSeparatorBackend, ToolStripSeparatorBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend>();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ILayoutToolStripBackend0, LayoutToolStripBackend>();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend>();

            WpfBackendHelper.ListenClipboard();

        }


        
    }
}