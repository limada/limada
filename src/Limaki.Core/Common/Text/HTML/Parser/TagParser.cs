using System;

namespace Limaki.Common.Text.HTML.Parser {
    /*  Hier werden Tags rausgefischt. Tags zeichnen sich so aus:
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
        public TagParser(string content): base(content) {
        }

        public TagParser(Stuff stuff)
            : base(stuff) {
        }

        private void Go() {
            _stuff.Status = Status.Text;
            while (_stuff.Position < _stuff.Text.Length) {
                if (_stop) {
                    break;
                }
                _actual = _stuff.Text[_stuff.Position]; //Position ist das Zeichen an der aktuellen Stelle
                Watch(); //Auf zur Untersuchung von Position und status
                _stuff.Position++; //Auf zum nächsten Zeichen
            }
        }


        public Action<Stuff> DoElement;

        private void OnElement() {
            if (DoElement != null) {
                DoElement(_stuff);
            }
        }

        public Action<Stuff> DoTag;

        private void OnTag() {
            if (DoTag != null) {
                DoTag(_stuff);
            }
        }

        public Action<Stuff> DoText;

        private void OnText() {
            if (DoText != null) {
                DoText(_stuff);
            }
        }


        private void Watch() {
            //Bei diesen Zeichen ist u.U. etwas zu unternehmen
            if (_stuff.Status.Equals(Status.Prename)) {
                if (Letter(_actual)) {
                    State = Status.Text;
                    _stuff.Position--;
                    OnText();
                    _stuff.Position++;
                    _stuff.Origin = _stuff.Position;
                    State = Status.Name;
                }
            }
            if (_actual.Equals('<')) {
                //Möglicherweise beginnt ein Tag
                _stuff.TagPosition = _stuff.Position;
                State = Status.Prename;
            } else if (_actual.Equals('>')) {
                if (_stuff.Status.Equals(Status.Text) == false) {
                    if (_stuff.Status.Equals(Status.Cite) == false) {
                        //Ein Tag endet
                        OnElement();
                        _stuff.Position++;
                        OnTag();
                        _stuff.Position--;
                        _stuff.Origin = _stuff.Position + 1;
                        State = Status.Text;
                    }
                }
            } else if (_actual.Equals('!')) {
                if (_stuff.Status.Equals(Status.Prename)) {
                    State = Status.Text;
                    _stuff.Position--;
                    OnText();
                    _stuff.Position++;
                    _stuff.Origin = _stuff.Position;
                    State = Status.Commenttag;
                }
            } else if (_actual.Equals('/')) {
                if (_stuff.Status.Equals(Status.Prename)) {
                    State = Status.Text;
                    _stuff.Position--;
                    OnText();
                    _stuff.Position++;
                    _stuff.Origin = _stuff.Position;
                    State = Status.Endtag;
                } else if (_stuff.Status.Equals(Status.Name)) {
                    State = Status.Solotag;
                } else if (_stuff.Status.Equals(Status.Value)) {
                    State = Status.Solotag;
                } else if (_stuff.Status.Equals(Status.Attribute)) {
                    //Sollte nicht sein: Tag endet mit Attribut-Namen und Slash
                    State = Status.Solotag;
                }
            } else if (_actual.Equals('\"')) {
                if (_stuff.Status.Equals(Status.Value)) {
                    State = Status.Cite;
                } else if (_stuff.Status.Equals(Status.Cite)) {
                    State = Status.Value;
                }
            } else if (_actual.Equals('=')) {
                if (_stuff.Status.Equals(Status.Attribute)) {
                    OnElement();
                    _stuff.Origin = _stuff.Position + 1;
                    State = Status.Value;
                }
            } else if (_actual.Equals(' ')) {
                if (_stuff.Status.Equals(Status.Prename)) {
                    State = Status.Text;
                }
                if (_stuff.Status.Equals(Status.Name)) {
                    OnElement();
                    _stuff.Origin = _stuff.Position + 1;
                    State = Status.Attribute;
                }
                if (_stuff.Status.Equals(Status.Value)) {
                    OnElement();
                    _stuff.Origin = _stuff.Position + 1;
                    State = Status.Attribute;
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
                    _stuff.Position = starts;
                    Go();
                }
            }
        }

        public override bool Remove(int from, int to) {
            bool result = base.Remove(from, to);
            if (result && _stuff.Status.Equals(Status.Text) == false) {
                State = Status.Text;
            }
            return result;
        }
    }
}