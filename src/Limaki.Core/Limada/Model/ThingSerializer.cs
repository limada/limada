using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Limada.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using Id = System.Int64;

namespace Limada.Model {
    public class ThingSerializer : ThingSerializerBase {
        public virtual XmlWriterSettings Settings {
            get {
                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel = ConformanceLevel.Fragment;
                settings.CloseOutput = false;
                return settings;
            }
        }

        public override XmlReader CreateReader(Stream s) {
#if ! SILVERLIGHT
            var result = XmlDictionaryReader
                //.CreateBinaryReader(s, XmlDictionaryReaderQuotas.Max);
                .CreateTextReader(s, XmlDictionaryReaderQuotas.Max);
#else
            var result = XmlReader.Create (s);
#endif

            return result;
        }

        public override XmlWriter CreateWriter(Stream s) {
            var writer = XmlDictionaryWriter
                //.CreateBinaryWriter(s,null,null,false);
                .CreateDictionaryWriter(XmlWriter.Create(s, Settings));
            return writer;
        }


        public virtual XmlObjectSerializer Serializer {
            get {
                var factory = new ThingFactory();
                var knownClasses = factory.KnownClasses.ToList();
                knownClasses.Add(typeof(RealData<Byte[]>));
                return new DataContractSerializer(factory.Clazz<IThing>(), knownClasses);
            }
        }

        protected virtual void Write(IEnumerable<IThing> things, XmlWriter writer, XmlObjectSerializer serializer) {

        }

        public override void Write(Stream s) {

            using (var writer = CreateWriter(s)) {
                writer.WriteStartElement("root");
                writer.WriteStartElement("things");

                var serializer = this.Serializer;
                var streams = new List<IStreamThing>();
                foreach (var thing in this.ThingCollection) {
                    serializer.WriteObject(writer, thing);
                    if (thing is IStreamThing) {
                        streams.Add((IStreamThing)thing);
                    }
                }

                writer.WriteEndElement();
                if (streams.Count > 0) {
                    writer.WriteStartElement("streamthings");
                    foreach (var thing in streams) {
                        if (thing.DataContainer != null) {
                            var data = thing.DataContainer.GetById(thing.Id);
                            serializer.WriteObject(writer, data);
                            data = null;
                        }
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.Flush();
            }
        }

        /// <summary>
        /// Attention! the stream will be closed after reading!
        /// </summary>
        /// <param name="s"></param>
        public override void Read(Stream s) {
            using (var reader = CreateReader(s)) {
                reader.ReadStartElement("root");
                reader.ReadStartElement("things");
                var streamThings = new Dictionary<Id, IStreamThing>();
                while (Serializer.IsStartObject(reader)) {
                    IThing thing = Serializer.ReadObject(reader) as IThing;
                    ThingCollection.Add(thing);
                    var streamThing = thing as IStreamThing;
                    if (streamThing!=null) {
                        streamThings.Add(thing.Id, streamThing);
                    }
                }

                reader.ReadEndElement();
                if (reader.IsStartElement("streamthings")) {
                    reader.ReadStartElement("streamthings");
                    while (Serializer.IsStartObject(reader)) {
                        var data = Serializer.ReadObject (reader) as RealData<byte[]>;
                        if (this.Graph.DataContainer != null) {
                            this.Graph.DataContainer.Add (data);
                        }
                    }
                }
            }
        }
    }
}