/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Context;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Presenter.UI;
using Limaki.Widgets;
using Limaki.Presenter.Winform.Display;

namespace Limaki.Presenter.Winform {
    /// <summary>
    /// the concrete ApplicationContextRecourceLoader an application
    /// which uses StyleSheets, a WidgetDisplay and IGraphMapping
    /// </summary>
    public class WinformContextRecourceLoader : IDeviceContextRecourceLoader {
        public virtual void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader ().ApplyResources (context);


            context.Factory.Add<IExceptionHandler,WinformExeptionHandler>();
            context.Factory.Add<IDrawingUtils,GDIDrawingUtils>();
            context.Factory.Add<ISystemFonts, GDISystemFonts>();
            context.Factory.Add<IPainterFactory, PainterFactory>();

            context.Factory.Add<IUISystemInformation, WinformSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IWidgetFactory,WidgetFactory>();
            
            context.Factory.Add<IDeviceCursor, CursorHandler>();
            context.Factory.Add<IDisplay<IGraphScene<IWidget, IEdgeWidget>>>(() => new WinformWidgetDisplay().Display);

            new ViewContextRecourceLoader().ApplyResources(context);

            
        }
        



    }
}

