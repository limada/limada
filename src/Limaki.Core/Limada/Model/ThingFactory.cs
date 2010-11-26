using System;
using System.IO;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Model.Streams;
using Id = System.Int64;

namespace Limada.Model {
    public class ThingFactory : FactoryBase, IThingFactory {
        public IThing CreateThing(IThingGraph graph, object data) {
            IThing result = CreateThing (data);
            if (result is IContainerProxy<Id>) {
                ((IContainerProxy<Int64>)result).DataContainer = graph.DataContainer;
            }
            return result;
        }

        public IThing CreateThing(object data) {
            IThing result = null;
            if (data == null || data is Empty) {
                result = new Thing();
            } if (data is Stream) {
                result = new StreamThing (data as Stream);
            }
            else
                result = new Thing<string>(data.ToString());
            return result;
        }

        public ILink CreateLink(IThingGraph graph, object data) {
            return CreateLink (data);
        }


        public ILink CreateLink(IThing root, IThing leaf, IThing marker) {
            if (marker == null) {
                marker = CommonSchema.EmptyMarker;
            }
            return new Link (root, leaf, marker);
        }

        public ILink CreateLink(object data) {
            ILink result = new Link(CommonSchema.EmptyMarker);
            return result;
        }

        protected override void InstrumentClazzes() {
            Clazzes.Add(typeof(IThing), typeof(Thing));
            Clazzes.Add(typeof(IThing<string>), typeof(Thing<string>));
            Clazzes.Add(typeof(IThing<Stream>), typeof(StreamThing));
            Clazzes.Add(typeof(IStreamThing), typeof(StreamThing));
            Clazzes.Add(typeof(IThing<long>), typeof(NumberThing));
            Clazzes.Add(typeof(INumberThing), typeof(NumberThing));
            Clazzes.Add(typeof(ILink), typeof(Link));
        }
    }
}