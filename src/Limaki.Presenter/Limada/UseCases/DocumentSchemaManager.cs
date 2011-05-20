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
using Limaki.Visuals;
using System.Linq;
using Limaki.Model.Content;
using Limaki.Common;
using System.Linq;
using System;
using Limaki.Graphs.Extensions;
namespace Limada.UseCases {
    public class DocumentSchemaManager {
        public IEnumerable<IVisual> Pages(IGraph<IVisual, IVisualEdge> source, IVisual document) {
            var docSchema = new DocumentSchema(source.ThingGraph(), source.ThingOf(document));
            return docSchema.Pages().Select(t=>source.VisualOf(t));
        }

        public IEnumerable<StreamInfo<Stream>> PageStreams(IGraph<IVisual, IVisualEdge> source, IVisual document) {
            var docSchema = new DocumentSchema(source.ThingGraph(),source.ThingOf(document));
            return docSchema.PageStreams();
        }
        
        public StreamInfo<Stream> PageStream(IGraph<IVisual, IVisualEdge> source, IVisual page) {
            StreamInfo<Stream> result = null;
            var pageThing = source.ThingOf(page) as IStreamThing;
            var imageStreamProvider = new ImageStreamProvider();
            if(pageThing != null && imageStreamProvider.Supports(pageThing.StreamType)) {
                try {
                    result = ThingStreamFacade.GetStreamInfo(pageThing);
                } finally {
                    pageThing.ClearRealSubject(true);
                }
            }
            return result;
        }

        //public Stream StreamFrom(StreamInfo<Stream> streamInfo) {
        //    var result = streamInfo.Data;
        //    result.Position = 0;
        //    var compWorker = Registry.Factory.Create<ICompressionWorker>();
        //    if (compWorker.Compressable(streamInfo.Compression))
        //        try {
        //            result = compWorker.DeCompress(streamInfo.Data, streamInfo.Compression);
        //        } catch (Exception e) {
        //            result = null;
        //        }
        //    return result;
        //}

        public bool HasPages(IGraph<IVisual, IVisualEdge> source, IVisual visual) {
            var docSchema = new DocumentSchema(source.ThingGraph(),source.ThingOf(visual));
            return docSchema.HasPages();
        }

        public bool IsPage(IGraph<IVisual, IVisualEdge> source, IVisual page) {
            var docSchema = new DocumentSchema(source.ThingGraph(), source.ThingOf(page));
            var pageThing = source.ThingOf(page) as IStreamThing;
            var imageStreamProvider = new ImageStreamProvider();
            return (pageThing != null && imageStreamProvider.Supports(pageThing.StreamType));
        }
    }
}