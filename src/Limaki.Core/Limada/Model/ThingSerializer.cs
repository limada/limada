using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Limada.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using Id = System.Int64;
using Limaki.Common;

namespace Limada.Model {

    public class ThingSerializer : ThingSerializerBase {

        public static class NodeNames {
            public const string Things = "things";
            public const string Root = "root";
            public const string StreamThings = "streamthings";
        }

        public virtual XmlWriterSettings Settings {
            get {
                var settings = new XmlWriterSettings {
                    OmitXmlDeclaration = true,
                    ConformanceLevel = ConformanceLevel.Fragment,
                    CloseOutput = false,
                };
                return settings;
            }
        }

        public override XmlReader CreateReader (Stream s) {
#if ! SILVERLIGHT
            var result = XmlDictionaryReader
                //.CreateBinaryReader(s, XmlDictionaryReaderQuotas.Max);
                .CreateTextReader (s, XmlDictionaryReaderQuotas.Max);
#else
            var result = XmlReader.Create (s);
#endif

            return result;
        }

        public override XmlWriter CreateWriter (Stream s) {
            var writer = XmlDictionaryWriter
                //.CreateBinaryWriter(s,null,null,false);
                .CreateDictionaryWriter (XmlWriter.Create (s, Settings));
            return writer;
        }

        XmlObjectSerializer _serializer = null;
        public virtual XmlObjectSerializer Serializer {
            get {
                if (_serializer == null) {
                    var factory = Registry.Factory.Create<IThingFactory> ();
                    var knownClasses = factory.KnownClasses.ToList ();
                    knownClasses.Add (typeof (RealData<Byte[]>));
                    _serializer = new DataContractSerializer (factory.Clazz<IThing> (), knownClasses);
                }
                return _serializer;
            }
        }

        protected virtual void Write (IEnumerable<IThing> things, XmlWriter writer, XmlObjectSerializer serializer) {
            throw new NotImplementedException ();
        }

        public override void Write (Stream s) {

            using (var writer = CreateWriter (s)) {
                writer.WriteStartElement (NodeNames.Root);
                writer.WriteStartElement (NodeNames.Things);

                var serializer = this.Serializer;
                var streams = new List<IStreamThing> ();
                foreach (var thing in this.ThingCollection) {
                    serializer.WriteObject (writer, thing);
                    if (thing is IStreamThing) {
                        streams.Add ((IStreamThing) thing);
                    }
                }

                writer.WriteEndElement ();
                if (streams.Count > 0) {
                    writer.WriteStartElement (NodeNames.StreamThings);
                    foreach (var thing in streams) {
                        if (thing.ContentContainer != null) {
                            var data = thing.ContentContainer.GetById (thing.Id);
                            serializer.WriteObject (writer, data);
                            data = null;
                        }
                    }
                    writer.WriteEndElement ();
                }
                writer.WriteEndElement ();

                writer.Flush ();
            }
        }

        /// <summary>
        /// Attention! the stream will be closed after reading!
        /// </summary>
        /// <param name="s"></param>
        public override void Read (Stream s) {
            using (var reader = CreateReader (s)) {
                reader.ReadStartElement (NodeNames.Root);
                reader.ReadStartElement (NodeNames.Things);
                var streamThings = new Dictionary<Id, IStreamThing> ();
                while (Serializer.IsStartObject (reader)) {
                    var thing = Serializer.ReadObject (reader) as IThing;
                    ThingCollection.Add (thing);
                    var streamThing = thing as IStreamThing;
                    if (streamThing != null) {
                        streamThings[thing.Id] = streamThing;
                    }
                }

                reader.ReadEndElement ();
                if (reader.IsStartElement (NodeNames.StreamThings)) {
                    reader.ReadStartElement (NodeNames.StreamThings);
                    while (Serializer.IsStartObject (reader)) {
                        var data = Serializer.ReadObject (reader) as RealData<byte[]>;
                        if (this.Graph.ContentContainer != null) {
                            this.Graph.ContentContainer.Add (data);
                        }
                    }
                }
            }
        }
    }
}