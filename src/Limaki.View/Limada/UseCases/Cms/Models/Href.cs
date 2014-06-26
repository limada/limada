namespace Limada.UseCases.Cms.Models {

    public class Href {

        public Href () { }

        public Href (string text, string id) {
            this.Text = text;
            this.Id = id;
        }

        public string Text { get; set; }
        public string Id { get; set; }
    }
}