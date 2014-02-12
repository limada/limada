using System.Collections.Generic;
using Limaki.Contents;
using Limaki.Model.Content;

namespace Limada.Usecases.Cms.Models {

    public class ContentAndLinks {
        public Content Content { get; set; }
        public IEnumerable<LinkID> Roots { get; set; }
        public IEnumerable<LinkID> Leafs { get; set; }
    }
}