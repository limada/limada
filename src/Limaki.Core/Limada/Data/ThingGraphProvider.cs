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
 * http://limada.sourceforge.net
 * 
 */

using System.Collections.Generic;
using System.Linq;
using Limada.Model;
using Limaki.Data;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Streams;
using System.IO;
using Limaki.Common;
using System;

namespace Limada.Data {
    public abstract class ThingGraphProvider : DataProvider<IThingGraph>, IThingGraphProvider {
        protected IThingGraph _data = null;
        public override IThingGraph Data {
            get { return this._data; }
            set { _data = value; }
        }
 
        public override void Save() {
            SaveCurrent();
        }

        public override void SaveAs(IThingGraph source, DataBaseInfo FileName) {
            this.Data = null;
            Open(FileName);
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
                            streams ? "Streams" : type.Name, i, icount, iStreams),i,icount);
                    }
                };
            source.MergeThingsInto(this.Data, message, () => {
                streams = true;
                i = 0;
                count = iStreams;
            });
        }

        public void ReadIntoList(ICollection<IThing> things, IGraph<IThing, ILink> view) {
            var graph = view.RootSource();

            if (graph != null) {
                foreach (var thing in view) {

                    things.Add(thing);

                    var streamThing = thing as IStreamThing;
                    if (streamThing != null && streamThing.StreamType == StreamTypes.LimadaSheet) {

                        ThingIdSerializer ser = new ThingIdSerializer();
                        ser.Graph = graph.Two as IThingGraph;

                        streamThing.DeCompress();

                        ser.Read(streamThing.Data as Stream);

                        streamThing.ClearRealSubject();

                        var thingView = new GraphView<IThing, ILink>(graph.Two, new ThingGraph());
                        new GraphViewFacade<IThing, ILink>(thingView).Add(ser.ThingCollection);

                        ReadIntoList(things, thingView);
                    }
                }
            }
        }

        public virtual void Export(IGraph<IThing, ILink> view, IThingGraph target) {
            var graph = view.RootSource();
            if (graph != null) {
                var source = graph.Two as IThingGraph;
                var things = new List<IThing>();
                this.ReadIntoList(things, view);
                var completeThings =
                    things.Distinct ().CompletedThings(source).ToList ();
                target.AddRange(completeThings);
                foreach(var  thing in completeThings.OfType<IStreamThing>()) {
                    var data = source.DataContainer.GetById(thing.Id);
                    target.DataContainer.Add(data);
                }
                
            }
        }

        public virtual void RawImport(DataBaseInfo source, IDataProvider<IThingGraph> target) {
            Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(
                new Exception(string.Format(
                    "{0} RawImport of {1} not possible", this.Description, DataBaseInfo.ToFileName(source))), MessageType.OK);
        }
    }
}