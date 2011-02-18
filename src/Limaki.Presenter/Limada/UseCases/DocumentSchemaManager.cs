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
using System.Linq;

namespace Limada.UseCases {
    public class DocumentSchemaManager {
        public IEnumerable<IThing> Pages(IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            var docSchema = new DocumentSchema(source.ThingGraph(), source.ThingOf(widget));
            return docSchema.Pages();
        }

        public IEnumerable<StreamInfo<Stream>> PageStreams(IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            var docSchema = new DocumentSchema(source.ThingGraph(),source.ThingOf(widget));
            return docSchema.PageStreams();
        }

        public bool HasPages(IGraph<IWidget, IEdgeWidget> source, IWidget widget) {
            var docSchema = new DocumentSchema(source.ThingGraph(),source.ThingOf(widget));
            return docSchema.HasPages();
        }
    }
}