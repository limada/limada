using System.IO;
using Limada.Schemata;
using Limaki.Model.Streams;
using Limaki.Common;

namespace Limada.Model {
    public class ThingStreamFacade {
        public ThingStreamFacade (IThingFactory factory) {
            this._factory = factory;
        }

        public ThingStreamFacade() {}
        private IThingFactory _factory = null;

        public IThingFactory Factory {
            get {
                if (_factory == null) {
                    _factory = Registry.Factory.Create<IThingFactory>();
                }
                return _factory;
            }
            set { _factory = value; }
        }

        public IStreamThing CreateAndAdd(IThingGraph graph, StreamInfo<Stream> streamInfo) {

            IStreamThing thing = Factory.CreateItem(graph, streamInfo.Data) as IStreamThing;
            thing.Compression = streamInfo.Compression;
            thing.StreamType = streamInfo.StreamType;

            thing.Compress ();
            graph.Add(thing);

            thing.Flush();
            thing.ClearRealSubject();
            return thing;
        }

        public void SetStream(IStreamThing thing, StreamInfo<Stream> streamInfo) {
            if (thing != null && thing.DataContainer != null) {
                thing.Data = streamInfo.Data;
                thing.Compression = streamInfo.Compression;
                thing.StreamType = streamInfo.StreamType;
                thing.Compress();
                thing.Flush ();
                thing.ClearRealSubject ();
            }
        }

        public IStreamThing SetStream(IThingGraph graph, IStreamThing thing, StreamInfo<Stream> streamInfo) {
            if (thing != null) {
                thing.DataContainer = graph.DataContainer;
                SetStream (thing, streamInfo);
                AddStreamDescription(thing, streamInfo, graph);
            } else {
                thing = CreateAndAdd(graph, streamInfo);
                AddStreamDescription(thing, streamInfo, graph);            
            }

            return thing;
        }

        public virtual void AddStreamDescription(IThing thing, StreamInfo<Stream> streamInfo, IThingGraph thingGraph) {
            IThing streamThing = thing as IStreamThing;
            if (streamThing == null) return;
            CommonSchema schema = new CommonSchema(thingGraph, streamThing);
            if (streamInfo.Description != null) {
                if (schema.Description != null) {
                    schema.Description.Data = streamInfo.Description;
                } else {
                    schema.Description = Factory.CreateItem(streamInfo.Description); ;
                }
            }
            if (streamInfo.Source != null) {
                IThing des = schema.GetTheLeaf(CommonSchema.SourceMarker);
                if (des == null) {
                    des = Factory.CreateItem (streamInfo.Source);
                    schema.SetTheLeaf (CommonSchema.SourceMarker, des);
                } else {
                    des.Data = streamInfo.Source;
                }
            }
        }

        public static StreamInfo<Stream> GetStreamInfo(IThingGraph graph, IThing thing) {
            var result = GetStreamInfo (thing);
            if (result !=null && graph is SchemaThingGraph) {
                result.Description = ThingGraphUtils.GetDescription (graph, thing);
                result.Source = ThingGraphUtils.GetSource (graph, thing);
            }
            return result;
        }

        public static StreamInfo<Stream> GetStreamInfo(IThing thing) {
            var result = default( StreamInfo<Stream> );
            var streamThing = thing as IStreamThing;
            if (streamThing!=null) {
                result = new StreamInfo<Stream> ();
                
                streamThing.DeCompress ();
                result.Data = streamThing.Data as Stream;
                result.Compression = streamThing.Compression;
                result.StreamType = streamThing.StreamType;
                streamThing.ClearRealSubject (false);
            }
            return result;
        }
    }
}