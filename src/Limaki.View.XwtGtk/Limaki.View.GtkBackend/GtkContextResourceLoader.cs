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
using Limaki.Usecases;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.XwtBackend;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.GtkBackend {

    public class GtkContextResourceLoader : ContextResourceLoader, IToolkitAware {

        public override void ApplyResources (IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            //tk.RegisterBackend<SystemColorsBackend, XwtSystemColorsBackend>();

            context.Factory.Add<IUISystemInformation, GtkSystemInformation> ();

            var factories = context.Pooled<UsecaseFactories<ConceptUsecase>> ();
            factories.Add (new GtkUsecaseFactory ());

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripBackend, ToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripItemHostBackend, ToolStripItemHostBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripButtonBackend, ToolStripButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripDropDownButtonBackend, ToolStripDropDownButtonBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend> ();

            //VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend> ();
            //VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ILayoutToolStripBackend, LayoutToolStripBackend> ();
            


        }

        public Xwt.ToolkitType ToolkitType { get { return Xwt.ToolkitType.Gtk; } }
    }
}