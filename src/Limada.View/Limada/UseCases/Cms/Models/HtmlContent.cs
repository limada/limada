using Limaki.Contents;

namespace Limada.Usecases.Cms.Models {
    
    public class HtmlContent:Content<string> {
        public HtmlContent (Limaki.Contents.Content content) : base (content) { }
        public HtmlContent () {}
    }
}