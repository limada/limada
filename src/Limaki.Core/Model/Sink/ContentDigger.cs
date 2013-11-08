using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Limaki.Model.Content.IO {

    public interface IContentDigger : ISink<Content<Stream>, Content<Stream>> { }

    public class ContentDigger : IContentDigger {
        public ContentDigger () { }
        public ContentDigger (Func<Content<Stream>, Content<Stream>, Content<Stream>> digg) {
            this.DiggUse = digg;
        }
        public virtual Content<Stream> Use (Content<Stream> source) {
            return Use(source, source);
        }

        public virtual Content<Stream> Use (Content<Stream> source, Content<Stream> sink) {
            if (DiggUse != null)
                return DiggUse(source, sink);
            return sink;
        }

        public Func<Content<Stream>, Content<Stream>, Content<Stream>> DiggUse { get; protected set; }
    }

    public class ContentDiggProvider : SinkProvider<Content<Stream>, Content<Stream>> {
        public virtual Content<Stream> Use (Content<Stream> content) {
            return base.Use(content, content);
        }
    }
}
