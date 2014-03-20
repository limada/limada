namespace Limada.UseCases.Cms.Models {

    public class LinkID {

        public LinkID (string text, string id) {
            this.Text = text;
            this.ID = id;
        }

        public string Text { get; set; }
        public string ID { get; set; }
    }
}