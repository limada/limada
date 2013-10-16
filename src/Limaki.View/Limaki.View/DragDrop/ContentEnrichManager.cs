using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Limaki.View.DragDrop {
    public class ContentEnrichManager : IContentEnrichManager {
        public Content<Stream> Use (Content<Stream> source) {
            throw new NotImplementedException();
        }

        public Content<Stream> Use (Content<Stream> source, Content<Stream> sink) {
            throw new NotImplementedException();
        }
    }
}
