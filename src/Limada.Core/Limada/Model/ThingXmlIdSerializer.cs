/*
 * Limada
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Limaki.Common.Collections;
using Id = System.Int64;
using System.Xml.Linq;
using System.Xml;

namespace Limada.Model {
	
    public class ThingXmlIdSerializer:ThingXmlSerializerBase {

        public static class NodeNames {
            public const string Things = "things";
            public const string Thing = "thing";
            public const string Id = "id";
        }

        private XElement _things = null;
        public virtual XElement XThings {
            get {
                if (_things == null) {
                    _things = Document.Element(NodeNames.Things);
                }
                if (_things == null) {
                    _things = new XElement(NodeNames.Things);
                    Document.Add(_things);
                }
                return _things;
            }
            set { _things = value; }
        }

        public override XmlReader CreateReader(Stream s) {
            var result = XmlReader.Create (s,new XmlReaderSettings {CheckCharacters = true});
            return result;
        }

        public override XmlWriter CreateWriter(Stream s) {
            return XmlWriter.Create (s);
        }

        public virtual XElement Write(IThing thing) {
            var xmlthing = new XElement(NodeNames.Thing);
            xmlthing.Add(new XAttribute(NodeNames.Id, thing.Id.ToString("X")));
            XThings.Add(xmlthing);
            return xmlthing;
        }

        public virtual void Write(IEnumerable<IThing> things) {
            if (things == null) return;
            foreach (var thing in things) {
                Write(thing);
            }
        }

        public override void Write(Stream s) {
            Write(Things);
            using (var writer = CreateWriter(s)) {
                Document.Save (writer);
                writer.Flush ();
            }
        }
        
        protected virtual IThing Read(XElement node) {
            var id = ReadInt (node, NodeNames.Id, true);
            if (id != default(Id)) 
                return Graph.GetById(id);
            return null;
        }

        protected virtual void ReadInto(ICollection<IThing> things) {
            foreach (var node in XThings.Elements()) {
                var thing = Read(node);
                if (thing != null && !things.Contains(thing)) {
                    things.Add(thing);
                }
            }
        }

		/// <summary>
		/// converts XElements from XThings into Things
		/// </summary>
		public virtual void ReadXThings() {
            ReadInto (Things);
        }

        public override void Read (Stream s) {
            if (s.Length == 0) {
                return;
            }

            void TryRead (Stream stream) {

                using (var reader = CreateReader (s)) {
                    var result = XDocument.Load (reader);
                    this.Document = result;
                    this.XThings = null;
                    ReadXThings ();
                }

            }

            try {
                TryRead (s);


            } catch (XmlException ex) {
                var pos = ex.LinePosition;
                // TODO: is here a conceptional problem?
                // why the stream is 0-terminated and has wrong length?
                if (s is MemoryStream m) {
                    m.Position = 0;
                    m.SetLength (pos+2);
                    TryRead (m);
                }
            }
        }

    }

}


