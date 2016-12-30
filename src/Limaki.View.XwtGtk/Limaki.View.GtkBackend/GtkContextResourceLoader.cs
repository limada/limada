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
using System.Diagnostics;
using Xwt;
using Xwt.Backends;
using System;

namespace Limaki.View.GtkBackend {

    public class GtkContextResourceLoader : ContextResourceLoader, IToolkitAware {

        public Xwt.ToolkitType ToolkitType0 { get { return Xwt.ToolkitType.Gtk; } }

        public static readonly Guid ToolkitGuid = new Guid ("36FB195F-4AAA-4353-8A06-E792360EE63C");
       
        public virtual Guid ToolkitType {
            get { return ToolkitGuid; }
        }

		protected static bool Applied { get; set; } 

        public override void ApplyResources (IApplicationContext context) {

			if (Applied)
				return;

            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<ITextEntryBackend, Xwt.GtkBackend.TextEntryMultiLineBackend>();

            // prototype clipboard:
            tk.RegisterBackend<ClipboardBackend, GtkClipboardBackend1> ();
            tk.ClipboardBackend = new GtkClipboardBackend1 ();

            context.Factory.Add<IUISystemInformation, GtkSystemInformation> ();

            var factories = context.Pooled<UsecaseFactories<ConceptUsecase>> ();
            factories.Add (new GtkUsecaseFactory ());

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripBackend, ToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripItemHostBackend, ToolStripItemHostBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripButtonBackend, ToolStripButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripDropDownButtonBackend, ToolStripDropDownPopupButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripSeparatorBackend, ToolStripSeparatorBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ILayoutToolStripBackend, LayoutToolStripBackend> ();
#if WINDOWSBACKEND
            if (false) {
                var gtkEngine = Xwt.Backends.ToolkitEngineBackend.GetToolkitBackend<Xwt.GtkBackend.GtkEngine> ();
                // using Xwt.Gtk.Windows.WebViewBackend: not working on linux
                gtkEngine.RegisterBackend<IWebViewBackend, Xwt.Gtk.Windows.WebViewBackend> ();
                // hint: see GeckoWebBrowser.Gtk for Gtk-Binding of Winform
            }
#endif
            GLib.ExceptionManager.UnhandledException += (args) => {
                Trace.WriteLine (args.ToString ());
            };

			Applied = true;
        }

    }
}