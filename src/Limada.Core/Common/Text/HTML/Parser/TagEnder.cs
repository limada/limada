using System.Collections.Generic;

namespace Limaki.Common.Text.HTML.Parser {

    public class TagEnder {
        private Stack<string> _stack;
        public Stack<string> Stack { get { return _stack; } }
        public TagEnder() {
            _stack = new Stack<string>();
        }

        public bool Set(string name, State state) {
           if (state == State.Endtag) {
                return Remove(name);
            } else {
                Add(name,state);
                return true;
            }
        }

        public string Name(string name) {
            name = name.Replace("/", "");
            if (name.StartsWith("<")) {
                var pos = name.IndexOf(" ");
                if (pos != -1) {
                    return name.Substring(1, pos - 1);
                } else {
                    return name.Substring(1, name.Length - 2);
                }
            }
            return name;
        }

        public bool Add(string tag, State state) {
            var result = (state == State.Name || state == State.Attribute || state == State.Value);
            if (result)
                _stack.Push(tag);
            return result;
        }

        public bool Remove(string tag) {
            var result = _stack.Count > 0 && tag == _stack.Peek();
            if (result) {
                _stack.Pop();
            }
            return result;
        }

        public string CloseTags() {
            var result = "";
            while (_stack.Count > 0) {
                result = result + "</" + _stack.Pop() + ">";
            }
            return result;
        }
        public void Clear() {
            _stack.Clear();
        }

        public string LastTag() {
            if (_stack.Count > 0)
                return _stack.Peek();
            return "";
        }

        public string CloseTag(string tag) {
            var result = "";
            while (_stack.Count > 0) {
                var atag = _stack.Peek();
                if (atag == tag)
                    break;
                _stack.Pop();
                result += "</" + atag + ">";
            }
            return result;
        }
    }
}