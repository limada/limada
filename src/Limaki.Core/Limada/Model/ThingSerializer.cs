/*
 * Limada
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Limaki.Common.Collections;
using Id = System.Int64;

namespace Limada.Model {
#if ! SILVERLIGHT        
    public class ThingSerializer {
        private XmlDocument _document = new XmlDocument();

        public virtual XmlDocument Document {
            get {
                if (_document == null) {
                    _document = new XmlDocument();
                    XmlDocumentType doctype = _document.CreateDocumentType("limada", null, null, "<!ELEMENT limada.things ANY>");
                    _document.AppendChild(doctype);
                }
                return _document;
            }
            set { _document = value; }
        }
        
        private IThingGraph _graph = null;
        public virtual IThingGraph Graph {
            get { return _graph; }
            set { _graph = value; }
        }

        private XmlNode _things = null;
        public virtual XmlNode Things {
            get {
                if (_things == null) {
                    _things = Document.SelectSingleNode("things");
                }
                if (_things == null) {
                    _things = _document.CreateElement("things");
                    Document.AppendChild(_things);
                }
                return _things;
            }
            set { _things = value; }
        }
        
        private ICollection<IThing> _thingCollection = null;
        public ICollection<IThing> ThingCollection {
            get {
                if (_thingCollection == null && Graph != null) {
                    _thingCollection = new Set<IThing>();
                    //ReadInto(_thingCollection);
                }
                return _thingCollection;
            }
            set { _thingCollection = value; }
        }

        public virtual XmlElement Write(IThing thing) {
            XmlElement xmlthing = Document.CreateElement("thing");
            xmlthing.SetAttribute("id", thing.Id.ToString("X"));
            Things.AppendChild(xmlthing);
            return xmlthing;
        }

        public virtual void Write(IEnumerable<IThing> things) {
            if (things == null) return;
            foreach (IThing thing in things) {
                Write(thing);
            }
        }

        public virtual void Write(Stream s) {
            Write(ThingCollection);
            Document.Save(s);
        }

        public long ReadInt(XmlElement node, string attribute, bool hex) {
            long result = default(int);
            string s = node.GetAttribute(attribute);
            if (!string.IsNullOrEmpty(s)) {
                if (hex)
                    long.TryParse(s, NumberStyles.AllowHexSpecifier, null, out result);
                else
                    long.TryParse(s, out result);
            }
            return result;
        }

        protected virtual IThing Read(XmlElement node) {
            Id id = ReadInt (node, "id", true);
            if (id != default(Id)) 
                return Graph.GetById(id);
            return null;
        }

        protected virtual void ReadInto(ICollection<IThing> things) {
            foreach (XmlElement node in Things) {
                IThing thing = Read(node);
                if (thing != null && !things.Contains(thing)) {
                    things.Add(thing);
                }
            }
        }

        protected virtual void ReadThings() {
            ReadInto (ThingCollection);
        }

        public virtual void Read(Stream s) {
            if (s.Length ==0) {
                return;
            }
            XmlDocument document = new XmlDocument ();
            document.Load(s);
            this._document = document;
            this.Things = null;
            ReadThings ();
        }


    }
#endif
}
