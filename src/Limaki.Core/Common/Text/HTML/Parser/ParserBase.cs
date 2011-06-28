using System;

namespace Limaki.Common.Text.HTML.Parser {
    public class ParserBase {
        protected Stuff _stuff;
        protected char _actual;
        protected bool _stop;


        public ParserBase(string content) {
            SetContent(content);
        }

        public ParserBase(Stuff stuff) {
            _stuff = stuff;
        }

        public void SetContent(string content) {
            _stuff = new Stuff(content);
        }

        
        public Stuff Stuff { get { return _stuff; } set { _stuff = value; } }

        public void Stop() {
            if (_stop) {
                _stop = false;
            } else {
                _stop = true;
            }

        }

        public bool Replace(int from, int to, string with) {
            if (Remove(from, to)) {
                _stuff.Text.Insert(from, with);
                return true;
            }
            return false;
        }

        public bool Insert(int from, string with) {
            var  len = with.Length;

            if (len >0 && from < _stuff.Text.Length){
                _stuff.Text.Insert(from, with);
                _stuff.Origin = from + len;
                _stuff.Position = _stuff.Origin;
                _stuff.TagPosition = _stuff.Origin;
                return true;
            }
            return false;

        }

        public virtual bool Remove(int from, int to) {
            if ((from < _stuff.Text.Length) && (to <= _stuff.Text.Length)) {
                _stuff.Text.Remove(from, to - from);
                _stuff.Origin = Math.Max(from - 1,0);
                _stuff.Position = _stuff.Origin;
                _stuff.TagPosition = _stuff.Origin;

                return true;
            }
            return false;
        }


        protected Status State { set { _stuff.Status = value; } }

        public static bool Letter(char source) {
            return (source >= 'a' && source <= 'z') || (source >= 'A' && source <= 'Z');
        }
    }
}