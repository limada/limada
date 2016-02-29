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
using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.XwtBackend;
using Limaki.View.DragDrop;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.XwtBackend;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.XwtBackend {

    public class XwtContextResourceLoader : IBackendContextResourceLoader, IToolkitAware {

        public static readonly Guid ToolkitGuid = new Guid ("312266D8-76FA-4C3B-B4C9-39F7B04EEE3F");
        public static readonly Guid GtkToolkitGuid = new Guid ("36FB195F-4AAA-4353-8A06-E792360EE63C");
        public static readonly Guid WpfToolkitGuid = new Guid ("594AACED-2959-4192-AE9C-0A7ED9D45793");

        public virtual Guid ToolkitType {
            get { return ToolkitGuid; }
        }

        public virtual void ApplyXwtResources (IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<SystemColorsBackend, XwtSystemColorsBackend>();
            tk.RegisterBackend<SystemFontBackend, XwtSystemFontBackend> ();

            context.Factory.Add<IExceptionHandler, XwtExeptionHandlerBackend>();
            context.Factory.Add<IDrawingUtils, XwtDrawingUtils>();
            context.Factory.Add<IPainterFactory, DefaultPainterFactory>();

            context.Factory.Add<IUISystemInformation, XwtSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory, VisualFactory>();

            context.Factory.Add<ICursorHandler, XwtCursorHandlerBackend> ();
            context.Factory.Add<IMessageBoxShow, XwtMessageBoxShow> ();
            context.Factory.Add<IProgressHandler, ProgressHandler>();

            context.Factory.Add<IWebBrowserBackend, WebViewBackend> ();

            context.Factory.Add<IMarkdownEdit, MarkdownEdit> ();

            context.Factory.Add<IXwtConceptUseCaseComposer, XwtConceptUseCaseComposer>();
        }

        public virtual void ApplyResources (IApplicationContext context) {

            new LimakiCoreContextResourceLoader().ApplyResources(context);

            ApplyXwtResources(context);

            new ViewContextResourceLoader().ApplyResources(context);

            RegisterBackends(context);
            RegisterDragDropFormats(context);
        }

        public virtual void RegisterBackends (IApplicationContext context) {
            var engine = new XwtVidgetToolkitEngineBackend();
            engine.Initialize();
            VidgetToolkit.CurrentEngine = new VidgetToolkit { Backend = engine };
        }

        [TODO]
        public virtual void RegisterDragDropFormats (IApplicationContext context) {

			Registry.Factory.Add<IDragDropBackendHandler> (args => new DragDropBackendHandler (args [0] as IVidgetBackend));

			var man = Registry.Pooled<TransferDataManager> ();
			man.RegisterSome ();
			man.TransferContentTypes ["html format"] = ContentTypes.HTML;
			man.TransferContentTypes ["rich text format"] = ContentTypes.RTF;
			man.TransferContentTypes ["DeviceIndependentBitmap"] = ContentTypes.DIB;

			// TODO: register the others
			//man.TransferContentTypes.Add(System.Windows.Forms.DataFormats.UnicodeText, 0);

		}
    }
}
