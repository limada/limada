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
            return CreateItem(Common.Isaac.Long, graph, data);
        }

        public IThing CreateItem(Id id, IThingGraph graph, object data) {
            IThing result = CreateItem(id,data);
            if (result is IContainerProxy<Id>) {
                ((IContainerProxy<Int64>)result).DataContainer = graph.DataContainer;
            }
            return result;
        }

       
        public IThing CreateItem() {
            return CreateItem<object>(null);
        }

        public IThing CreateItem(Id id) {
            return CreateItem<object>(id,null);
        }

        public IThing CreateItem<T>(T data){
            return CreateItem(Common.Isaac.Long, data);
        }

        public IThing CreateItem<T>(Id id, T data) {
            IThing result = null;
            var type = typeof (T);
            if (type == typeof(object) && data != null) {
                type = data.GetType ();
            }

            if (type == typeof(string)) {
                result = new Thing<string>(id,data as string);//new StringThing(data.ToString());
            } else if (type == typeof(object) && data == null) {
                result = new Thing(id);
            } else if (type == typeof(Stream) || type.IsSubclassOf(typeof(Stream))) {
                result = new StreamThing(id,data as Stream);
            } else if (
                type == typeof(int) ||
                type == typeof(Int64) ||
                type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(DateTime) ||
                type == typeof(Quad16)
            ) {
                var number = new NumberThing(id,0);
                number.Number = data;
                result = number;
            } else if (type == typeof(Empty)) {
                result = new Thing(id);
            } else {
                result = new Thing<T>(id,data);
            }
            SetState(result);
            return result;
        }

        public ILink CreateEdge(Id id, IThingGraph graph, object data) {
            return CreateEdge(id,data);
        }

        public ILink CreateEdge(IThingGraph graph, object data) {
            return CreateEdge(Common.Isaac.Long, graph, data);
        }

        public ILink CreateEdge(Id id, IThing root, IThing leaf, IThing marker) {
            if (marker == null) {
                marker = CommonSchema.EmptyMarker;
            }
            var result = new Link(id, root, leaf, marker);
            SetState(result);
            return result;
        }

        public ILink CreateEdge(IThing root, IThing leaf, IThing marker) {
            return CreateEdge(Common.Isaac.Long, root, leaf, marker);
        }

        public ILink CreateEdge<T>(Id id, T data){
            var result = new Link(id, CommonSchema.EmptyMarker);
            SetState(result);
            return result;
        }

        public ILink CreateEdge<T>(T data) {
            return CreateEdge(Common.Isaac.Long, data);
        }

        protected override void InstrumentClazzes() {
            
                Clazzes[typeof (IThing)] =  typeof (Thing);
                Clazzes[typeof (IThing<string>)] =  typeof (Thing<string>); //typeof(StringThing); 

                Clazzes[typeof (IThing<Stream>)] =  typeof (StreamThing);
                Clazzes[typeof (IStreamThing)] =  typeof (StreamThing);

                Clazzes[typeof (IThing<long>)] =  typeof (NumberThing);
                Clazzes[typeof (IThing<double>)] =  typeof (NumberThing);
                Clazzes[typeof (IThing<DateTime>)] =  typeof (NumberThing);
                Clazzes[typeof (IThing<float>)] =  typeof (NumberThing);
                Clazzes[typeof (IThing<Quad16>)] =  typeof (NumberThing);
                Clazzes[typeof (IThing<int>)] =  typeof (NumberThing);
                Clazzes[typeof (INumberThing)] =  typeof (NumberThing);

                Clazzes[typeof (ILink)] =  typeof (Link);
            
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
                thing.SetCreationDate(DateTime.Now);
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