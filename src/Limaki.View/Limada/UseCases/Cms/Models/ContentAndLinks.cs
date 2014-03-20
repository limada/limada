using System.Collections.Generic;

namespace Limada.UseCases.Cms.Models {

    public class ContentAndLinks {
        public Limaki.Contents.Content Content { get; set; }
        public IEnumerable<LinkID> Roots { get; set; }
        public IEnumerable<LinkID> Leafs { get; set; }
    }
}