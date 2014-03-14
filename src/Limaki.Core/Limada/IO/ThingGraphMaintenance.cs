/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.Model;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Contents;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Limada.IO {

    public class ThingGraphMaintenance {

        ContentInfos _contentInfoPool = null;
        protected ContentInfos ContentInfoPool { get { return _contentInfoPool ?? (_contentInfoPool = Registry.Pooled<ContentInfos> ()); } }
        protected IDictionary<long, CompressionType> _compressions = new Dictionary<long, CompressionType> ();

        public void RefreshCompression (IGraph<IThing, ILink> graph, IThing thing, bool act) {

            var streamThing = thing as IStreamThing;
            if (streamThing == null || streamThing.Compression != CompressionType.None)
                return;

            var comp = CompressionType.None;
            if (!_compressions.TryGetValue (streamThing.StreamType, out comp)) {
                var info = ContentInfoPool.FirstOrDefault (i => i.ContentType == streamThing.StreamType);
                if (info != null) {
                    comp = info.Compression;
                    _compressions[info.ContentType] = info.Compression;
                    WriteLog ("Comp for {0}: {1} | {2}", info.Description, comp, streamThing.StreamType.ToString ("X"));
                }
            }
            if (comp != CompressionType.None) {
                WriteLog ("{0} -> {1} |{2}:{3}", streamThing.Compression, comp, streamThing.Id.ToString ("X"), streamThing.StreamType.ToString ("X"));
                streamThing.DeCompress();
                var oldSize = streamThing.Data.Length;
                if (act) {
                    streamThing.Compression = comp;
                    if (comp != CompressionType.neverCompress) {
                        streamThing.Compress();
                        streamThing.Flush();
                        streamThing.ClearRealSubject ();
                        WriteLog ("\t{0} -> {1}", oldSize, streamThing.Data.Length);
                        
                    }
                    graph.Add (streamThing);
                }
            }
        }

        public void RefreshCompression (IGraph<IThing, ILink> graph, bool act) {
            WriteLog ("RefreshCompression, act: {0}", act);
            foreach(var thing in graph) {
                RefreshCompression (graph, thing, act);
            }
            Flush (graph);
            WriteLog ("RefreshCompression done");
        }

        public void Flush (IGraph<IThing, ILink> graph) {
            var dbGraph = (graph.Unwrap() as DbGraph<IThing, ILink>);
            if (dbGraph != null) {
                dbGraph.Flush ();
            }
        }

        public void WriteLog (string message, params object[] args) {
            Trace.WriteLine (string.Format (message, args));
        }


        /// <summary>
        /// search for all StringThings where link.Marker == Document && text = null or empty
        /// get the Title of things
        /// set text of target to title.text
        /// remove title
        /// set rootlinks.where(marker==Document) to  marker = CommonSchema.Commonmarker
        /// </summary>
        /// <param name="graph"></param>
        public virtual void CleanWrongDocuments (SchemaThingGraph graph, bool act) {


            graph.Add (CommonSchema.CommonMarker);
            var nullStringThings = graph.GetByData ((string)null);

            foreach (var nullStringThing in nullStringThings) {
                var edges = graph.Edges (nullStringThing).ToArray ();
                var disp = graph.ThingToDisplay (nullStringThing);

                if (disp != nullStringThing) {
                    WriteLog ("+\t{0}\t[{1}]", nullStringThing.Data ?? "<null>", nullStringThing.Id.ToString ("X"));
                    var titleLink = edges.FirstOrDefault (l => l.Marker.Id == DigidocSchema.DocumentTitle.Id);
                    if (titleLink != null && titleLink.Leaf == disp) {
                        WriteLog ("\t-\t{0}\t[{1}]", disp.Data ?? "<null>", disp.Id.ToString ("X"));
                        if (act) {
                            nullStringThing.Data = disp.Data;
                            graph.Add (nullStringThing);
                            graph.Remove (titleLink);
                            var dispEdges = graph.Edges (disp).ToArray ();
                            foreach (var link in dispEdges) {
                                graph.ChangeEdge (link, nullStringThing, link.Root == disp);
                                graph.Add (link);
                            }
                            graph.Remove (disp);
                        }
                    }
                    var documentLink = edges.FirstOrDefault (l => l.Marker.Id == DigidocSchema.Document.Id);
                    if (documentLink != null) {
                        WriteLog (string.Format ("\t<>\t{0}\t{1}", DigidocSchema.Document.Data, CommonSchema.CommonMarker.Data));
                        if (act) {
                            documentLink.Marker = CommonSchema.CommonMarker;
                            graph.Add (documentLink);
                        }
                    }
                } else {
                    //if (edges.Count() == 0)
                    //    WriteLog("--\t{0}\t[{1}]", disp.Data ?? "<null>", disp.Id.ToString("X"));
                    //else
                    //    WriteLog("-\t{0}\t[{1}]", disp.Data ?? "<null>", disp.Id.ToString("X"));

                }
            }
            Flush(graph);
        }


    }

}