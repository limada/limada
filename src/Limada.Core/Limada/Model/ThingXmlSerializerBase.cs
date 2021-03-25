using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Limaki.Common;
using Limaki.Common.Collections;
using System.Globalization;

namespace Limada.Model {
	
    public abstract class ThingXmlSerializerBase:XmlSerializerBase {

        private XDocument _document;
        public virtual XDocument Document {
            get => _document ??= new XDocument ();
            set => _document = value;
        }

        public virtual IThingGraph Graph { get; set; }

        private ICollection<IThing> _thingCollection = null;
        public virtual ICollection<IThing> Things {
            get {
                if (_thingCollection == null && Graph != null) {
                    _thingCollection = new Set<IThing>();
                }
                return _thingCollection;
            }
            set => _thingCollection = value;
        }

        public abstract XmlReader CreateReader ( Stream s );
        public abstract XmlWriter CreateWriter (Stream s);

        public abstract void Write ( Stream s );

        public abstract void Read ( Stream s );
    }
}