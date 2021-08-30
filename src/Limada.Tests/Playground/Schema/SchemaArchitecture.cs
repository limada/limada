using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Limada.Model;
using Id = System.Int64;

using NUnit.Framework;
using Limaki.Common.Collections;
using System.IO;

namespace Limaki.Playground.Schema {
    public class SchemaArchitectureWithAnnotations {
        [ThingSchema(0x97FD9E201789BFCB, Data = "Marker")]
        public const IThing Marker = null;

        //[LinkSchema(0xCA8D04BE519DD777, Marker)]
        //public ILink Link;


    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ThingIdAttribute : Attribute {
        private Id _id = default(Id);
        public long Id {
            get { return _id; }
            set { _id = value; }
        }

        public ThingIdAttribute(Id id) { this._id = id; }
        public ThingIdAttribute(ulong id) : this(unchecked((long)id)) { }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ThingSchemaAttribute : ThingIdAttribute {
        public ThingSchemaAttribute(Id id) : base(id) { }
        public ThingSchemaAttribute(ulong id) : base(id) { }

        private object _data = null;
        public object Data {
            get { return _data; }
            set { _data = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class LinkSchemaAttribute : ThingIdAttribute {
        public LinkSchemaAttribute(Id id) : base(id) { }
        public LinkSchemaAttribute(ulong id) : base(id) { }
        public LinkSchemaAttribute(ulong id, IThing root): base(id) {
            this.Root = root;
        }

        private IThing _root;
        private IThing _leaf;
        private IThing _marker;

        public IThing Root {
            get { return _root; }
            set { _root = value; }
        }

        public IThing Leaf {
            get { return _leaf; }
            set { _leaf = value; }
        }

        public IThing Marker {
            get { return _marker; }
            set { _marker = value; }
        }
    }

}
