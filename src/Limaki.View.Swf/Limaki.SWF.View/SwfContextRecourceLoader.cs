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
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.IOC;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.Drawing.Gdi.Painters;
using Limaki.Drawing.Shapes;
using Limaki.View.Swf.Display;
using Limaki.Viewers;
using Limaki.View.UI;
using Limaki.Visuals;
using Limaki.Swf.Backends;
using Xwt.WinformBackend;

namespace Limaki.View.Swf {
    /// <summary>
    /// ApplicationContextRecourceLoader to use
    /// System.Windows.Forms
    /// </summary>
    public class SwfContextRecourceLoader : IBackendContextRecourceLoader {

        public virtual void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader ().ApplyResources (context);

            new SwfEngine().RegisterBackends();
            Xwt.Engine.WidgetRegistry.RegisterBackend(
                typeof (Xwt.Drawing.SystemColors),typeof (Xwt.Gdi.Backend.SystemColorsBackend)
            );

            context.Factory.Add<IExceptionHandler,SwfExeptionHandlerBackend>();
            context.Factory.Add<IDrawingUtils,GdiDrawingUtils>();
            context.Factory.Add<ISystemFonts, GdiSystemFonts>();
            context.Factory.Add<IPainterFactory, PainterFactory>();

            context.Factory.Add<IUISystemInformation, SwfSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory,VisualFactory>();
            
            context.Factory.Add<IBackendCursor, CursorHandler>();
            context.Factory.Add<IDisplay<IGraphScene<IVisual, IVisualEdge>>>(() => new SwfVisualsDisplayBackend().Display);
            context.Factory.Add<IMessageBoxShow, MessageBoxShow>();

            new ViewContextRecourceLoader().ApplyResources(context);

            
        }
        



    }
}

