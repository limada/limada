using System.Collections.Generic;
using System;
namespace Limaki.Widgets {
    public class WidgetFactory : IWidgetFactory {
        
        public static string NullString = ((char)(0x2260)).ToString(); // not equal

        public IWidget CreateItem<T>(T data) {
            return new Widget<T> (data);
        }

        public IEdgeWidget CreateEdge<T>(T data) {
            return new EdgeWidget<T>(data);
        }

        public IWidget CreateItem(object data) {
            string s = NullString;
            if (data != null)
                s = data.ToString ();
            IWidget result = new Widget<string>(s);
            return result;
        }

        public IEdgeWidget CreateEdge(object data) {
            IEdgeWidget result = new EdgeWidget<string>(data.ToString());
            return result;
        }

        public IEdgeWidget CreateEdge(IWidget root, IWidget leaf,object data) {
            IEdgeWidget result = new EdgeWidget<string>(data.ToString(), root,leaf);
            return result;
        }


        public IEnumerable<Type> KnownClasses {
            get { yield return typeof (Widget<string>);
                  yield return typeof (EdgeWidget<string>);
            }
        }

    }
}