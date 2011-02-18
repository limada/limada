using System;

namespace Limaki.Common.Text.HTML.Parser {
    /*  Hier werden tags rausgefischt. Tags zeichnen sich so aus:
     *  Tag			beginnt mit '<', gefolgt von einem Buchstaben.
     *  Tagname		beginnt unmittelbar nach dem '<' bzw. dem '/' bei Endtags
     *  Attribut	beginnt nach dem ' '
     *  Value		beginnt nach einem '="
     *  Folgende Tags sind möglich:
     *  Einführender Tag
     *				Nur Name
     *				Mit Attributen und Values
     *	Ausführender Tag
     *				Nur Name (aber egal)
     *	Commenttag
     *				Beginnt mit '!'
     *	Solotag
     *				Endet mit '/' */

    /// <summary>
    /// Der Parser durchläuft einen HTML-formatierten Text, der als Stringbuilder vorliegt,
    /// und benachrichtigt die aufrufende Klasse am Ende jedes Tag(teil)s bzw. Textteils. 
    /// </summary>
    public class TagParser : ParserBase {
        public TagParser(string content)
            : base(content) {
        }

        public TagParser(Stuff stuff)
            : base(stuff) {
        }

        private void Go() {
            _stuff.Status = Status.Text;
            while (_stuff.ActAt < _stuff.Text.Length) {
                if (_stop) {
                    break;
                }
                _actual = _stuff.Text[_stuff.ActAt]; //actual ist das Zeichen an der aktuellen Stelle
                Watch(); //Auf zur Untersuchung von actual und status
                _stuff.ActAt++; //Auf zum nächsten Zeichen
            }
        }


        public Action<Stuff> Element;

        private void OnElement() {
            if (Element != null) {
                Element(_stuff);
            }
        }

        public Action<Stuff> Tag;

        private void OnTag() {
            if (Tag != null) {
                Tag(_stuff);
            }
        }

        public Action<Stuff> Text;

        private void OnText() {
            if (Text != null) {
                Text(_stuff);
            }
        }


        private void Watch() {
            //Bei diesen Zeichen ist u.U. etwas zu unternehmen
            if (_stuff.Status.Equals(Status.Prename)) {
                if (Letter(_actual)) {
                    State(Status.Text);
                    _stuff.ActAt--;
                    OnText();
                    _stuff.ActAt++;
                    _stuff.StartAt = _stuff.ActAt;
                    State(Status.Name);
                }
            }
            if (_actual.Equals('<')) {
                //Möglicherweise beginnt ein Tag
                _stuff.StartTag = _stuff.ActAt;
                State(Status.Prename);
            } else if (_actual.Equals('>')) {
                if (_stuff.Status.Equals(Status.Text) == false) {
                    if (_stuff.Status.Equals(Status.Cite) == false) {
                        //Ein Tag endet
                        OnElement();
                        _stuff.ActAt++;
                        OnTag();
                        _stuff.ActAt--;
                        _stuff.StartAt = _stuff.ActAt + 1;
                        State(Status.Text);
                    }
                }
            } else if (_actual.Equals('!')) {
                if (_stuff.Status.Equals(Status.Prename)) {
                    State(Status.Text);
                    _stuff.ActAt--;
                    OnText();
                    _stuff.ActAt++;
                    _stuff.StartAt = _stuff.ActAt;
                    State(Status.Commenttag);
                }
            } else if (_actual.Equals('/')) {
                if (_stuff.Status.Equals(Status.Prename)) {
                    State(Status.Text);
                    _stuff.ActAt--;
                    OnText();
                    _stuff.ActAt++;
                    _stuff.StartAt = _stuff.ActAt;
                    State(Status.Endtag);
                } else if (_stuff.Status.Equals(Status.Name)) {
                    State(Status.Solotag);
                } else if (_stuff.Status.Equals(Status.Value)) {
                    State(Status.Solotag);
                } else if (_stuff.Status.Equals(Status.Attribute)) {
                    //Sollte nicht sein: Tag endet mit Attribut-Namen und Slash
                    State(Status.Solotag);
                }
            } else if (_actual.Equals('\"')) {
                if (_stuff.Status.Equals(Status.Value)) {
                    State(Status.Cite);
                } else if (_stuff.Status.Equals(Status.Cite)) {
                    State(Status.Value);
                }
            } else if (_actual.Equals('=')) {
                if (_stuff.Status.Equals(Status.Attribute)) {
                    OnElement();
                    _stuff.StartAt = _stuff.ActAt + 1;
                    State(Status.Value);
                }
            } else if (_actual.Equals(' ')) {
                if (_stuff.Status.Equals(Status.Prename)) {
                    State(Status.Text);
                }
                if (_stuff.Status.Equals(Status.Name)) {
                    OnElement();
                    _stuff.StartAt = _stuff.ActAt + 1;
                    State(Status.Attribute);
                }
                if (_stuff.Status.Equals(Status.Value)) {
                    OnElement();
                    _stuff.StartAt = _stuff.ActAt + 1;
                    State(Status.Attribute);
                }
            }
        }

        public void Parse() {
            if (_stuff != null) {
                Go();
            }
        }
        public void Parse(int starts) {
            if (_stuff != null) {
                if (starts < _stuff.Text.Length) {
                    _stuff.ActAt = starts;
                    Go();
                }
            }
        }

        public override bool Remove(int from, int to) {
            bool result = base.Remove(from, to);
            if (result && _stuff.Status.Equals(Status.Text) == false) {
                State(Status.Text);
            }
            return result;
        }
    }
}