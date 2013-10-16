using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Limaki.View.DragDrop {
    public interface IContentEnrichManager : ISink<Content<Stream>, Content<Stream>> {}
}
