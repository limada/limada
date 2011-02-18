using Limaki.Widgets;
using Limaki.Widgets.WPF;
using System;
using System.Collections.Generic;

namespace Limaki.Widgets.WPF {
    public class WPFWidgetFactory : IWidgetFactory {

        public static string NullString = ((char)(0x2260)).ToString(); // not equal

        public IWidget CreateItem(object data) {
            string s = NullString;
            if (data != null)
                s = data.ToString();
            IWidget result = new WPFWidget<string>(s);
            return result;
        }

        public IEdgeWidget CreateEdge(object data) {
            IEdgeWidget result = new WPFEdgeWidget<string>(data.ToString());
            return result;
        }

        public IEdgeWidget CreateEdge(IWidget root, IWidget leaf,object data) {
            IEdgeWidget result = new WPFEdgeWidget<string>(data.ToString(), root, leaf);
            return result;
        }

        public IWidget CreateItem<T>(T data) {
            return new WPFWidget<T> (data);
        }

        public IEdgeWidget CreateEdge<T>(T data) {
            return new WPFEdgeWidget<T> (data);
        }




        public IEnumerable<Type> KnownClasses {
            get { yield return typeof (WPFWidget<string>);
                  yield return typeof(WPFEdgeWidget<string>);
            }
        }


    }
}