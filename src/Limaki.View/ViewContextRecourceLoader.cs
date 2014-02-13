/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 */


using Limaki.Common.IOC;
using Limaki.Drawing.Styles;
using Limaki.Graphs.Extensions;
using Limada.Model;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.View.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.Visuals;
using Limaki.Drawing;
using Limaki.View.Visuals.Layout;
using Limaki.Common;
using Limaki.View.UI;
using System;
using Limaki.Viewers;
using Limada.Usecases;

namespace Limaki.View {

    public class ViewContextRecourceLoader : IContextRecourceLoader {
        /// <summary>
        /// Attention! Before calling this ResourceLoader, all DrawingFactories
        /// have to be loaded! 
        /// </summary>
        /// <param name="context"></param>
        public virtual void ApplyResources (IApplicationContext context) {

            if (!context.Factory.Contains<IShapeFactory>())
                context.Factory.Add<IShapeFactory, Limaki.Drawing.Shapes.ShapeFactory>();
            
            if (!context.Factory.Contains<IPainterFactory>())
                context.Factory.Add<IPainterFactory, Limaki.Drawing.Painters.DefaultPainterFactory>();

            if (!context.Factory.Contains<IVisualFactory>())
                context.Factory.Add<IVisualFactory, VisualFactory>();


            if (!context.Factory.Contains<IDrawingUtils>())
                context.Factory.Add<IDrawingUtils, BlindDrawingUtils>();

            if (!context.Factory.Contains<ISystemFonts>())
                context.Factory.Add<ISystemFonts, BlindSystemFonts>();

            if (!context.Factory.Contains<IUISystemInformation>())
                context.Factory.Add<IUISystemInformation, BlindSystemInformation>();


            var styleSheets = context.Pool.TryGetCreate<StyleSheets>();
            styleSheets.Init();

            GraphMapping.ChainGraphMapping<GraphVisualEntityMapping>(context);
            GraphMapping.ChainGraphMapping<VisualThingGraphMapping>(context);

            var markerProcessor =
                context.Pool.TryGetCreate<MarkerContextProcessor>();
            markerProcessor.CreateMarkerFacade = this.MarkerFacade;

            var displayRecourceLoader = new DisplayRecourceLoader();
            displayRecourceLoader.ApplyResources(context);

            var visualsRecourceLoader = new VisualsRecourceLoader();
            visualsRecourceLoader.ApplyResources(context);

            context.Factory.Add<ISheetManager, SheetManager>();
            context.Factory.Add<IGraphSceneLayout<IVisual, IVisualEdge>>(args =>
                     new VisualsSceneLayout<IVisual, IVisualEdge>(args[0] as Func<IGraphScene<IVisual, IVisualEdge>>, args[1] as IStyleSheet)
             );

            context.Factory.Add<IVisualContentViz, VisualThingsContentViz>();
            context.Factory.Add<IVisualContentViz<IThing>, VisualThingsContentViz>();

            // TODO: find a better place for this
            var dependencies = Registry.Pool.TryGetCreate<GraphDepencencies<IVisual, IVisualEdge>>();
            dependencies.Visitor += (c, a, t) => new DigidocViz().DependencyVisitor (c, a, t);

        }

        public virtual IMarkerFacade<IVisual, IVisualEdge> MarkerFacade (IGraph<IVisual, IVisualEdge> graph) {

            if (graph.Source<IVisual, IVisualEdge, IThing, ILink>() != null) {
                return new VisualThingMarkerFacade(graph);
            }

            return null;
        }
    }
}