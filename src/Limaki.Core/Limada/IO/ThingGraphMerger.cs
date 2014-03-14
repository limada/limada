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

using System;
using Limada.Model;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Graphs;
using System.Linq;

namespace Limada.IO {

    public class ThingGraphMerger : IPipe<IThingGraph, IThingGraph>, IProgress {
       
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

            GraphExtensions.MergeInto(source, sink, message, thing => {
                var existing = sink.GetById(thing.Id);
                if (existing == null)
                    sink.Add(thing);
                else
                    sink.UniqueThing(thing);
                var streamThing = thing as IStreamThing;
                if (streamThing != null) {
                    var data = source.DataContainer.GetById(thing.Id);
                    sink.DataContainer.Add(data);
                }
            });

            return sink;
        }

        public virtual void AttachThings (IThingGraph sink) {
            Action<IThing> message = null;
            var i = 0;
            var count = sink.Count;
            if (this.Progress != null)
                message = thing => this.Progress("attach {0} of {1} ", i++, count);

            foreach (var t in sink) {
                
                message(t);

                var streamThing = t as IStreamThing;
                if (streamThing != null) {
                    streamThing.DataContainer = sink.DataContainer;
                }

                var link = t as Link;
                if (link != null)
                    link.GetByID = sink.GetById;
            };
        }

        public IThingGraph Use (IThingGraph source) {
            return source;
        }
    }

}