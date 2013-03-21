using System.IO;

namespace Limaki.Model.Content {
    public interface IConverter {
        long SourceType { get; set; }
        long ResultType { get; set; }

        Stream Source { get; set; }
        string StringSource { set; }

        Stream Result { get; }
        string StringResult { get; }

        void Read ();
        void Write ();
        void RemovePmTags ();
        void Clear ();
    }
}