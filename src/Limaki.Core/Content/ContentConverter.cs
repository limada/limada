using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Limaki.Model.Content {

    public class ConverterPool<T>:List<IContentConverter<T>> {
        public IContentConverter<T> Find(long source, long sink) {
            return this.Where (c => c.SupportedTypes.Any (t => t.Item1 == source && t.Item2 == sink))
                .FirstOrDefault();
        }
    }

    public abstract class ContentConverter<T> : IContentConverter<T> {
        public virtual Content<T> Use (Content<T> source) {
            var conv = SupportedTypes.Where(t => t.Item1 == source.ContentType);
            if (conv.Count() > 1 || conv.Count() == 0)
                throw new ArgumentException("No unique conversion found");
            return Use(source, conv.First().Item2);
        }

        public virtual Content<T> Use (Content<T> source, long sinkType) {
            return Use(source, new Content<T> { ContentType = sinkType });
        }

        public abstract IEnumerable<Tuple<long, long>> SupportedTypes { get; }

        public abstract Content<T> Use (Content<T> source, Content<T> sink);
    }
}