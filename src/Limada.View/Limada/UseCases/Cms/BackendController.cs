﻿/*
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

using Limada.IO;
using Limada.Model;
using Limada.Schemata;
using Limada.Usecases.Cms.Models;
using Limada.View;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Contents.IO;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.View;
using Limaki.View.Visuals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Limaki.Drawing.Styles;
using Limaki.View.Viz.Mapping;
using System.Net;
using Limaki.Contents.Text;

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
                Trace.WriteLine ($"Provider already opened {Current.Description}");
                var conn = Current.Data as IGatewayConnection;
                if (conn != null) {
                    Trace.WriteLine ($"Connection already opened {conn.Gateway.IsOpen}/{conn.Gateway.Iori.ToFileName ()}");
                }
            } else {
                var ioManager = new ThingGraphIoManager { };
                var sinkIo = ioManager.GetSinkIO(Iori, IoMode.Read) as ThingGraphIo;
                try {
                    var sink = sinkIo.Open(Iori);
                    if (sink != null) {
                        Trace.WriteLine ($"DataBase opened {Iori.ToFileName ()}");
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
                    Trace.WriteLine ($"Empty Graph created {Iori.ToFileName ()}");
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
                Trace.WriteLine ($"DataBase closed {Iori.ToFileName ()}");
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

            schema.EnsureDefaultThings (graph);

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

                    if (autoView != null) {
                        topic = autoView;
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

            var contentIoPool = Registry.Pooled<StreamContentIoPool> ();
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

            var result = new StreamContent (thing.ContentOf ());
            result.Source = thing.Id.ToString ("X16");

            SetMimeType(result);

            if (result.ContentType == ContentTypes.TIF) {
                var sinkType = ContentTypes.PNG;
                var converter = Registry.Pooled<ConverterPool<Stream>>()
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

        public static ICollection<long> ConvertibleHtmlStreamTypes = null;
        public virtual bool IsConvertibleToHtml (IThing thing) {
            var streamThing = thing as IStreamThing;
            if (streamThing == null)
                return false;
            if (streamThing.StreamType == ContentTypes.HTML)
                return true;
            if (ConvertibleHtmlStreamTypes == null) {
                ConvertibleHtmlStreamTypes = new HashSet<long> (Registry.Pooled<ConverterPool<Stream>> ().SelectMany (c => c.SupportedTypes).Where (t => t.Item2 == ContentTypes.HTML).Select (t => t.Item1));
            }
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
                if (thing.StreamType == ContentTypes.HTML) {
                    thing.DeCompress ();
                    var reader = new StreamReader (thing.Data);
                    result.Data = reader.ReadToEnd ();
                    thing.ClearRealSubject ();

                } else {
                    var sinkType = ContentTypes.HTML;
                    var converter = Registry.Pooled<ConverterPool<Stream>> ()
                            .Find (thing.StreamType, sinkType);
                    if (converter != null) {
                        var source = thing.ContentOf ();
                        if (converter is IAdobeRtfFilterConverter adbC) {
                            adbC.UseAdobeFilter = true;
                        }
                        if (converter is IHtmlConverter htmlConverter) {
                            htmlConverter.UseHtmlHeaderTags = false;
                            result.Data = htmlConverter.ToHtml (source.Data);
                        } else {
                            using (var reader = new StreamReader (converter.Use (source, sinkType).Data))
                                result.Data = reader.ReadToEnd ();
                        }

                        source.Data.Dispose ();
                    }
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

        /// <summary>
        /// an expandwalk with leafs of thing
        /// attention! use it only once, or make tolist or toarray
        /// </summary>
        /// <param name="thing"></param>
        /// <returns></returns>
        public IEnumerable<IThing> Leafs (IThing thing) {
            return ThingGraph.Walk()
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
            return ThingGraph.Walk()
                .ExpandWalk (thing, 0, Walk.Roots<IThing, ILink> ())
                .Where (item => !(item.Node is ILink || item.Node == thing))
                .Select (item => item.Node);
        }

       public Href HrefOfThing (IThing t) {
           var d = ThingDataToDisplay (t);
            return new Href (d.ToString (), t.Id.ToString ("X16"));//DescribedThing (t).Id.ToString ("X16"));
       }

       public IEnumerable<Href> HrefsOfThings (IEnumerable<IThing> things) {
           return things.Select (t => HrefOfThing (t));
       }

       public VisualHref HrefOfVisual (VisualThingGraph graph, IVisual v) {
           var t = graph.ThingOf (v);
           return new VisualHref {
               Text = v.Data.ToString (),
               Id = t.Id.ToString ("X16"),
               Location = v.Location,
               Size = v.Size
           };
       }

       IGraphSceneLayout<IVisual, IVisualEdge> _layout = null;
       public virtual IGraphSceneLayout<IVisual, IVisualEdge> Layout {
           get {
               if (_layout == null) {
                  // this makes a bug; some leafs don't work after that! 
                  // var scene = new VisualThingsSceneViz ().CreateScene (this.ThingGraph);
                   var scene = new Scene ();
                   Func<IGraphScene<IVisual,IVisualEdge >> fScene = () => scene;
                   _layout = Registry.Factory.Create<IGraphSceneLayout<IVisual, IVisualEdge>> (
                       fScene, Registry.Pooled<StyleSheets> ().DefaultStyleSheet);
               }
               return _layout;
           }
           protected set { _layout = value; }
       }

       public IEnumerable<VisualHref> VisualHrefsOf (IStreamThing source) {
           source.DeCompress();
           
           IEnumerable<VisualHref> result = new VisualHref[0];
           var stream = source.Data;
           if (stream == null)
               return null;
           try {

               stream.Position = 0;
               var graph = new VisualThingGraph () { Source = ThingGraph };
               var serializer = new VisualThingXmlSerializer { VisualThingGraph = graph, Layout = this.Layout };
               serializer.Read (stream);
               stream.Position = 0;

               result = serializer.VisualsCollection
                   .Where (v => !(v is IVisualEdge))
                   .Select (v => HrefOfVisual (graph, v))
                   .ToArray ();

           } catch (Exception ex) {
               // TODO: stream-closed-error should never happen.Try to get reread the source  
               Registry.Pooled<IExceptionHandler> ().Catch (ex, MessageType.OK);
           } finally {
               source.ClearRealSubject ();
           }
           return result;
       }


    }
}