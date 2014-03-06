/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limada.Data;
using Limada.Model;
using Limada.Schemata;
using Limada.Usecases.Cms.Models;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Limada.Usecases.Cms {

    public class BackendController {

        IThingGraph _thingGraph = null;
        public virtual IThingGraph ThingGraph {
            get {
                if (_thingGraph == null) {
                    Open ();
                }
                return _thingGraph;
            }
            set {
                _thingGraph = value;
            }
        }

        public virtual Iori Iori { get; set; }
        public ThingGraphContent Current { get; set; }

        public void Open () {
            if (Current != null) {
                Trace.WriteLine (string.Format ("Provider already opened {0}", Current.Description));
                var conn = Current.Data as IGatewayConnection;
                if (conn != null) {
                    Trace.WriteLine (string.Format ("Connection already opened {0}/{1}", conn.Gateway.IsOpen (), Iori.ToFileName (conn.Gateway.Iori)));
                }
            } else {
                var ioManager = new ThingGraphIoManager { };
                var sinkIo = ioManager.GetSinkIO(Iori, IoMode.Read) as ThingGraphIo;
                try {
                    var sink = sinkIo.Open(Iori);
                    if (sink != null) {
                        Trace.WriteLine (string.Format ("DataBase opened {0}", Iori.ToFileName (Iori)));
                        Current = sink;
                        var graph = new SchemaThingGraph (Current.Data);
                        PrepareGraph (graph);
                        _thingGraph = graph;
                    } else {
                        throw new Exception ("Database not found: " + Iori.ToString ());
                    }
                } catch (Exception e) {
                    Trace.WriteLine (e.Message);
                    _thingGraph = new ThingGraph ();
                    Trace.WriteLine (string.Format ("Empty Graph created", Iori.ToFileName (Iori)));
                }
            }
        }

        protected void Close (ThingGraphContent data) {
            if (data == null)
                return;
            var sinkIo = new ThingGraphIoManager { }.GetSinkIO(data.ContentType, IoMode.Write) as ThingGraphIo;
            if (sinkIo != null)
                sinkIo.Close(data);
        }

        public void Close () {
            if (Current != null) {

                Close(Current);
                Trace.WriteLine (string.Format ("DataBase closed {0}", Iori.ToFileName (Iori)));
                Current = null;
            }
        }

        public void PrepareGraph (SchemaThingGraph graph) {
            var schema = new CmsSiteSchema ();
            graph.Initialize ();
            graph.Hiddens.Add (CommonSchema.SourceMarker.Id);
            graph.EdgeFilter = (link) => {
                if (link == null)
                    return false;
                var idLink = (ILink<long>) link;
                return graph.SchemaEdgeFilter (link) &&
                       !graph.Hiddens.Contains (idLink.Marker);
            };
            schema.EnsurceDefaultThings (graph);
        }

        public IThing Topic () {
            var source = this.ThingGraph;
            IThing topic = source.GetById (TopicSchema.Topics.Id);
            if (topic != null && (source.Edges (topic).Count > 0)) {
                var autoView = (
                                   from link in source.Edges (topic)
                                   where link.Marker.Id == TopicSchema.AutoViewMarker.Id
                                   select source.Adjacent (link, topic)
                               ).FirstOrDefault ();

                var streamThing = autoView as IStreamThing;
                if (streamThing != null && streamThing.StreamType == ContentTypes.LimadaSheet) {

                    //TODO: LoadSheet
                } else {
                    if (autoView != null) {
                        topic = autoView;
                    }
                }
            }
            return topic;
        }

        protected virtual string DisplayText (IThing thing) {
            var text = string.Empty;
            var toview = ThingDataToDisplay (thing);
            if (toview is string) {
                text = toview as string;
            }
            return text;
        }

        public object ThingDataToDisplay (IThing thing) {
            return ThingDataToDisplay (this.ThingGraph, thing);
        }

        public object ThingDataToDisplay (IGraph<IThing, ILink> graph, IThing thing) {
            if (thing == null)
                return CommonSchema.NullString;

            thing = ThingToDisplay (graph, thing);
            object result = null;
            if (thing is IProxy) {
                result = CommonSchema.ProxyString;
            } else {
                result = thing.Data;
            }

            if (thing is ILink)
                result = thing.ToString ();
            else if (thing.Id == CommonSchema.EmptyMarker.Id) {
                result = CommonSchema.EmtpyMarkerString;
            } else if (thing.GetType () == typeof (Thing))
                result = CommonSchema.ThingString;
            if (result == null)
                return CommonSchema.NullString;
            return result;
        }

        public IThing ThingToDisplay (IThing thing) {
            return ThingToDisplay(this.ThingGraph, thing);
        }

        public IThing ThingToDisplay (IGraph<IThing, ILink> graph, IThing thing) {
            if (graph is SchemaThingGraph) {
                return ((SchemaThingGraph) graph).ThingToDisplay (thing);
            } else {
                return thing;
            }
        }

        public IThing DescribedThing (IThing thing) {
            return DescribedThing (this.ThingGraph, thing);
        }

        public IThing DescribedThing (IGraph<IThing, ILink> graph, IThing thing) {
            if (graph is SchemaThingGraph) {
                return ((SchemaThingGraph) graph).DescribedThing (thing);
            } else {
                return thing;
            }
        }

        public virtual IEnumerable<IThing> ResolveRequest (string request) {
            if (string.IsNullOrEmpty(request)) {
                return new IThing[] { this.Topic () };
            } else {
                Int64 id = 0;
                if (Int64.TryParse (request, NumberStyles.HexNumber, null, out id)) {
                    var thing = ThingGraph.GetById (id);
                    return new IThing[] { thing };
                } else {
                    var things = ThingGraph.Search (request, false);
                    return things;
                }
            }
            return null;
        }

        public StreamContent SetMimeType (StreamContent content) {

            var contentIoPool = Registry.Pool.TryGetCreate<StreamContentIoPool> ();
            var contentIo = contentIoPool.Find (content.ContentType);

            content.MimeType = "unknown";
            if (contentIo != null) {
                var info = contentIo.Use (content.Data);
                if (info == null)
                    info = contentIo.Detector.Find (content.ContentType);
                if (info != null) {
                    content.MimeType = info.MimeType;
                    content.ContentType = info.ContentType;
                }
            }

            return content;
        }

        public StreamContent StreamContent (IStreamThing thing) {
            if (thing == null)
                return null;

            var result = new StreamContent (ThingContentFacade.ContentOf (ThingGraph, thing));
            result.Source = thing.Id.ToString ("X16");

            SetMimeType(result);

            if (result.ContentType == ContentTypes.TIF) {
                var sinkType = ContentTypes.PNG;
                var converter = Registry.Pool.TryGetCreate<ConverterPool<Stream>>()
                    .Find(ContentTypes.TIF, sinkType);

                if (converter != null) {
                    var conv = converter.Use(result, sinkType);
                    result.Data.Dispose();
                    result.Data = conv.Data;
                    result.ContentType = conv.ContentType;

                    SetMimeType (result);
                }
            }

            return result;
        }

        public static long[] ConvertibleHtmlStreamTypes = new long[] { ContentTypes.RTF, ContentTypes.HTML };
        public virtual bool IsConvertibleToHtml (IThing thing) {
            var streamThing = thing as IStreamThing;
            if (streamThing == null)
                return false;
            return ConvertibleHtmlStreamTypes.Contains (streamThing.StreamType);
        }

        /// <summary>
        /// gives back a content 
        /// where data is a html-string
        /// null if not supported
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        public virtual HtmlContent HtmlContent (IStreamThing thing) {

            if (!IsConvertibleToHtml (thing))
                return null;

            var result = new HtmlContent {
                Data = thing.Id.ToString ("X16"), // dummy value
                Description = ThingToDisplay (ThingGraph, thing).ToString (),
                ContentType = ContentTypes.HTML,
                Source = thing.Id.ToString ("X16"),
            };
            try {
                if (thing.StreamType == ContentTypes.RTF) {
                    var sinkType = ContentTypes.HTML;
                    var converter = Registry.Pool.TryGetCreate<ConverterPool<Stream>>()
                            .Find(thing.StreamType, sinkType);
                    if (converter != null) {
                        var source = ThingContentFacade.ContentOf (thing);
                        using (var reader = new StreamReader(converter.Use(source, sinkType).Data))
                            result.Data = reader.ReadToEnd();
                        source.Data.Dispose();
                    }
                } else if (thing.StreamType == ContentTypes.HTML) {
                    thing.DeCompress ();
                    var reader = new StreamReader (thing.Data);
                    result.Data = reader.ReadToEnd ();
                    thing.ClearRealSubject ();

                }
            } catch (Exception ex) {

#if DEBUG
                result.Data = "Error:" + ex.Message + "<br>" + ex.StackTrace;
#else
                result.Data = "Server überlastet. Bitte später nochmal probieren...";
#endif
                result.ContentType = ContentTypes.Unknown;

            }
            return result;
        }

        public string RenderSheet (IStreamThing streamThing) {
            return string.Empty;
        }

        /// <summary>
        /// an expandwalk with leafs of thing
        /// attention! use it only once, or make tolist or toarray
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        public IEnumerable<IThing> Leafs (IThing thing) {
            return new Walker<IThing, ILink> (ThingGraph)
                 .ExpandWalk (thing, 0, Walk.Leafs<IThing, ILink> ())
                 .Where (item => !(item.Node is ILink || item.Node == thing))
                 .Select (item => item.Node);
        }

        /// <summary>
        /// an expandwalk with roots of thing
        /// attention! use it only once, or make tolist or toarray
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        public IEnumerable<IThing> Roots (IThing thing) {
            return new Walker<IThing, ILink> (ThingGraph)
                .ExpandWalk (thing, 0, Walk.Roots<IThing, ILink> ())
                .Where (item => !(item.Node is ILink || item.Node == thing))
                .Select (item => item.Node);
        }

       public LinkID LinkOfThing (IThing t) {
           var d = ThingDataToDisplay (t);
           return new LinkID (d.ToString (), t.Id.ToString ("X16"));//DescribedThing (t).Id.ToString ("X16"));
       }

       public IEnumerable<LinkID> LinksOfThings (IEnumerable<IThing> things) {
           return things.Select (t => LinkOfThing (t));
       }


    }
}