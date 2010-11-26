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
            ThingGraphUtils.MergeGraphs(source, this.Data);
        }

        public void ReadIntoList(ICollection<IThing> things, IGraph<IThing, ILink> view) {
            var graph = new GraphPairFacade<IThing, ILink>().Source(view);

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
            var graph = new GraphPairFacade<IThing, ILink>().Source(view);
            if (graph != null) {
                var source = graph.Two as IThingGraph;
                var things = new List<IThing>();
                this.ReadIntoList(things, view);
                var completeThings =
                    ThingGraphUtils.CompletedThings (things.Distinct (), source).ToList ();
                ThingGraphUtils.AddRange(target, completeThings);
                foreach(var  thing in completeThings.OfType<IStreamThing>()) {
                    var data = source.DataContainer.GetById(thing.Id);
                    target.DataContainer.Add(data);
                }
                
            }
        }
    }
}