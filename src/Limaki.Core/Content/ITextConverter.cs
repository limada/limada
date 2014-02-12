using System.IO;

namespace Limaki.Model.Content {
   
    public interface ITextConverter {
        long SourceType { get; set; }
        long ResultType { get; set; }

        Stream Source { get; set; }
        Stream Result { get; }

        string StringSource { set; }
        string StringResult { get; }

        void Read ();
        void Write ();
        void RemovePmTags ();
        void Clear ();
    }
}