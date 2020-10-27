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
using System.Diagnostics;
using Limada.Usecases;
using Limaki.Contents.IO;
using Limaki.Usecases;
using Limaki.View.Common;

namespace Limaki.View.XwtBackend {

    public class XwtContextResourceLoader : IBackendContextResourceLoader, IToolkitAware {

	    public virtual Guid ToolkitType => LimakiViewGuids.ToolkitGuid;

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

            context.Factory.Add<IXwtBackendConceptUseCaseComposer, XwtConceptUseCaseComposer>();
			context.Factory.Add<IBackendConceptUseCaseComposer, XwtConceptUseCaseComposer> ();
        }

        public virtual void ApplyResources (IApplicationContext context) {

            new LimakiCoreContextResourceLoader().ApplyResources(context);

            ApplyXwtResources(context);

            new ViewContextResourceLoader().ApplyResources(context);

            RegisterBackends(context);
            RegisterDragDropFormats(context);
        }

        public virtual void RegisterBackends (IApplicationContext context) {
			
			var backend = default(VidgetToolkitEngineBackend);
			var currentEngine = VidgetToolkit.CurrentEngine;
			if (currentEngine != null) {
				Trace.WriteLine (string.Format ("Warning\t{0} has already an engine: {1}", typeof(VidgetToolkit).Name, VidgetToolkit.CurrentEngine.GetType ().Name));
				if (currentEngine is VidgetToolkit) {
					Trace.WriteLine (string.Format ("\ttake current engine {0}", currentEngine.GetType ().Name));
				}
				if (currentEngine.Backend != null) {
					Trace.WriteLine (string.Format ("Warning:\t{0} has already a backend: {1}", 
						currentEngine.GetType ().Name, 
						currentEngine.Backend.GetType ().Name));
					backend = currentEngine.Backend;
					if (backend is XwtVidgetToolkitEngineBackend) {
						Trace.WriteLine (string.Format ("\ttake current engine backend {0}", backend.GetType ().Name));
					}
				}
			}

			if (backend == null) {
				backend = new XwtVidgetToolkitEngineBackend ();
				backend.Initialize ();
			}

			if (currentEngine == null) {
				currentEngine = new VidgetToolkit { Backend = backend };
			}

			VidgetToolkit.CurrentEngine = currentEngine;
		}

        [TODO]
        public virtual void RegisterDragDropFormats (IApplicationContext context) {

			context.Factory.Add<IDragDropBackendHandler> (args => new DragDropBackendHandler (args [0] as IVidgetBackend));

			var man = context.Pooled<TransferDataManager> ();
			man.RegisterSome ();



		}
    }
}
