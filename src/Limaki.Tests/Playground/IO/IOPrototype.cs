using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Model.Content;
using Limaki.Common;
using Limaki.Model.Content.IO;

namespace Limaki.Playground.IO {
   

    public interface ISinkPipe<TSource, TOver, TSink> {
        TSink Sink(TSource source, ISink<TSource, TOver> sourceOver, ISink<TOver, TSink> overSink);
    }

    public interface IInOut<TSource, TSink> {
        TSource Source { get; set; }
        TSink Sink { get; set; }
    }
}
