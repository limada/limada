/*
 * Limada 
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
 */

using System.Collections.Generic;
using System.IO;
using Limada.Model;
using Limada.Schemata;
using Limada.View;
using Limaki.Graphs;
using Limaki.Model.Streams;
using Limaki.Widgets;

namespace Limada.App {
    public class DocumentSchemaManager {
        IEnumerable<IThing> Pages(IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            var document = WidgetThingGraphExtension.GetThing(source, widget);
            var graph = WidgetThingGraphExtension.GetThingGraph(source);
            if (document != null && graph != null)
                foreach (ILink link in graph.Edges(document)) {
                    if (link.Marker.Id == DocumentSchema.DocumentPage.Id) {
                        yield return graph.Adjacent(link, document);
                    }
                }
        }

        public IEnumerable<StreamInfo<Stream>> PageStreams(IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            var graph = WidgetThingGraphExtension.GetThingGraph(source) as SchemaThingGraph;
            foreach (IThing page in Pages(source, widget)) {
                if (page is IStreamThing) {
                    StreamInfo<Stream> info = ThingStreamFacade.GetStreamInfo(graph, page);
                    yield return info;
                }
            }
        }

        public bool HasPages(IGraph<IWidget, IEdgeWidget> graph, IWidget widget) {
            bool result = false;
            foreach (IThing page in Pages(graph, widget)) {
                result = true;
                break;
            }

            return result;
        }
    }
}