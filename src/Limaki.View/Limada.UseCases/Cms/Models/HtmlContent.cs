using Limaki.Common;
using Limaki.Contents;

namespace Limada.Usecases.Cms.Models {
    
    public class HtmlContent:Content<string> {
        public HtmlContent (Content content) : base (content) { }
        public HtmlContent () {}
    }
}