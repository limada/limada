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

using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Limada.UseCases;
using Limada.View.Vidgets;
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing.WpfBackend;
using Limaki.Iconerias;
using Limaki.Usecases;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.XwtBackend;
using Xwt;
using Xwt.Backends;
using Size = Xwt.Size;

namespace Limaki.View.WpfBackend {

    public class WpfContextResourceLoader : ContextResourceLoader, IToolkitAware {

        public Xwt.ToolkitType ToolkitType {
            get { return Xwt.ToolkitType.Wpf; }
        }

        public override void ApplyResources (IApplicationContext context) {

            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<SystemColorsBackend, WpfSystemColorsBackend> ();
            tk.RegisterBackend<SystemFontBackend, WpfSystemFontBackend> ();
            tk.RegisterBackend<ITextViewerWidgetBackend, TextViewerWidgetBackend> ();
            tk.RegisterBackend<ITextViewerWithToolstripWidgetBackend, TextViewerWithToolStripWidgetBackend> ();

            context.Factory.Add<IUISystemInformation, WpfSystemInformation> ();

            Iconery.DefaultSize = new Size (21, 21);
            Iconery.Compose ();

            var factories = context.Pooled<UsecaseFactories<ConceptUsecase>> ();
            factories.Add (new WpfUsecaseFactory ());

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripBackend, ToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripButtonBackend, ToolStripButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripDropDownButtonBackend, ToolStripDropDownButtonBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripItemHostBackend, ToolStripItemHostBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IToolStripSeparatorBackend, ToolStripSeparatorBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ILayoutToolStripBackend0, LayoutToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend> ();
            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ITextViewerVidgetBackend, TextViewerVidgetBackend> ();

            VidgetToolkit.CurrentEngine.Backend.RegisterBackend<ITextViewerWithToolstripVidgetBackend, TextViewerWithToolStripVidgetBackend> ();

            WpfBackendHelper.ListenClipboard ();
            if (true) { // take GeckoWebBrowser
                tk.RegisterBackend<IWebBrowserWidgetBackend, GeckoWebBrowserBackend> ();
                WebBrowserCreatorFallback = context.Factory.Func<IWebBrowserBackend> ();
                context.Factory.Add<IWebBrowserBackend> (() => CreateWebBrowserBackend ());
            }

        }

        protected Func<IWebBrowserBackend> WebBrowserCreatorFallback { get; set; }
        public static bool GeckoFailed = false;
        public IWebBrowserBackend CreateWebBrowserBackend () {
            IWebBrowserBackend _backend = null;
            if (GeckoFailed || OS.Mono) { //(true) { //|| OS.IsWin64Process
                _backend = WebBrowserCreatorFallback();
                GeckoFailed = true;
                Trace.WriteLine ("No Gecko");
            } else {
                try {
                    _backend = new WebBrowserVidgetBackend ();
                    if (_backend == null)
                       throw new Exception ();
                } catch {
                    GeckoFailed = true;
                    return CreateWebBrowserBackend ();
                }
                Thread.Sleep (0);
            }
            return _backend;
        }



        
    }
}