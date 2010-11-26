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


using System;
using System.Reflection;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.Painters;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using Limaki.View;
using Limaki.Widgets;
using Limaki.Winform;
using Limaki.Winform.Displays;
using Limaki.Drawing.Shapes;

namespace Limaki.Context {
    /// <summary>
    /// the concrete ApplicationContextRecourceLoader an application
    /// which uses StyleSheets, a WidgetDisplay and IGraphMapping
    /// </summary>
    public class WinformContextRecourceLoader : Common.ContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader ().ApplyResources (context);


            context.Factory.Add<IExceptionHandler,WinformExeptionHandler>();
            context.Factory.Add<IPainterFactory, PainterFactory>();
            context.Factory.Add<IDrawingUtils,GDIDrawingUtils>();
            context.Factory.Add<ISystemFonts, GDISystemFonts>();


            context.Factory.Add<IUISystemInformation, WinformSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IWidgetFactory,WidgetFactory>();

            new ViewContextRecourceLoader().ApplyResources(context);

            context.Factory.Add<ICursorHander, CursorHandler> ();
            context.Factory.Add<ISelectionPainter, SelectionPainter> ();
            context.Factory.Add<ISelectionShapePainter, SelectionShapePainter>();
            context.Factory.Add<DisplayContextProcessor<Scene>, WidgetDisplayContextProcessor>();
        }
        



    }
}

