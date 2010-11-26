using System;

using System.Collections.Generic;

namespace Limaki.Common.Text.HTML {


    /**Ein Tag besteht aus:
     * Name
     * Attribut
     * Wert
     * oder
     * Text, falls der Tag tatsächlich ein Text zwischen zwei Tags ist
     * Beginn des Tags im HTML-Text
     * Ende des Tags im HTML-Text
     * Tag -> Text = null; Text -> Name = null (Achtung: Daher haben HTML-Kommentare keinen Text, sie sind ja Tags. Der Text befindet sich im Tag-Name*/
    public class Tag {

        public string Name { get;  set; }

        
        int _start = -1;
        /**An dieser Position innerhalb der HTML-Datei beginnt dieser Tag*/
        public int Start { get { return _start; }  set { _start = value; } }
        
        int _end = -1;
        /**An dieser Position innerhalb der HTML-Datei endet dieser Tag*/
        public int End { get { return _end; } set { _end = value; } }

        /**Liste der Attribute des tags*/
        ICollection<string> _attributes = null;
        public ICollection<string> Attributes {
            get {
                if (_attributes == null)
                    _attributes = new List<String>();
                return _attributes;
            }
            protected set { _attributes = value; }
        }
        /**Liste der Werte zu den Attributen*/
        ICollection<string> _values = null;
        public ICollection<string> Values {
            get {
                if (_values == null)
                    _values = new List<String>();
                return _values;
            }
            protected set { _values = value; }
        }


        /**Falls der Tag tatsächlich ein Text zwischen zwei Tags ist: Hier der Text*/
        public string Text { get; protected set; }

        /**Konstruktor für einen Tag, der tatsächlich Text zwischen Tags ist*/
        public Tag(string text) {
            this.Text = text;
        }
        /**Konstruktor für einen Tag, der tatsächlich Text zwischen Tags ist*/
        public Tag(string text, int start, int end) {
            this.Text = text;
            this.Start = start;
            this.End = end;
        }

        /**Konstruktor für einen Tag*/
        public Tag(string name, int start, int end, ICollection<string> attributes, ICollection<string> values) {
            this.Name = name;
            this.Start = start;
            this.End = end;
            this.Attributes = attributes;
            this.Values = values;
        }



    }
}