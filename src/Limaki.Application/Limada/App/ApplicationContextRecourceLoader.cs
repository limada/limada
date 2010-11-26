/*
 * Limada
 * Version 0.08
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


using Limada.Model;
using Limada.View;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Widgets;

namespace Limada.App {
    /// <summary>
    /// the concrete ApplicationContextRecourceLoader for limada-application
    /// which uses a DataBaseHandler, a Marker, and a GraphMapping for IWidget, IThing-Graphs
    /// </summary>
    public class ApplicationContextRecourceLoader : Limaki.Context.ApplicationContextRecourceLoader {
        public override void ApplyResources(IApplicationContext context) {
            base.ApplyResources (context);

            context.Factory.Add<IDataBaseHandler, DataBaseHandler> ();
            context.Factory.Add<ISheetManager, SheetManager>();
            GraphMapping.ChainGraphMapping<WidgetThingGraphMapping> (context);

            MarkerContextProcessor markerProcessor =
                context.Pool.TryGetCreate<MarkerContextProcessor> ();
            markerProcessor.CreateMarkerFacade = this.MarkerFacade;

           
        }

        public virtual IMarkerFacade<IWidget, IEdgeWidget> MarkerFacade(IGraph<IWidget, IEdgeWidget> graph) {
            
            if (new GraphPairFacade<IWidget, IEdgeWidget>()
                    .Source<IThing, ILink>(graph) != null) {

                return new WidgetThingMarkerFacade(graph);
            }

            return null;
        }


    }
}