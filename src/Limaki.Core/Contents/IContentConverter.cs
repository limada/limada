using System;
using System.Collections.Generic;
using System.IO;
using Limaki.Contents.IO;
using Limaki.Model.Content;

namespace Limaki.Contents {
    public interface IContentConverter<T>:IPipe<Content<T>,Content<T>> {
        /// <summary>
        /// a list of ContentTypes, where Item1 is source, Item2 is sink-Type
        /// </summary>
        IEnumerable<Tuple<long, long>> SupportedTypes { get; }

        Content<T> Use (Content<T> source, long sinkType);
    }
}