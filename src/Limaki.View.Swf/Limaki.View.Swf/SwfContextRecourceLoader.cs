/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.IOC;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.Drawing.Gdi.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Viewers.Vidgets;
using Limaki.View.Swf.Visualizers;
using Limaki.Viewers;
using Limaki.View.UI;
using Limaki.Visuals;
using Limaki.Swf.Backends;
using Xwt.WinformBackend;
using System;
using Xwt.Engine;
using Limaki.Swf.Backends.Viewers.Content;
using Limaki.Swf.Backends.TextEditor;
using Limaki.Viewers.StreamViewers;
using Limaki.View.Swf.Backends;
using Limaki.View.Visuals.Visualizers;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Swf.Backends.Viewers.ToolStrips;
using Limaki.Swf.Backends.Viewers;


namespace Limaki.View.Swf {

   
    /// <summary>
    /// ApplicationContextRecourceLoader to use
    /// System.Windows.Forms
    /// </summary>
    public class SwfContextRecourceLoader : IBackendContextRecourceLoader {

        public virtual void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader ().ApplyResources (context);

            new SwfEngine().InitializeRegistry(WidgetRegistry.MainRegistry);
            WidgetRegistry.MainRegistry.RegisterBackend(
                typeof (Xwt.Drawing.SystemColors),typeof (Xwt.Gdi.Backend.SystemColorsBackend)
            );

            context.Factory.Add<IExceptionHandler,SwfExeptionHandlerBackend>();
            context.Factory.Add<IProgressHandler, ProgressHandler>();
            context.Factory.Add<IDrawingUtils,GdiDrawingUtils>();
            context.Factory.Add<ISystemFonts, GdiSystemFonts>();
            context.Factory.Add<IPainterFactory, PainterFactory>();

            context.Factory.Add<IUISystemInformation, SwfSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory,VisualFactory>();
            
            context.Factory.Add<ICursorHandler, CursorHandlerBackend>();
            context.Factory.Add<IMessageBoxShow, MessageBoxShow>();

            context.Factory.Add<IDisplay<System.Drawing.Image>, ImageDisplay>();
            context.Factory.Add<IDisplay<IGraphScene<IVisual,IVisualEdge>>, VisualsDisplay>();

            context.Factory.Add<IWebBrowserBackend>(() => CreateWebBrowserBackend());

            new ViewContextRecourceLoader().ApplyResources(context);

            RegisterBackends(context);

        }

        public virtual void RegisterBackends (IApplicationContext context) {
            var engine = new SwfVidgetToolkitEngineBackend();
            engine.Initialize();
            VidgetToolkit.CurrentEngine = new VidgetToolkit { Backend = engine };
        }

        public static bool GeckoFailed = false;
        public IWebBrowserBackend CreateWebBrowserBackend () {
            Control _backend = null;
            if (GeckoFailed || OS.Mono || OS.IsWin64Process) { //(true){ //
                _backend = new WebBrowserBackend();
                GeckoFailed = true;
                Trace.WriteLine("No Gecko");
            } else {
                try {
                    var gecko = Activator.CreateInstance(
                       this.GetType().Assembly.FullName,
                       typeof(WebBrowserBackend).Namespace + ".GeckoWebBrowserBackend");
                    if (gecko != null)
                        _backend = (Control)gecko.Unwrap();
                    else
                        throw new Exception();
                } catch {
                    GeckoFailed = true;
                    return CreateWebBrowserBackend();
                }
                Thread.Sleep(0);
            }
            return _backend as IWebBrowserBackend;
        }
    }

    public class SwfVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {
            
            base.InitializeBackends();

            RegisterBackend<IImageDisplayBackend, ImageDisplayBackend>();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend>();

            RegisterBackend<ITextViewerBackend, TextViewerBackend>();
            RegisterBackend<ITextViewerWithToolstripBackend, TextViewerWithToolstripBackend>();
            RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();

            RegisterBackend<ISplitViewBackend, SplitViewBackend>();

            RegisterBackend<IDigidocViewerBackend, DigidocViewerBackend>();

            RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend>();
            RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend>();
            RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend>();
            RegisterBackend<ILayoutToolStripBackend, LayoutToolStripBackend>();
            RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend>();


        }
    }
}

