namespace Limaki.Widgets {
    public class WidgetFactory : IWidgetFactory {
        
        public static string NullString = ((char)(0x2260)).ToString(); // not equal

        public IWidget CreateWidget(object data) {
            string s = NullString;
            if (data != null)
                s = data.ToString ();
            IWidget result = new Widget<string>(s);
            return result;
        }

        public IEdgeWidget CreateEdgeWidget(object data) {
            IEdgeWidget result = new EdgeWidget<string>(data.ToString());
            return result;
        }
        public IEdgeWidget CreateEdgeWidget(object data, IWidget root, IWidget leaf) {
            IEdgeWidget result = new EdgeWidget<string>(data.ToString(), root,leaf);
            return result;
        }
    }
}