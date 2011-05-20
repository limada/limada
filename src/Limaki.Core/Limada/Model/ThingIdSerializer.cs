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
 * http://limada.sourceforge.net
 * 
 */

using System.Collections.Generic;
using System.Globalization;
using System.IO;
//using System.Xml;
using Limaki.Common.Collections;
using Id = System.Int64;
using System.Xml.Linq;
using System.Xml;

namespace Limada.Model {
    public class ThingIdSerializer:ThingSerializerBase {

        private XElement _things = null;
        public virtual XElement Things {
            get {
                if (_things == null) {
                    _things = Document.Element("things");
                }
                if (_things == null) {
                    _things = new XElement("things");
                    Document.Add(_things);
                }
                return _things;
            }
            set { _things = value; }
        }

        public override XmlReader CreateReader(Stream s) {
            return XmlReader.Create (s);
        }

        public override XmlWriter CreateWriter(Stream s) {
            return XmlWriter.Create (s);
        }

        public virtual XElement Write(IThing thing) {
            XElement xmlthing = new XElement("thing");
            xmlthing.Add(new XAttribute("id",thing.Id.ToString("X")));
            Things.Add(xmlthing);
            return xmlthing;
        }

        public virtual void Write(IEnumerable<IThing> things) {
            if (things == null) return;
            foreach (IThing thing in things) {
                Write(thing);
            }
        }

        public override void Write(Stream s) {
            Write(ThingCollection);
            using (System.Xml.XmlWriter writer = CreateWriter(s)) {
                Document.Save (writer);
                writer.Flush ();
            }
        }



        protected virtual IThing Read(XElement node) {
            Id id = ReadInt (node, "id", true);
            if (id != default(Id)) 
                return Graph.GetById(id);
            return null;
        }

        protected virtual void ReadInto(ICollection<IThing> things) {
            foreach (XElement node in Things.Elements()) {
                IThing thing = Read(node);
                if (thing != null && !things.Contains(thing)) {
                    things.Add(thing);
                }
            }
        }

        protected virtual void ReadThings() {
            ReadInto (ThingCollection);
        }

        public override void Read(Stream s) {
            if (s.Length ==0) {
                return;
            }

            using (System.Xml.XmlReader reader = CreateReader (s)){
                XDocument document = XDocument.Load (reader);
                this.Document = document;
                this.Things = null;
                ReadThings ();
            }
        }


    }

}


