/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Limada.Model;
using Limada.Schemata;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Graphs;
using Limada.View.VisualThings;
using Limaki.View.Visuals;

namespace Limada.View.Vidgets {

    public class DigidocViz {

        public IEnumerable<IVisual> Pages(IGraph<IVisual, IVisualEdge> source, IVisual document) {
            var digidoc = new DigidocSchema(source.ThingGraph(), source.ThingOf(document));
            return digidoc.Pages().Select(t => source.VisualOf(t));
        }

        public IEnumerable<IVisual> Pages (GraphCursor<IVisual, IVisualEdge> source) {
            return Pages (source.Graph, source.Cursor);
        }

        public IEnumerable<Content<Stream>> PageStreams(IGraph<IVisual, IVisualEdge> source, IVisual document) {
            var digidoc = new DigidocSchema(source.ThingGraph(), source.ThingOf(document));
            return digidoc.PageStreams();
        }

        public Content<Stream> PageContent(IGraph<IVisual, IVisualEdge> source, IVisual page) {
            Content<Stream> result = null;
            var pageThing = source.ThingOf(page) as IStreamThing;
            if (pageThing != null ) {
                try {
                    result = pageThing.ContentOf();
                } finally {
                    pageThing.ClearRealSubject(true);
                }
            }
            return result;
        }

        public bool HasPages(IGraph<IVisual, IVisualEdge> source, IVisual visual) {
            return new DigidocSchema(source.ThingGraph(), source.ThingOf(visual)).HasPages();
        }

        public bool HasPages (GraphCursor<IVisual, IVisualEdge> source) {
            return HasPages (source.Graph, source.Cursor);
        }

        public bool IsPage(IGraph<IVisual, IVisualEdge> source, IVisual page) {
            var digidoc = new DigidocSchema(source.ThingGraph(), source.ThingOf(page));
            var pageThing = source.ThingOf(page) as IStreamThing;
            var info = new ImageContentSpot();
            return (pageThing != null && info.Supports(pageThing.StreamType));
        }

        public void ExportPages (string dir, GraphCursor<IVisual, IVisualEdge> source) {
            var graph = source.Graph;
            var digidocVisual = source.Cursor;
            var man = new StreamContentIoManager();
            if (HasPages(graph, digidocVisual)) {
                int i = 0;
                var s = source.Cursor.Data == null ? CommonSchema.NullString : source.Cursor.Data.ToString();

                foreach (var pageContent in PageStreams(graph, digidocVisual)) {
                    var info = man.GetContentInfo (pageContent);
                    var ext = info != null ? "." + info.Extension : "";
                    var pageName = i.ToString().PadLeft(5, '0');
                    if (pageContent.Description != null)
                        pageName = pageContent.Description.ToString().PadLeft(5, '0');

                    var name = dir + Path.DirectorySeparatorChar + s + " " + pageName + ext;

                    man.WriteSink (pageContent, new Uri(name));
                    pageContent.Data.Dispose();
                    pageContent.Data = null;
                }
            }
        }
    }
}