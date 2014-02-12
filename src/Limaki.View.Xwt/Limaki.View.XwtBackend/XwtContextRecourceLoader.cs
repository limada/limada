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

using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.IOC;
using Limaki.Model.Content;
using Limaki.View.DragDrop;
using Limaki.View.UI;
using Limaki.View.XwtContext;
using Limaki.Viewers;
using Limaki.Visuals;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.XwtBackend {

    public class XwtContextRecourceLoader : IBackendContextRecourceLoader {

        public virtual void ApplyXwtResources (IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<SystemColorsBackend, XwtSystemColorsBackend>();

            context.Factory.Add<IExceptionHandler, XwtExeptionHandlerBackend>();
            context.Factory.Add<IDrawingUtils, XwtDrawingUtils>();
            context.Factory.Add<ISystemFonts, XwtSystemFonts>();

            context.Factory.Add<IPainterFactory, DefaultPainterFactory>();

            context.Factory.Add<IUISystemInformation, XwtSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory, VisualFactory>();

            context.Factory.Add<ICursorHandler, XwtCursorHandlerBackend> ();
            context.Factory.Add<IMessageBoxShow, XwtMessageBoxShow> ();
            context.Factory.Add<IProgressHandler, ProgressHandler>();

            context.Factory.Add<IXwtConceptUseCaseComposer, XwtConceptUseCaseComposer>();
        }

        public virtual void ApplyResources (IApplicationContext context) {

            new LimakiCoreContextRecourceLoader().ApplyResources(context);

            ApplyXwtResources(context);

            new ViewContextRecourceLoader().ApplyResources(context);

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

            Registry.Factory.Add<IDragDropBackendHandler>(args => new DragDropBackendHandler(args[0] as IVidgetBackend));

            var man = new TransferDataManager();
            man.RegisterSome();
            man.TransferContentTypes.Add("html format", ContentTypes.HTML);
            man.TransferContentTypes.Add("rich text format", ContentTypes.RTF);

            // TODO: register the others
            //man.TransferContentTypes.Add(System.Windows.Forms.DataFormats.UnicodeText, 0);

        }
    }
}
