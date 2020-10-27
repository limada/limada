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
using Limada.View.Vidgets;
using Limaki.Common.IOC;
using Limaki.Usecases;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.Toolbars;
using Limaki.View.XwtBackend;
using System.Diagnostics;
using Xwt;
using Xwt.Backends;
using System;
using Limaki.View.DragDrop;
using Limaki.Contents;

namespace Limaki.View.GtkBackend {

    public class GtkContextResourceLoader : ContextResourceLoader, IToolkitAware {

        public static readonly Guid ToolkitGuid =
#if XWT_GTK3
        LimakiViewGuids.Gtk3ToolkitGuid;
#else
        LimakiViewGuids.GtkToolkitGuid;
#endif
       
        public virtual Guid ToolkitType {
            get { return ToolkitGuid; }
        }

		protected static bool Applied { get; set; } 

        public override void ApplyResources (IApplicationContext context) {

			if (Applied)
				return;

            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<ITextEntryBackend, Xwt.GtkBackend.TextEntryMultiLineBackend>();


            context.Factory.Add<IUISystemInformation, GtkSystemInformation> ();

            var factories = context.Pooled<UsecaseFactories<ConceptUsecase>> ();
            factories.Add (new GtkUsecaseFactory ());

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolbarPanelBackend, ToolbarPanelBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolbarBackend, ToolbarBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolbarItemHostBackend, ToolbarItemHostBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolbarButtonBackend, ToolbarButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolbarDropDownButtonBackend, ToolbarDropDownPopupButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolbarSeparatorBackend, ToolbarSeparatorBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IArrangerToolbarBackend, ArrangerToolbarBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IDisplayModeToolbarBackend, DisplayModeToolbarBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IMarkerToolbarBackend, MarkerToolbarBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ISplitViewToolbarBackend, SplitViewToolbarBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ILayoutToolbarBackend, LayoutToolbarBackend> ();
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

            RegisterDragDropFormats (context);
        }

        public virtual void RegisterDragDropFormats (IApplicationContext context) { 
            var man = context.Pooled<TransferDataManager> ();

            var imageFormats = GtkPrototypes.ImageFormats.ListImageFormats ();
        }
    }
}