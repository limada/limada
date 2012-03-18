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
using Limaki.Context;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.Painters;
using Limaki.Drawing.Shapes;
using Limaki.Viewers;
using Limaki.View.UI;
using Limaki.View.Winform.Display;
using Limaki.Visuals;
using Limaki.Winform.Controls;
using Xwt.WinformBackend;

namespace Limaki.View.Winform {
    /// <summary>
    /// ApplicationContextRecourceLoader to use
    /// System.Windows.Forms
    /// </summary>
    public class WinformContextRecourceLoader : IDeviceContextRecourceLoader {

        public virtual void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader ().ApplyResources (context);

            new SwfEngine().RegisterBackends();
            Xwt.Engine.WidgetRegistry.RegisterBackend(
                typeof (Xwt.Drawing.SystemColors),
                typeof (Xwt.Gdi.Backends.SystemColorsBackend)
                );

            context.Factory.Add<IExceptionHandler,WinformExeptionHandler>();
            context.Factory.Add<IDrawingUtils,GDIDrawingUtils>();
            context.Factory.Add<ISystemFonts, GDISystemFonts>();
            context.Factory.Add<IPainterFactory, PainterFactory>();

            context.Factory.Add<IUISystemInformation, WinformSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory,VisualFactory>();
            
            context.Factory.Add<IDeviceCursor, CursorHandler>();
            context.Factory.Add<IDisplay<IGraphScene<IVisual, IVisualEdge>>>(() => new WinformVisualsDisplay().Display);
            context.Factory.Add<IMessageBoxShow, MessageBoxShow>();

            new ViewContextRecourceLoader().ApplyResources(context);

            
        }
        



    }
}

