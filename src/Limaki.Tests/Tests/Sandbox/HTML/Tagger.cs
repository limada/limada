namespace Limaki.Common.Text.HTML {
    using System;

    using System.Collections.Generic;
    using System.Text;
    /**Tagger zerlegt einen HTML-Text in
     * Tags (das sind sowohl Tags als auch Texte zwischen den Tags) und
     * Styles (das ist die Sammlung aller Texte zwischen allen style-Tags im HTML-Text.
     * Der Vorgang kann auf x Zeichen reinen Text (d.h. Text nach dem body-Tag) in der
     * Datei eingeschränkt werden.*/
 

    public class Tagger {

        /**Die HTML-Datei als string*/
        private string content = null;

        private List<Tag> _tags = new List<Tag> ();
        private List<Style> _styles = new List<Style> ();

        /**Vorzeitiger, erwünschter Abbruch bei x Zeichen Text im body*/
        private int textLength = -1;

        /**Erstes und letztes Zeichen des Strings sowie Länge der reinen Textes, der zurückgeliefert wird*/
        public int[] FromTo = new int[3];

        /**Konstruktor mit Übergabe des Inhalts der HTML-Datei als string*/

        public Tagger ( string HTMLcontent ) {
            this.content = HTMLcontent;
            setTagging ();
        }

        /**Konstruktor mit Übergabe des Inhalts der HTML-Datei als string sowie der gewünschten Länge reinen Textes, der zurückgegeben werden soll*/

        public Tagger ( string HTMLcontent, int textLength ) {
            this.textLength = textLength;
            this.content = HTMLcontent;
            setTagging ();
        }

        /**Generiert tags und styles aus dem Inhalt eines HTML-Dokuments*/

        private void setTagging() {
            //Aus dem string content eine Liste von Tags generieren (Texte zwischen tags ist hier ebenfalls ein tag mit dem Namen = null)
            if (this.content != null) {
                setTags ();
                //Style: den tag zwischen style und /style auswerten
                List<string> styleTags = setStyleTags ();
                setStyles (styleTags);
            }
        }



        /**Anhand von Content wird der Text in Tags gelagert, wobei:
         * Tags mit "name" HTML-Tags sind und
         * Tags mit "text" HTML-Text zwischen den Tags sind*/

        private void setTags() {
            /**In tag werden die Daten geschrieben*/
            Tag tag = null;
            /**startedAt gibt an, wo zuletzt Daten gespeichert wurden*/
            int startedAt = 0;
            /**lastBlank verweist immer auf den zuletzt gefundenen blank*/
            int lastBlank = 0;
            /**isBlank bedeutet, dass lastBlank verwendet werden darf: in einem Tag wurde bereits ein blank gefunden*/
            bool isBlank = false;
            /**isTag bedeutet, dass ein Tag durchforstet wird*/
            bool isTag = false;
            /**isName bedeutet, dass bisher noch keine Daten aus dem Tag gespeichert wurden*/
            bool isName = false;
            /**isAttribute bedeutet, dass zuletzt der Name oder ein Value gespeichert wurde*/
            bool isAttribute = false;
            /**actualChar ist ein string auf das Zeichen(i)*/
            char actualChar = default( char );
            /**Die ersten vier Zeichen sind am Anfang und am Ende eines Textes, alle am Anfang/Ende eines TagName, Value und Attribute verboten*/
            string noSuchChar = " " + "\r" + "\n" + "\t" + "\"" + ">" + "<" + "\'" + "=";
            /**isForbidden hält fest, ob die verpönten Anfangszeichen vorbei sind, d.h. es hat der reguläre Ausdruck begonnen*/
            bool isTagForbidden = true;
            /**startedAllowed legt fest, ab wo ein Teilstring in den Text aufgenommen werden darf*/
            int startedAllowed = 0;
            /**startedAllowed legt fest, ab wo ein Teilstring in einem Tag aufgenommen werden darf*/
            int startedTagAllowed = 0;
            /**endedAllowed legt fest, bis wohin ein Teilstring in den Text aufgenommen werden darf*/
            int endedAllowed = 0;
            int lastEndedAllowed = 0;
            /**TagName speichert den TagName zwischen*/
            string tagName = "";
            /**Hier die Arrays, die dem tag übergeben werden*/
            List<string> attributes = new List<string> ();
            List<string> values = new List<string> ();
            /**Hier wird der Content durchsucht*/
            for (int i = 0; i < this.content.Length; i++) {
                actualChar = this.content[i];
                /**Hier wird untersucht, ob einführende bzw. ausführende verpönte Zeichen vorkommen*/
                if (isTagForbidden) {
                    bool hasTagChanged = false;
                    for (int j = 0; j < noSuchChar.Length - 1; j++) {
                        if (actualChar == noSuchChar[j]) {
                            if (j < 6) {
                                //Original: 5
                                startedAllowed = i;
                            }
                            startedTagAllowed = i;
                            hasTagChanged = true;
                            break;
                        }
                    }
                    isTagForbidden = hasTagChanged;
                }
                bool isAllowed = true;
                for (int j = 0; j < 7; j++) {
                    /**Solange ein erlaubtes Zeichen gefunden wird, erhöht sich endedAllowed*/
                    if (actualChar == noSuchChar[j]) {
                        lastEndedAllowed = endedAllowed;
                        isAllowed = false;
                        break;
                    }
                }
                if (isAllowed) {
                    endedAllowed = i;
                }
                /**Hier wird auf Aufnahme von tags untersucht*/
                if (actualChar.Equals (' ')) {
                    /**Den letzten Blank für die Trennung von Name und Attribut bzw. Value und Attribut merken*/
                    lastBlank = i;
                    isBlank = true;
                }
                /**Text wird in einen Tag aufgenommen*/
                else if (actualChar.Equals ('<')) {
                    /**Hier endet ein Textteil und es beginnt ein Tag*/
                    if (i > 0) {
                        if (( startedAt + 1 ) < ( i - 1 )) {
                            /**Der Text wird in einen tag übernommen*/
                            if (startedAllowed > startedAt) {
                                if (( startedAllowed + 1 ) < ( i - 1 )) {
                                    tag =
                                        new Tag (
                                            this.content.Substring (startedAllowed + 1,
                                                                    endedAllowed - startedAllowed).Trim (),
                                            startedAt, i);
                                    this._tags.Add (tag); //Hier die Texte
                                    /**Wenn nur ein Textteil zurückgegeben werden soll*/
                                    if (this.FromTo[0] > 0) {
                                        this.FromTo[2] = this.FromTo[2] + tag.Text.Length;
                                        /**Wenn die angeforderte Textlänge erreicht wurde*/
                                        if (this.textLength > -1) {
                                            if (this.FromTo[2] > this.textLength) {
                                                this.FromTo[1] = i;
                                                break;
                                            }
                                        }
                                    }
                                } else {
                                    /**Annahme: hier handelt es sich um einen string, der ausschließlich aus verpönten Zeichen besteht*/
                                }
                            } else {
                                tag =
                                    new Tag (
                                        this.content.Substring(startedAt + 1, endedAllowed - startedAt).Trim(),
                                        startedAt, i);
                                this._tags.Add (tag); //Hier die Texte
                                /**Wenn nur ein Textteil zurückgegeben werden soll*/
                                if (this.FromTo[0] > 0) {
                                    this.FromTo[2] = this.FromTo[2] + tag.Text.Length;
                                    /**Wenn die angeforderte Textlänge erreicht wurde*/
                                    if (this.textLength > -1) {
                                        if (this.FromTo[2] > this.textLength) {
                                            this.FromTo[1] = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        } else {
                            /**Hier sind kurze Strings, die vernachlässigt werden*/
                        }
                    }
                    /**Ein Tag beginnt immer mit einem Name*/
                    startedAt = i;
                    isTag = true;
                    isName = true;
                    isBlank = false;
                }
                    /**Tag wird in einen tag aufgenommen*/
                else if (actualChar.Equals ('>')) {
                    /**Hier endet ein Tag und es beginnt ein Textteil*/
                    if (isName) {
                        /**In diesem Tag war nur ein Name*/
                        if (startedTagAllowed > startedAt) {
                            tagName = this.content.Substring(startedTagAllowed + 1, endedAllowed - startedTagAllowed).Trim();
                        } else {
                            tagName = this.content.Substring(startedAt + 1, endedAllowed - startedAt).Trim();
                        }
                    } else if (isAttribute) {
                        /**Es kann sich nur um den letzten Value eines Tags handeln*/
                        if (startedTagAllowed <= endedAllowed) {
                            values.Add (
                                this.content.Substring(startedTagAllowed + 1, endedAllowed - startedTagAllowed).Trim());
                        } else {
                            values.Add(this.content.Substring(startedAt, endedAllowed - startedAt-1).Trim());
                        }
                    } else {
                        /**Hier kann es sich nach der Ausschliessungsmethode nur um einen Value handeln*/
                        if (startedTagAllowed > startedAt) {
                            if (startedTagAllowed >= endedAllowed) {
                                values.Add ("");
                            } else {
                                values.Add (this.content.Substring (startedTagAllowed, endedAllowed-startedTagAllowed-1).Trim());
                            }
                        } else {
                            values.Add(this.content.Substring(startedAt, endedAllowed - startedAt-1).Trim());
                        }
                    }
                    /**Wenn ein Tag abgeschlossen ist, muss er einem neuen tag übergeben werden*/
                    tag = new Tag (tagName, startedAt, i, attributes, values);
                    this._tags.Add (tag);
                    /**Wenn nur eine bestimmte Textsequenz zurückgegeben werden soll, dann beginnt sie hier*/
                    if (tagName.Equals ("body", StringComparison.OrdinalIgnoreCase)) {
                        this.FromTo[0] = i + 1;
                    }
                    if (tagName.Equals ("/body", StringComparison.OrdinalIgnoreCase)) {
                        this.FromTo[1] = startedAt - 1;
                    }
                    /**Alle Tag-Parameter werden zurückgesetzt und die neuen Elemente für den nächsten Tag erzeugt*/
                    attributes = new List<string> ();
                    values = new List<string> ();
                    isTag = false;
                    isName = false;
                    isAttribute = false;
                    isBlank = false;
                    isTagForbidden = true;
                    startedAt = i;
                }
                if (isTag) {
                    /**Hier wird das Trennzeichen zwischen Attribute und Value gefunden*/
                    if (actualChar.Equals ('=')) {
                        if (isAttribute) {
                            /**Text vor = - wenn es kein Name ist, teilt sich in Value und Attribute, wobei Attribute beim letzten blank beginnt und alles davor Attribute ist*/
                            if (isBlank) {
                                if (startedTagAllowed > startedAt) {
                                    if (( startedTagAllowed + 1 ) >= lastBlank) {
                                        values.Add ("");
                                        attributes.Add(this.content.Substring(lastBlank + 1, endedAllowed - lastBlank-1));
                                    } else {
                                        values.Add(this.content.Substring(startedTagAllowed + 1, lastEndedAllowed - startedTagAllowed));
                                        attributes.Add(this.content.Substring(lastBlank + 1, endedAllowed - lastBlank-1));
                                    }
                                } else {
                                    values.Add(this.content.Substring(startedTagAllowed, endedAllowed - startedTagAllowed-1));
                                    attributes.Add(this.content.Substring(lastBlank, endedAllowed - lastBlank-1));
                                }
                            } else {
                                /**Hier muss ein Fehler vorliegen: es gibt kein Trennzeichen zwischen Attribut und Value*/
                            }
                        } else {
                            /**Wenn es sich um das erste Attribut handelt, dann gibt es davor noch keinen Value, sondern den Name, und der wurde bereits aufgenommen*/
                            if (startedTagAllowed > startedAt) {
                                tagName = this.content.Substring(startedTagAllowed, endedAllowed - startedTagAllowed-1).Trim();
                                attributes.Add(this.content.Substring(startedTagAllowed, endedAllowed - startedTagAllowed-1));
                            } else {
                                if (startedAt >= lastBlank) {
                                    System.Console.WriteLine ("Fehler in HTML.setTags: startedAt: " + startedAt +
                                                              " lastBlank: " + lastBlank + " i: " + i);
                                } else {
                                    tagName = this.content.Substring(startedAt + 1, lastBlank - startedAt).Trim();
                                    attributes.Add(this.content.Substring(lastBlank + 1, endedAllowed - lastBlank-1));
                                }
                            }
                        }
                        isName = false;
                        isBlank = false;
                        isAttribute = true;
                        startedAt = i;
                        isTagForbidden = true;
                    }
                }
            }
        }

        /**Sammelt den Text zwischen dem "style"-Tag und dem "style"-Endetag in eine List "StyleTags"*/

        private List<string> setStyleTags() {
            List<string> result = new List<string> ();
            bool isNext = false;
            if (this._tags.Count > 0) {
                foreach (Tag tag in this._tags) {
                    if (isNext) {
                        if (tag.Name == null) {
                            result.Add (tag.Text);
                        } else if (tag.Name.Substring (0, 3).Equals ("!--")) {
                            result.Add (tag.Name);
                        }
                        isNext = false;
                    }
                    if (tag.Name != null) {
                        if (tag.Name.Equals ("style", StringComparison.OrdinalIgnoreCase)) {
                            isNext = true;
                        }
                    }
                }
            }
            return result;
        }

        /**Erstellt für jeden Eintrag in der List "StyleTags" ein Style-Objekt*/

        private void setStyles ( IEnumerable<string> source ) {
            foreach (string result in source) {
                setStyles (result);
            }
        }

        public string CutComments(string source) {
            var sb = new StringBuilder(source);
            var last = default(char);
            var start = -1;
            for (int i = 0; i < sb.Length; i++) {
                var c = sb[i];
                if (last == '/' && c == '*')
                    start = i - 1;
                if (start != -1 && last == '*' && c == '/') {
                    var len = i - start + 1;
                    sb.Remove(start, len);
                    i -= len;
                    start = -1;
                }
                last = sb[i];
            }
            if (start != -1)
                sb.Remove(start, sb.Length - start);
            return sb.ToString();
        }

        /**Erstellt aus einem Style-Tag-Text ein Style-Objekt*/

        private void setStyles ( string source ) {
            //Nach diesen Chars wird gesucht
            char EOName = '{';
            char EOAttr = ':';
            char EOVal = ';';
            char EOStyle = '}';
            int charAt = 0;
            Style style = new Style ();
            string attribute = null;

            source = CutComments (source);

            /**Style-Name, Attribute und Werte werden erstellt*/
            if (source.Length > 0) {
                for (int i = 0; i < source.Length; i++) {
                    char c = source[i];
                    if (c == EOStyle) {
                        this._styles.Add (style);
                        style = new Style ();
                    } else if (c == EOVal) {
                        style.SetAttribute (attribute, source.Substring (charAt, i-charAt).Trim(wipes));
                        charAt = i++;
                    } else if (c == EOAttr) {
                        attribute = source.Substring(charAt, i - charAt).Trim(wipes);
                        charAt = i++;
                    } else if (c == EOName) {
                        style.Name = source.Substring(charAt, i - charAt).Trim(wipes);
                        charAt = i++;
                    }
                }
            }
        }

        private char[] wipes = new char[] { '!', '-', '\r', '\n', '\t', ' ', ':', '{', '}', ';' };
        
        /**Gibt die Liste der Tags zurück (d.h. auch der Texte zwischen den Tags)*/
        public ICollection<Tag> Tags {
            get { return this._tags; }
        }
        /**Gibt die Liste der Styles zurück*/
        public ICollection<Style> Styles {
            get { return this._styles; }
        }

        /**Gibt HTML-Text ab nach dem body-Tag zurück*/
        public string Body {
            get { return this.content.Substring(this.FromTo[0], this.FromTo[1] - this.FromTo[0]-1); }
        }
    }

    public struct FromTo {
        public int Start;
        public int End;
        public int Length;
    }
}