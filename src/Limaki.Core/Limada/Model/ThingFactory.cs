using System;
using System.IO;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Contents;
using Id = System.Int64;
using Limaki.Graphs;

namespace Limada.Model {

    public class ThingFactory : Factory, IThingFactory, IGraphModelPropertyChanger<IThing,ILink> {

        #region Item, Thing

        public IThing CreateItem<T> (Id id, T data) {
            IThing result = null;
            var type = typeof (T);
            if (type == typeof (object) && data != null) {
                type = data.GetType ();
            }

            if (type == typeof (string)) {
                result = new Thing<string> (id, data as string);//new StringThing(data.ToString());
            } else if (type == typeof (object) && data == null) {
                result = new Thing (id);
            } else if (type == typeof (Stream) || type.IsSubclassOf (typeof (Stream))) {
                result = new StreamThing (id, data as Stream);
            } else if (
                type == typeof (int) ||
                type == typeof (Int64) ||
                type == typeof (float) ||
                type == typeof (double) ||
                type == typeof (DateTime) ||
                type == typeof (Quad16)
            ) {
                var number = new NumberThing (id, 0);
                number.Number = data;
                result = number;
            } else if (type == typeof (Empty)) {
                result = new Thing (id);
            } else {
                result = new Thing<T> (id, data);
            }
            SetState (result);
            return result;
        }

        public IThing CreateItem(IThingGraph graph, object data) {
            return CreateItem(Isaac.Long, graph, data);
        }

        public IThing CreateItem(Id id, IThingGraph graph, object data) {
            var result = CreateItem(id,data);
            if (result is IContainerProxy<Id>) {
                ((IContainerProxy<Int64>)result).ContentContainer = graph.ContentContainer;
            }
            return result;
        }

        public IThing CreateItem() {
            return CreateItem<object>(null);
        }

        public IThing CreateIdItem(Id id) {
            return CreateItem<object>(id,null);
        }

        public IThing CreateItem<T>(T data){
            return CreateItem(Isaac.Long, data);
        }

        #endregion

        #region Edge, Link

        public ILink CreateEdge<T> (Id id, T data) {
            var marker = TryGetCreateMarker (data);
            var result = new Link (id, marker);
            SetState (result);
            return result;
        }

        public ILink CreateEdge (Id id, IThing root, IThing leaf, IThing marker) {
            if (marker == null) {
                marker = CommonSchema.EmptyMarker;
            }
            var result = new Link (id, root, leaf, marker);
            SetState (result);
            return result;
        }

        public IThing TryGetCreateMarker (object data) {
            var marker = data as IThing;

            if (data == null || data == CommonSchema.EmtpyMarkerString || data == CommonSchema.NullString)
                marker = CommonSchema.EmptyMarker;

            if (marker == null) {
                marker = CreateItem (data);
            }
            return marker;
        }

        public void SetState (IThing thing) {
            if (thing != null) {
                thing.State.Hollow = true;
                thing.SetCreationDate (DateTime.Now);
            }
        }

        public ILink CreateEdge<T> (T data) {
            return CreateEdge (Isaac.Long, data);
        }

        public ILink CreateEdge(Id id, IThingGraph graph, object data) {
            return CreateEdge(id,data);
        }

        public ILink CreateEdge(IThingGraph graph, object data) {
            return CreateEdge(Isaac.Long, graph, data);
        }

        public ILink CreateEdge(IThing root, IThing leaf, IThing marker) {
            return CreateEdge(Isaac.Long, root, leaf, marker);
        }

        ILink IGraphModelFactory<IThing, ILink>.CreateEdge (IThing root, IThing leaf, object data) {
            return CreateEdge (root, leaf, data);
        }

        protected ILink CreateEdge (IThing root, IThing leaf, object data) {
            var marker = TryGetCreateMarker (data);
            return CreateEdge (root, leaf, marker);
        }

        public ILink CreateEdge (IThing root, IThing leaf) {
            return CreateEdge (root, leaf, default (IThing));
        }

        public ILink CreateEdge () {
            return CreateEdge (default (IThing), default (IThing), default (IThing));
        }

        #endregion

        public IGraph<IThing, ILink> Graph () {
            return Create<IThingGraph> ();
        }


        #region IFactory

        protected override void InstrumentClazzes () {

            Add<IThingGraph> (() => new ThingGraph ());
            AddKnown<IThingGraph, ThingGraph> ();

            Add<IThing> (() => new Thing ());
            AddKnown<IThing, Thing> ();

            Add<IThing<string>> (() => new Thing<string> ()); //typeof(StringThing()); 
            AddKnown<IThing<string>, Thing<string>> ();

            Add<IThing<Stream>> (() => new StreamThing ());
            Add<IStreamThing> (() => new StreamThing ());
            AddKnown<IThing<Stream>, StreamThing> ();
            AddKnown<IStreamThing, StreamThing> ();

            Add<IThing<long>> (() => new NumberThing ());
            Add<IThing<double>> (() => new NumberThing ());
            Add<IThing<DateTime>> (() => new NumberThing ());
            Add<IThing<Quad16>> (() => new NumberThing ());
            Add<IThing<int>> (() => new NumberThing ());
            Add<INumberThing> (() => new NumberThing ());

            Add<ILink> (() => new Link ());


            AddKnown<IThing<long>, NumberThing> ();
            AddKnown<IThing<double>, NumberThing> ();
            AddKnown<IThing<DateTime>, NumberThing> ();
            AddKnown<IThing<Quad16>, NumberThing> ();
            AddKnown<IThing<int>, NumberThing> ();
            AddKnown<INumberThing, NumberThing> ();

            AddKnown<ILink, Link> ();
        }

        public override T Create<T> () {
            var result = base.Create<T> ();
            SetState (result as IThing);
            return result;
        }

        public override object Create (Type type) {
            var result = base.Create (type);
            SetState (result as IThing);
            return result;
        }

        #endregion

        #region IGraphModelPropertyChanger

        public void SetProperty (IThing item, object data) {
            var link = item as ILink;
            if (link != null) {
                var marker = link.Marker;
                if (marker == null || marker.Data != data)
                    link.Marker = TryGetCreateMarker (data);
            } else
                item.Data = data;
        }

        public object GetProperty (IThing item) {
            return item.Data;
        }

        #endregion

    }
}