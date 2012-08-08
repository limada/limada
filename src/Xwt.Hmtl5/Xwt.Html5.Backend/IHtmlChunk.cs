namespace Xwt.Html5.Backend {

    public interface IHtmlChunk {
        string Target { get; }
        void Write (string s);
        void Write (string s, params object[] args);
        void WriteLine (string s);
        void WriteLine (string s, params object[] args);

        void Command (string s);
        void Command (string s, params object[] args);
        void CommandLine (string s);
        void CommandLine (string s, params object[] args);

        string Html ();
    }

}