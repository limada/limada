/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Linq;
using Limada.Model;
using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using System;
using Limaki.Common;
using Limaki.Graphs;
using System.Collections.Generic;
using Limaki.Graphs.Extensions;
using System.IO;
using Limaki.Common.Linqish;
using Limaki.Data;

namespace Limada.Data {

    public abstract class ThingGraphIo : SinkIo<IoInfo>, ISink<IoInfo,ThingGraphContent>, ISink<ThingGraphContent, IoInfo> {

        protected ThingGraphIo(ContentInfoSink supportedContents) : base(supportedContents) {}

        public override bool Supports (IoInfo source) {
            return InfoSink.Supports(source.Extension);
        }

        public override ContentInfo Use (IoInfo source) {
            if (Supports(source))
                return InfoSink.SupportedContents.First();
            return null;
        }

        public override ContentInfo Use (IoInfo source, ContentInfo sink) {
            if (Supports(source))
                return SinkExtensions.Use(source, sink, s => Use(s));
            return null;
        }

        protected abstract ThingGraphContent OpenInternal(IoInfo source);
        public abstract void Flush (ThingGraphContent sink);
        public abstract void Close(ThingGraphContent sink);

        ThingGraphContent ISink<IoInfo,ThingGraphContent>.Use (IoInfo source) {
            return Open(source);
        }

        public virtual ThingGraphContent Open (IoInfo source) {
            var result = OpenInternal(source);
            result.Source = source;
            return result;
        }

        public virtual ThingGraphContent Use (IoInfo source, ThingGraphContent sink) {
            var result = Open(source);
            if (sink.Data == null)
                return result;
            else {
                throw new NotImplementedException("Merging not implemented");
            }
        }

        public virtual IoInfo Use (ThingGraphContent source) {
            Close(source);
            return source.Source as IoInfo;
        }

        public virtual IoInfo Use (ThingGraphContent source, IoInfo sink) {
            source.Source = sink;
            Close(source);
            return sink;

        }
    }

    /// <summary>
    /// returns all things of source
    /// extract all things of source stored in a sheet
    /// and returns them
    /// </summary>
    public class ThingGraphExportSink : ISink<IGraph<IThing, ILink>, IThingGraph>, IProgress {

        public Action<string, int, int> Progress { get; set; }

        /// <summary>
        /// returns all things of source
        /// extract all things of source stored in a sheet
        /// and returns them
        /// </summary>
        /// <param name="sourceView"></param>
        /// <param name="sink"></param>
        public virtual IEnumerable<IThing> ExpandThings (IThingGraph thingGraph, IEnumerable<IThing> source) {

            var result = new Stack<IThing>(source);

            while (result.Count > 0) {

                var thing = result.Pop();
                yield return thing;

                var streamThing = thing as IStreamThing;
                if (streamThing != null && streamThing.StreamType == ContentTypes.LimadaSheet) {

                    var ser = new ThingIdSerializer();
                    ser.Graph = thingGraph;

                    streamThing.DeCompress();

                    ser.Read(streamThing.Data);

                    streamThing.ClearRealSubject();
                    ser.ThingCollection.ForEach(t => result.Push(t));

                }
            }
        }

        public virtual IThingGraph Use (IGraph<IThing, ILink> source, IThingGraph sink) {
            
            var thingGraph = Use(source);
            
            if (thingGraph != null) {
                var things = ExpandThings(thingGraph, source);
                var completeThings =
                    things.Distinct().CompletedThings(thingGraph)
                        .ToList();
                sink.AddRange(completeThings);
                foreach (var thing in completeThings.OfType<IStreamThing>()) {
                    var data = thingGraph.DataContainer.GetById(thing.Id);
                    sink.DataContainer.Add(data);
                }

            }
            return sink;
        }

        public virtual IThingGraph Use (IGraph<IThing, ILink> source) {
             var graph = source.RootSource();
             if (graph == null)
                 return null;
             return graph.Two as IThingGraph;
        }
    }

    public class ThingGraphMergeSink : ISink<IThingGraph, IThingGraph>, IProgress {
       
        public Action<string, int, int> Progress { get; set; }

        public IThingGraph Use (IThingGraph source, IThingGraph sink) {
            Action<IThing> message = null;
            var i = 0;
            var iStreams = 0;
            var count = source.Count;
            bool streams = false;
            if (this.Progress != null)
                message = thing => {
                    i++;
                    if (thing != null) {
                        var type = thing.GetType();
                        if (!streams && Reflector.Implements(type, typeof(IStreamThing)))
                            iStreams++;
                        var icount = streams ? count : count + iStreams;
                        this.Progress(string.Format("merging {2} of {3} ({4} Streams / {1} {0} )", thing.Id.ToString("X"),
                            streams ? "Streams" : type.Name, i, icount, iStreams), i, icount);
                    }
                };
            source.MergeThingsInto(sink, message, () => {
                streams = true;
                i = 0;
                count = iStreams;
            });
            return sink;
        }

        public IThingGraph Use (IThingGraph source) {
            return source;
        }
    }
}