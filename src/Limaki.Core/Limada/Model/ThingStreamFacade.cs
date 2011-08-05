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

        public IStreamThing CreateAndAdd(IThingGraph graph, Content<Stream> content) {

            IStreamThing thing = Factory.CreateItem(graph, content.Data) as IStreamThing;
            thing.Compression = content.Compression;
            thing.StreamType = content.StreamType;

            thing.Compress ();
            graph.Add(thing);

            thing.Flush();
            thing.ClearRealSubject();
            return thing;
        }

        public void SetStream(IStreamThing thing, Content<Stream> content) {
            if (thing != null && thing.DataContainer != null) {
                thing.Data = content.Data;
                thing.Compression = content.Compression;
                thing.StreamType = content.StreamType;
                thing.Compress();
                thing.Flush ();
                thing.ClearRealSubject ();
            }
        }

        public IStreamThing SetStream(IThingGraph graph, IStreamThing thing, Content<Stream> content) {
            if (thing != null) {
                thing.DataContainer = graph.DataContainer;
                SetStream (thing, content);
                AddStreamDescription(thing, content, graph);
            } else {
                thing = CreateAndAdd(graph, content);
                AddStreamDescription(thing, content, graph);            
            }

            return thing;
        }

        public virtual void AddStreamDescription(IThing thing, Content<Stream> content, IThingGraph thingGraph) {
            IThing streamThing = thing as IStreamThing;
            if (streamThing == null) return;
            CommonSchema schema = new CommonSchema(thingGraph, streamThing);
            if (content.Description != null) {
                if (schema.Description != null) {
                    schema.Description.Data = content.Description;
                } else {
                    schema.Description = Factory.CreateItem(content.Description); ;
                }
            }
            if (content.Source != null) {
                IThing des = schema.GetTheLeaf(CommonSchema.SourceMarker);
                if (des == null) {
                    des = Factory.CreateItem (content.Source);
                    schema.SetTheLeaf (CommonSchema.SourceMarker, des);
                } else {
                    des.Data = content.Source;
                }
            }
        }

        public static Content<Stream> GetContent(IThingGraph graph, IThing thing) {
            var result = GetContent (thing);
            if (result !=null && graph is SchemaThingGraph) {
                result.Description = graph.Description(thing);
                result.Source = graph.Source(thing);
            }
            return result;
        }

        public static Content<Stream> GetContent(IThing thing) {
            var result = default( Content<Stream> );
            var streamThing = thing as IStreamThing;
            if (streamThing!=null) {
                result = new Content<Stream> ();
                
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