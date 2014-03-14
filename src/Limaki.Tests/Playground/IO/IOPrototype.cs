using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Contents;
using Limaki.Model.Content;
using Limaki.Common;
using Limaki.Contents.IO;

namespace Limaki.Playground.IO {
   
    public interface ISinkPipe<TSource, TOver, TSink> {
        TSink Sink(TSource source, IPipe<TSource, TOver> sourceOver, IPipe<TOver, TSink> overSink);
    }

    public interface IInOut<TSource, TSink> {
        TSource Source { get; set; }
        TSink Sink { get; set; }
    }
}
