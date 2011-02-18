using System;

namespace Limaki.Common.Text.HTML.Parser {
    public class ParserBase {
        protected Stuff _stuff;
        protected char _actual;
        protected Boolean _stop;


        public ParserBase(string content) {
            SetContent(content);
        }

        public ParserBase(Stuff stuff) {
            _stuff = stuff;
        }

        public void SetContent(string content) {
            _stuff = new Stuff(content);
        }

        public void Stuff(Stuff stuff) {
            _stuff = stuff;
        }
        public Stuff Stuff() {
            return _stuff;
        }

        public void Stop() {
            if (_stop) {
                _stop = false;
            } else {
                _stop = true;
            }

        }

        public Boolean Replace(int from, int to, string with) {
            if (Remove(from, to)) {
                _stuff.Text.Insert(from, with);
                return true;
            }
            return false;
        }

        public virtual Boolean Remove(int from, int to) {
            if ((from < _stuff.Text.Length) && (to <= _stuff.Text.Length)) {
                _stuff.Text.Remove(from, to - from);
                _stuff.StartAt = from - 1;
                _stuff.ActAt = from - 1;
                _stuff.StartTag = _stuff.StartAt;

                return true;
            }
            return false;
        }


        protected void State(Status status) {
            _stuff.Status = status;
        }

        public static Boolean Letter(char source) {
            return (source >= 'a' && source <= 'z') || (source >= 'A' && source <= 'Z');
        }
    }
}