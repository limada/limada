/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 */


using Limada.View;
using Limaki.Common.IOC;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Drawing.XwtBackend;
using Limaki.Usecases;
using Limaki.View.Viz;
using Limaki.View.Viz.Visuals;

namespace Limaki.View {

    public class ViewContextResourceLoader : IContextResourceLoader {

        public virtual void ApplyResources (IApplicationContext context) {

            if (!context.Factory.Contains<IShapeFactory>())
                context.Factory.Add<IShapeFactory, Limaki.Drawing.Shapes.ShapeFactory>();
            
            if (!context.Factory.Contains<IPainterFactory>())
                context.Factory.Add<IPainterFactory, DefaultPainterFactory>();

            var styleSheets = context.Pooled<StyleSheets>();
            styleSheets.Compose();

            new VisualsResourceLoader().ApplyResources(context);
            new DisplayResourceLoader ().ApplyResources (context);

            new VisualEntityResourceLoader ().ApplyResources (context);

            new VisualThingsResourceLoader ().ApplyResources (context);

            new ViewContentResourceLoader ().ApplyResources (context);

        }

    }
}