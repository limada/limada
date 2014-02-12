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

using System.Collections.Generic;
using System.IO;
using Limada.Model;
using Limada.Schemata;
using Limaki.Contents;
using Limaki.Graphs;
using Limada.View;
using Limada.VisualThings;
using Limaki.Model.Content;
using System.Linq;
using Limaki.Visuals;
using Limaki.Contents.IO;
using Limaki.Drawing;

namespace Limada.Usecases {

    public class DigidocViz {

        public IEnumerable<IVisual> Pages(IGraph<IVisual, IVisualEdge> source, IVisual document) {
            var digidoc = new DigidocSchema(source.ThingGraph(), source.ThingOf(document));
            return digidoc.Pages().Select(t => source.VisualOf(t));
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
                    result = ThingContentFacade.ConentOf(pageThing);
                } finally {
                    pageThing.ClearRealSubject(true);
                }
            }
            return result;
        }

        public bool HasPages(IGraph<IVisual, IVisualEdge> source, IVisual visual) {
            return new DigidocSchema(source.ThingGraph(), source.ThingOf(visual)).HasPages();
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

            if (HasPages(graph, digidocVisual)) {
                int i = 0;
                foreach (var streamThing in PageStreams(graph, digidocVisual)) {
                    var pageName = i.ToString().PadLeft(5, '0');
                    if (streamThing.Description != null)
                        pageName = streamThing.Description.ToString().PadLeft(5, '0');

                    var s = source.Cursor.Data == null ? CommonSchema.NullString : source.Cursor.Data.ToString();
                    var name = dir + Path.DirectorySeparatorChar +
                               s + " " +
                               pageName +
                               ContentTypes.Extension(streamThing.ContentType);

                    streamThing.Data.Position = 0;
                    using (var fileStream = new FileStream(name, FileMode.Create)) {
                        var buff = new byte[streamThing.Data.Length];
                        streamThing.Data.Read(buff, 0, (int)streamThing.Data.Length);
                        fileStream.Write(buff, 0, (int)streamThing.Data.Length);
                        fileStream.Flush();
                        fileStream.Close();
                    }
                    streamThing.Data.Dispose();
                    streamThing.Data = null;
                }
            }
        }
    }
}