using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using System.Globalization;

namespace Limada.Model {
    public abstract class ThingSerializerBase:SerializerBase {
        private XDocument _document = new XDocument();
        public virtual XDocument Document {
            get {
                if (_document == null) {
                    _document = new XDocument();

                    //XDocumentType doctype = new XDocumentType("limada", null, null, "<!ELEMENT limada.things ANY>");
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



        private ICollection<IThing> _thingCollection = null;
        public virtual ICollection<IThing> ThingCollection {
            get {
                if (_thingCollection == null && Graph != null) {
                    _thingCollection = new Set<IThing>();
                }
                return _thingCollection;
            }
            set { _thingCollection = value; }
        }

        public abstract XmlReader CreateReader ( Stream s );
        public abstract XmlWriter CreateWriter (Stream s);

        public abstract void Write ( Stream s );

        public abstract void Read ( Stream s );
    }
}