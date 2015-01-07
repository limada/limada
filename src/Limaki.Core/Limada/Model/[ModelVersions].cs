using System;
using System.IO;
using Limada.Schemata;
using Limaki.Common;
using Limaki.Contents;
using Id = System.Int64;

namespace Limada.Model {

    public class ModelVersion {
        public double CurrentVersion { get { return 0.08; } }
    }

    public class ModelVersion_0_8 {

        public virtual double Version { get { return 0.08; } }

        public IThing IThing { get; set; }

        public IThing<object> IThing_T { get; set; }

        public ILink ILink { get; set; }

        public ILink<Id> IdLink { get; set; }

        public INumberThing INumberThing { get; set; }

        public IStreamThing IStreamThing { get; set; }

        public Thing Thing { get; set; }

        public Thing<object> Thing_T { get; set; }

        public Link Link { get; set; }

        public NumberThing NumberThing { get; set; }

        public StreamThing StreamThing { get; set; }

        public IIdContent<Id, byte[]> IIdContentByteArray { get; set; }
        public RealData<Byte[]> RealDataByteArray { get; set; }
        

        // declared, but not used:
        public IStringThing IStringThing { get; set; }
        // declared, but not used:
        public StringThing StringThing { get; set; }

        public class CommonSchema_0_8 {

            public static readonly IThing EmptyMarker = Thing (0x528FE1B697E54910);
            
            public static string NullString = ((char) (0x2260)).ToString (); // not equal
            public static string EmtpyMarkerString = "°";

            protected static IThing Thing (ulong id) { return new ModelVersion_0_8 ().CreateItem<object> (unchecked((Id)id), null); }
        }

        public virtual IThing CreateItem<T> (Id id, T data) {
            IThing result = null;
            var type = typeof (T);
            if (type == typeof (object) && data != null) {
                type = data.GetType ();
            }

            if (type == typeof (string)) {
                result = new Thing<string> (id, data as string);
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
            return result;
        }

        public virtual ILink CreateEdge<T> (Id id, T data) {
            var marker = TryGetCreateMarker (data);
            var result = new Link (id, marker);
            return result;
        }

        public virtual ILink CreateEdge (Id id, IThing root, IThing leaf, IThing marker) {
            if (marker == null) {
                marker = CommonSchema_0_8.EmptyMarker;
            }
            var result = new Link (id, root, leaf, marker);
            return result;
        }

        public virtual IThing TryGetCreateMarker (object data) {
            var marker = data as IThing;

            if (data == null || data == CommonSchema_0_8.EmtpyMarkerString || data == CommonSchema_0_8.NullString)
                marker = CommonSchema.EmptyMarker;

            if (marker == null) {
                marker = CreateItem (Isaac.Long, data);
            }
            return marker;
        }
    }

    public class ModelVersion_0_11 : ModelVersion_0_8 {

        public virtual double Version { get { return 0.11; } }

        // all of then used in LinqThingGraph, declared in Db4o.ThingGraph, but not in Factory now

        public IStringThing IStringThing { get; set; }
        public StringThing StringThing { get; set; }

        public INodeThing INodeThing { get; set; }
        public NodeThing NodeThing { get; set; }
    }
}