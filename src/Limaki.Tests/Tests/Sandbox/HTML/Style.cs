namespace Limaki.Common.Text.HTML {
    using System;

    using System.Collections.Generic;
    using System.Linq;

/**Ein Style-Eintrag besteht aus:
 * Name, nächstes Trennzeichen "{"
 * Attribut, nächtes Trennzeichen ":"
 * Wert, nächstes Trennzeichen ";"
 * Endezeichen "}"*/
public class Style {
	
    private IDictionary<string, string> attributes = new Dictionary<string, string> ();

    public Style(){}
	
	public Style(string name){
		this.Name = name;
	}

    public string Name { get; set; }

	public void SetAttribute(string attribute, string value){
		this.attributes[attribute.ToLower()]=value;
	}

	public string Value(string attribute){
	    var result = string.Empty;
	    attributes.TryGetValue (attribute.ToLower (), out result);
	    return result;
	}

    public IEnumerable<KeyValuePair<string,string>> AttributesAndValues {
        get {
            foreach (var item in attributes)
                yield return item;
        }
    }
    public ICollection<string> Attributes {
        get { return this.attributes.Keys; }
    }

    public ICollection<string> Values {
        get { return this.attributes.Values; }
    }
}
}