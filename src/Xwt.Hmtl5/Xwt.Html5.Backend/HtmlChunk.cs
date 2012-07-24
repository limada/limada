using System.IO;

namespace Xwt.Html5.Backend {

    public class HtmlChunk : IHtmlChunk {

        public string Target { get; protected set; }

        public HtmlChunk (string command) {
            this.Target = command;
        }

        TextWriter _content = null;

        protected TextWriter Content {
            get { return _content ?? (_content = new StringWriter ()); }
            set { _content = value; }
        }

        public void Write (string s) {
            Content.Write (s);
        }

        public void Write (string s, params object[] args) {
            Content.Write (s, args);
        }

        public void WriteLine (string s) {
            Content.WriteLine (s);
        }

        public void WriteLine (string s, params object[] args) {
            Content.WriteLine (s, args);
        }

        public virtual string Render () {
            return Content.ToString ();
        }

        public void Command (string s) {
            Content.Write (Target +"."+ s + ";");
        }

        public void Command (string s, params object[] args) {
            Content.Write (Target + "." + s + ";", args);
        }

        public void CommandLine (string s) {
            Content.WriteLine (Target + "." + s + ";");
        }

        public void CommandLine (string s, params object[] args) {
            Content.WriteLine (Target + "." + s + ";", args);
        }

      
    }
}