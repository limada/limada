using System.Globalization;
using System.Xml.Linq;
using System;
using Limaki.Common;

namespace Limaki.Common {
    public class SerializerBase {
        public virtual XAttribute Write<T>(T item) {
            return new XAttribute(this.GetType().Name, item.ToString());
        }
        public virtual XAttribute Write<T>(string name, T item) {
            return new XAttribute(name, item.ToString());
        }

        public virtual Pair<string, string> ReadTuple(XElement node, string attribute) {
            var result = new Pair<string, string>();
            string s = node.Attribute(attribute).Value;
            var pos = s.IndexOf (",");
            result.One = s.Substring (1, pos - 1);
            result.Two = s.Substring (pos + 1, s.Length - 2 - pos);
            return result;
        }
        
        //public virtual XAttribute WriteTuple(Pair<T, T> item, string attribute) {
        //    return new XAttribute (attribute, item.ToString());
            
        //}

        public long ReadInt(XElement node, string attribute, bool hex) {
            long result = default(int);
            string s = node.Attribute(attribute).Value;
            if (!string.IsNullOrEmpty(s)) {
                if (hex)
                    long.TryParse(s, NumberStyles.AllowHexSpecifier, null, out result);
                else
                    long.TryParse(s, out result);
            }
            return result;
        }

        public XAttribute WriteInt(int value, string attribute, bool hex) {
            if (hex) {
                return new XAttribute (attribute, value.ToString ("X"));
            } else {
                return new XAttribute(attribute, value.ToString());
            }
        }

        public double ReadDouble(XElement node, string attribute) {
            double result = default(int);
            string s = node.Attribute(attribute).Value;
            if (!string.IsNullOrEmpty(s)) {
                double.TryParse(s, out result);
            }
            return result;
        }

        public string ReadString(XElement node, string attribute) {
            var att = node.Attribute (attribute);
            if (att != null) {
                return att.Value;
            }
            return null;
        }

        public virtual bool ReadBool(XElement node, string attribute) {
            bool result = false;
            bool.TryParse (node.Attribute(attribute).Value, out result);
            return result;
        }

        public virtual T ReadEnum<T>(string value) {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}