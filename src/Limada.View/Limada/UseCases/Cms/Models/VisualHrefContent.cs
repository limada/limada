using Limaki.View;
using Limaki.View.Visuals;
using System.Collections.Generic;

namespace Limada.Usecases.Cms.Models {

    public class VisualHrefContent {

        public IEnumerable<VisualHref> Hrefs { get; set; }
        public object Description { get; set; }
        public IEnumerable<Href> Roots { get; set; }
        public IEnumerable<Href> Leafs { get; set; }

    }
}