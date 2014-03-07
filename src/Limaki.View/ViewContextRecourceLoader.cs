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
using Limaki.View.Mesh;
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
using Limaki.View.Visuals.UI;
using Limaki.Model;
using Limaki.View.DragDrop;
using Limaki.Contents.IO;
using System.Linq;
using Limaki.Contents;

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

            var styleSheets = context.Pool.TryGetCreate<StyleSheets>();
            styleSheets.Compose();

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

            context.Factory.Add<IGraphSceneMesh<IVisual, IVisualEdge>, VisualGraphSceneMesh>();

            context.Factory.Add<GraphItemTransformer<IGraphEntity, IVisual, IGraphEdge, IVisualEdge>, GraphItem2VisualTransformer> ();
            context.Factory.Add<GraphItemTransformer<IVisual, IGraphEntity, IVisualEdge, IGraphEdge>, VisualGraphItemTransformer> ();

            context.Factory.Add<GraphItemTransformer<IVisual, IThing, IVisualEdge, ILink>, VisualThingTransformer> ();
            context.Factory.Add<GraphItemTransformer<IThing, IVisual, ILink, IVisualEdge>> (t => new VisualThingTransformer ().Reverted ());

            var mimeFingerPrints = Registry.Pool.TryGetCreate<MimeFingerPrints> ();
            mimeFingerPrints.SynonymFormats ("DeviceIndependentBitmap", new ImageContentSpot().ContentSpecs.First (s => s.ContentType == ContentTypes.DIB).MimeType);
            mimeFingerPrints.SynonymFormats ("CF_DIB", new ImageContentSpot ().ContentSpecs.First (s => s.ContentType == ContentTypes.DIB).MimeType);

            var contentDiggPool = Registry.Pool.TryGetCreate<ContentDiggPool> ();
            contentDiggPool.Add (new ImageContentDigger ());

            // TODO: find a better place for this
            var dependencies = Registry.Pool.TryGetCreate<GraphDepencencies<IVisual, IVisualEdge>>();
            dependencies.Visitor += (c, a, t) => new VisualThingsViz ().DependencyVisitor (c, a, t);

        }

        public virtual IMarkerFacade<IVisual, IVisualEdge> MarkerFacade (IGraph<IVisual, IVisualEdge> graph) {

            if (graph.Source<IVisual, IVisualEdge, IThing, ILink>() != null) {
                return new VisualThingMarkerFacade(graph);
            }

            return null;
        }
    }
}