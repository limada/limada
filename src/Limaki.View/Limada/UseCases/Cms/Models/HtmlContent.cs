using Limaki.Contents;

namespace Limada.UseCases.Cms.Models {
    
    public class HtmlContent:Content<string> {
        public HtmlContent (Limaki.Contents.Content content) : base (content) { }
        public HtmlContent () {}
    }
}