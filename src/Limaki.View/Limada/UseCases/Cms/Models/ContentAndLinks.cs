using Limaki.Contents;
using System.Collections.Generic;

namespace Limada.UseCases.Cms.Models {

    public class ContentAndLinks {
        public Content Content { get; set; }
        public IEnumerable<Href> Roots { get; set; }
        public IEnumerable<Href> Leafs { get; set; }
    }
}