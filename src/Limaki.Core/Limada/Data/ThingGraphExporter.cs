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
using System.Collections.Generic;
using System.Linq;
using Limada.Model;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using Limaki.Model.Content.IO;

namespace Limada.Data {
    /// <summary>
    /// returns all things of source
    /// source can be a subgraph (a SubGraph)
    /// </summary>
    public class ThingGraphExporter : IPipe<IGraph<IThing, ILink>, IThingGraph>, IProgress {

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
            return graph.Source as IThingGraph;
        }
    }
}