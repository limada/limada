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
            context.Factory.Add<IDisplay<IGraphScene<IVisual, IVisualEdge>>>(() => new VisualsDisplayBackend().Display);
            context.Factory.Add<IMessageBoxShow, MessageBoxShow>();

            if (!OS.IsWin64Process)
                context.Factory.Add<IGeckoWebBrowser>(() => {
                    // this is needed to avoid loading win86-Assembly:
                    var gecko = Activator.CreateInstance(
                        this.GetType().Assembly.FullName,
                        typeof(WebBrowserBackend).Namespace + ".GeckoWebBrowser");
                    if (gecko != null)
                        return (IGeckoWebBrowser)gecko.Unwrap();
                    return null;
                });
            
            context.Factory.Add<IWebBrowserBackend>(() => new WebBrowserBackend());

            new ViewContextRecourceLoader().ApplyResources(context);

            RegisterBackends(context);

        }

        public virtual void RegisterBackends (IApplicationContext context) {
            var engine = new SwfVidgetToolkitEngineBackend();
            engine.Initialize();
            VidgetToolkit.CurrentEngine = new VidgetToolkit { Backend = engine };
        }


    }

    public class SwfVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {
            
            base.InitializeBackends();

            RegisterBackend<IImageDisplayBackend, ImageDisplayBackend>();
            RegisterBackend<IDisplayBackend<IGraphScene<IVisual, IVisualEdge>>, VisualsDisplayBackend>();

            RegisterBackend<ITextViewerBackend, TextViewerBackend>();
            RegisterBackend<ITextViewerWithToolstripBackend, TextViewerWithToolstripBackend>();
            RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();
        }
    }
}

