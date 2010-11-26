using System;
using System.IO;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Model.Streams;
using Id = System.Int64;
using Limaki.Graphs;

namespace Limada.Model {
    public class ThingFactory : FactoryBase, IThingFactory {
        public IThing CreateItem(IThingGraph graph, object data) {
            IThing result = CreateItem (data);
            if (result is IContainerProxy<Id>) {
                ((IContainerProxy<Int64>)result).DataContainer = graph.DataContainer;
            }
            return result;
        }

        public IThing CreateItem() {
            return CreateItem<object>(null);
        }
        public IThing CreateItem<T>(T data) {
            IThing result = null;
            var type = typeof (T);
            if (type == typeof(object) && data != null) {
                type = data.GetType ();
            }

            if (type == typeof(string)) {
                result = new Thing<string>(data as string);//new StringThing(data.ToString());
            } else if (type == typeof(object) && data == null) {
                result = new Thing();
            } else if (type == typeof(Stream) || type.IsSubclassOf(typeof(Stream))) {
                result = new StreamThing(data as Stream);
            } else if (
                type == typeof(int) ||
                type == typeof(Int64) ||
                type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(DateTime) ||
                type == typeof(Quad16)
            ) {
                var number = new NumberThing();
                number.Number = data;
                result = number;
            } else if (type == typeof(Empty)) {
                result = new Thing();
            } else {
                result = new Thing<T>(data);
            }
            SetState(result);
            return result;
        }

        public ILink CreateEdge(IThingGraph graph, object data) {
            return CreateEdge (data);
        }


        public ILink CreateEdge(IThing root, IThing leaf, IThing marker) {
            if (marker == null) {
                marker = CommonSchema.EmptyMarker;
            }
            var result =  new Link (root, leaf, marker);
            SetState(result);
            return result;
        }



        public ILink CreateEdge<T>(T data) {
            var result = new Link(CommonSchema.EmptyMarker);
            SetState(result);
            return result;
        }

        protected override void InstrumentClazzes() {
            Clazzes.Add(typeof(IThing), typeof(Thing));
            Clazzes.Add(typeof(IThing<string>), typeof(Thing<string>)); //typeof(StringThing)); 

            Clazzes.Add(typeof(IThing<Stream>), typeof(StreamThing));
            Clazzes.Add(typeof(IStreamThing), typeof(StreamThing));

            Clazzes.Add(typeof(IThing<long>), typeof(NumberThing));
            Clazzes.Add(typeof(IThing<double>), typeof(NumberThing));
            Clazzes.Add(typeof(IThing<DateTime>), typeof(NumberThing));
            Clazzes.Add(typeof(IThing<float>), typeof(NumberThing));
            Clazzes.Add(typeof(IThing<Quad16>), typeof(NumberThing));
            Clazzes.Add(typeof(IThing<int>), typeof(NumberThing));
            Clazzes.Add(typeof(INumberThing), typeof(NumberThing));

            Clazzes.Add(typeof(ILink), typeof(Link));
        }

        ILink IGraphModelFactory<IThing, ILink>.CreateEdge(IThing root, IThing leaf, object data){
            IThing marker = data as IThing;
            if (marker == null) {
                CreateItem (data);
            }
            return CreateEdge (root, leaf, marker);
        }

        public void SetState(IThing thing) {
            if (thing != null) {
                thing.State.Hollow = true;
            }
        }

        public override T Create<T>() {
            var result = base.Create<T>();
            SetState (result as IThing);
            return result;
        }

        public override object Create(Type type) {
            var result = base.Create(type);
            SetState(result as IThing);
            return result;
        }
    }
}