/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.View.GdiBackend;
using Limaki.Drawing.Shapes;
using Limaki.View;
using Limaki.View.DragDrop;
using Limaki.View.SwfBackend.Controls;
using Limaki.View.SwfBackend.DragDrop;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.SwfBackend.Viz;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visuals;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Xwt;
using Xwt.Backends;
using Limaki.View.Viz.Visualizers;
using Limada.Usecases;
using Limada.Usecases;
using Limaki.View.Common;

namespace Limaki.View.SwfBackend {

    /// <summary>
    /// IBackendContextResourceLoader to use
    /// System.Windows.Forms
    /// </summary>
    public class SwfContextResourceLoader : IBackendContextResourceLoader, IToolkitAware {
        
        // { 0xeb5d0ce9, 0xdc0, 0x4f1f, { 0x85, 0xcc, 0x2b, 0xf2, 0xa5, 0x13, 0x8a, 0x28 } };
        public static readonly Guid ToolkitGuid = new Guid ("EB5D0CE9-0DC0-4F1F-85CC-2BF2A5138A28");

        public virtual Guid ToolkitType { get { return ToolkitGuid; } }

        public Toolkit Toolkit { get; set; }

        public virtual void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextResourceLoader ().ApplyResources (context);

            Toolkit = Toolkit.CreateToolkit<Xwt.SwfBackend.SwfEngine>(false);
            Toolkit.RegisterBackend<Xwt.Backends.SystemColorsBackend, Xwt.GdiBackend.SystemColorsGdiBackend>();
            Toolkit.RegisterBackend<SystemFontBackend, GdiSystemFontBackend> ();

            Toolkit.SetActive();
            Xwt.Application.Initialize(Toolkit);

            context.Factory.Add<IExceptionHandler,SwfExeptionHandlerBackend>();
            context.Factory.Add<IProgressHandler, ProgressHandler>();
            context.Factory.Add<IDrawingUtils,GdiDrawingUtils>();
            

            context.Factory.Add<IUISystemInformation, SwfSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory,VisualFactory>();
            
            context.Factory.Add<ICursorHandler, CursorHandlerBackend>();
            context.Factory.Add<IMessageBoxShow, MessageBoxShow>();

            context.Factory.Add<IDisplay<System.Drawing.Image>, SdImageDisplay>();
            context.Factory.Add<IDisplay<Xwt.Drawing.Image>, ImageDisplay> ();
            context.Factory.Add<IDisplay<IGraphScene<IVisual,IVisualEdge>>, VisualsDisplay>();

            context.Factory.Add<IWebBrowserBackend>(() => CreateWebBrowserBackend());

            context.Factory.Add<IBackendConceptUseCaseComposer, SwfConceptUseCaseComposer> ();

            context.Factory.Add<IMarkdownEdit, MarkdownEdit> ();

            new ViewContextResourceLoader().ApplyResources(context);

            RegisterBackends(context);
            RegisterDragDropFormats(context);
        }

        public virtual void RegisterBackends (IApplicationContext context) {
            var engine = new SwfVidgetToolkitEngineBackend();
            engine.Initialize();
            VidgetToolkit.CurrentEngine = new VidgetToolkit { Backend = engine };
        }

        public static bool GeckoFailed = false;
        public IWebBrowserBackend CreateWebBrowserBackend () {
            IWebBrowserBackend _backend = null;
            if (GeckoFailed || OS.Mono) { //(true) { //|| OS.IsWin64Process
                _backend = new WebBrowserBackend();
                GeckoFailed = true;
                Trace.WriteLine("No Gecko");
            } else {
                try {
                    var gecko = Activator.CreateInstance(
                       this.GetType().Assembly.FullName,
                       typeof(WebBrowserBackend).Namespace + ".GeckoWebBrowserBackend");
                    if (gecko != null) {
                        var control = gecko.Unwrap ();
                        _backend = (IWebBrowserBackend) control;
                    } else
                        throw new Exception ();
                } catch {
                    GeckoFailed = true;
                    return CreateWebBrowserBackend();
                }
                Thread.Sleep(0);
            }
            return _backend;
        }

        public virtual void RegisterDragDropFormats (IApplicationContext context) {

            Registry.Factory.Add<IDragDropBackendHandler>(args => new DragDropBackendHandler(args[0] as IVidgetBackend));

            var man = Registry.Pooled<TransferDataManager>();
            man.MimeFingerPrints.SynonymFormats ("CF_DIB", "DeviceIndependentBitmap");

            man.RegisterSome();
            man.TransferContentTypes[System.Windows.Forms.DataFormats.Html.ToLower()] = ContentTypes.HTML;
            man.TransferContentTypes[System.Windows.Forms.DataFormats.Rtf.ToLower()] = ContentTypes.RTF;
            man.TransferContentTypes["DeviceIndependentBitmap"] = ContentTypes.DIB;

            // TODO: register the others
            //man.TransferContentTypes.Add(System.Windows.Forms.DataFormats.UnicodeText, 0);

        }
    }
}

