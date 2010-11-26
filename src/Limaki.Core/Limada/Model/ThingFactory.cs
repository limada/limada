using System;
using System.IO;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Model.Streams;
using Id = System.Int64;

namespace Limada.Model {
    public class ThingFactory {
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

        public ILink CreateLink(object data) {
            ILink result = new Link(CommonSchema.EmptyMarker);
            return result;
        }
    }
}