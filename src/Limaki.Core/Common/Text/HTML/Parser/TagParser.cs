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
            _stuff.State = Parser.State.Text;
            while (_stuff.Position < _stuff.Text.Length) {
                if (_stop) {
                    break;
                }
                _actual = _stuff.Text[_stuff.Position]; //Position ist das Zeichen an der aktuellen Stelle
                Watch(); //Auf zur Untersuchung von Position und State
                _stuff.Position++; //Auf zum nächsten Zeichen
            }
        }


        public Action<Stuff> DoElement;

        private void OnElement() {
            if (DoElement != null) {
                DoElement(_stuff);
            }
        }

        /// <summary>
        /// a tag was parsed; either <tag> or </tag>
        /// </summary>
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
            if (_stuff.State.Equals(Parser.State.Prename)) {
                if (Letter(_actual)) {
                    State = Parser.State.Text;
                    _stuff.Position--;
                    OnText();
                    _stuff.Position++;
                    _stuff.Origin = _stuff.Position;
                    State = Parser.State.Name;
                }
            }
            if (_actual.Equals('<')) {
                //Möglicherweise beginnt ein Tag
                _stuff.TagPosition = _stuff.Position;
                State = Parser.State.Prename;
            } else if (_actual.Equals('>')) {
                if (_stuff.State.Equals(Parser.State.Text) == false) {
                    if (_stuff.State.Equals(Parser.State.Cite) == false) {
                        //Ein Tag endet
                        OnElement();
                        _stuff.Position++;
                        OnTag();
                        _stuff.Position--;
                        _stuff.Origin = _stuff.Position + 1;
                        State = Parser.State.Text;
                    }
                }
            } else if (_actual.Equals('!')) {
                if (_stuff.State.Equals(Parser.State.Prename)) {
                    State = Parser.State.Text;
                    _stuff.Position--;
                    OnText();
                    _stuff.Position++;
                    _stuff.Origin = _stuff.Position;
                    State = Parser.State.Commenttag;
                }
            } else if (_actual.Equals('/')) {
                if (_stuff.State.Equals(Parser.State.Prename)) {
                    State = Parser.State.Text;
                    _stuff.Position--;
                    OnText();
                    _stuff.Position++;
                    _stuff.Origin = _stuff.Position;
                    State = Parser.State.Endtag;
                } else if (_stuff.State.Equals(Parser.State.Name)) {
                    State = Parser.State.Solotag;
                } else if (_stuff.State.Equals(Parser.State.Value)) {
                    State = Parser.State.Solotag;
                } else if (_stuff.State.Equals(Parser.State.Attribute)) {
                    //Sollte nicht sein: Tag endet mit Attribut-Namen und Slash
                    State = Parser.State.Solotag;
                }
            } else if (_actual.Equals('\"')) {
                if (_stuff.State.Equals(Parser.State.Value)) {
                    State = Parser.State.Cite;
                } else if (_stuff.State.Equals(Parser.State.Cite)) {
                    State = Parser.State.Value;
                }
            } else if (_actual.Equals('=')) {
                if (_stuff.State.Equals(Parser.State.Attribute)) {
                    OnElement();
                    _stuff.Origin = _stuff.Position + 1;
                    State = Parser.State.Value;
                }
            } else if (_actual.Equals(' ')) {
                if (_stuff.State.Equals(Parser.State.Prename)) {
                    State = Parser.State.Text;
                }
                if (_stuff.State.Equals(Parser.State.Name)) {
                    OnElement();
                    _stuff.Origin = _stuff.Position + 1;
                    State = Parser.State.Attribute;
                }
                if (_stuff.State.Equals(Parser.State.Value)) {
                    OnElement();
                    _stuff.Origin = _stuff.Position + 1;
                    State = Parser.State.Attribute;
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
            if (result && _stuff.State.Equals(Parser.State.Text) == false) {
                State = Parser.State.Text;
            }
            return result;
        }
    }
}