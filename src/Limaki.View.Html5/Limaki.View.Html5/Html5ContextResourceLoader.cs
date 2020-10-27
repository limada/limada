﻿/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.XwtBackend;
using Limaki.View;
using Limaki.View.Visuals;
using Xwt;
using Xwt.Html5.Backend;
using Limaki.Common.IOC;
using Limaki.Drawing.Shapes;
using Limaki.Drawing.Painters;
using Xwt.Backends;


namespace Limaki.View.Html5 {

    public class Html5ContextResourceLoader : IBackendContextResourceLoader {

        public virtual void RegisterXwtBackends (bool asGuest) {
            var tk = Toolkit.CreateToolkit<Html5Engine>(asGuest);
            if (!asGuest) {
                tk.SetActive();
                Xwt.Application.Initialize (tk.Type,tk.Backend);
            }
            tk.RegisterBackend<SystemColorsBackend, HtmlSystemColorsBackend>();
            tk.RegisterBackend<SystemFontBackend, Html5SystemFontBackend>();
        }

        public virtual void ApplyHtml5Resources (IApplicationContext context) {
            
            RegisterXwtBackends(false);

            context.Factory.Add<IExceptionHandler, Html5ExeptionHandlerBackend> ();
            context.Factory.Add<IDrawingUtils, Html5DrawingUtils> ();
            context.Factory.Add<IUISystemInformation, Html5SystemInformation> ();

            context.Factory.Add<IPainterFactory, DefaultPainterFactory> ();
            context.Factory.Add<IShapeFactory, ShapeFactory> ();
            context.Factory.Add<IVisualFactory, VisualFactory> ();

            //context.Factory.Add<ICursorHandler, CursorHandlerBackend> ();
            //context.Factory.Add<IDisplay<IGraphScene<IVisual, IVisualEdge>>> (() => new Html5VisualsDisplayBackend ().Display);
            //context.Factory.Add<IMessageBoxShow, MessageBoxShow> ();
        }

        public virtual void ApplyResources (IApplicationContext context) {
            
            new LimakiCoreContextResourceLoader ().ApplyResources (context);

            ApplyHtml5Resources (context);
            
            new ViewContextResourceLoader ().ApplyResources (context);


        }




    }
}