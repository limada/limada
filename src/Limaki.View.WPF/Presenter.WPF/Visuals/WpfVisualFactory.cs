using Limaki.Visuals;
using Limaki.Visuals.WPF;
using System;
using System.Collections.Generic;

namespace Limaki.Visuals.WPF {
    public class WpfVisualFactory : IVisualFactory {

        public static string NullString = ((char)(0x2260)).ToString(); // not equal

        public IVisual CreateItem(object data) {
            string s = NullString;
            if (data != null)
                s = data.ToString();
            IVisual result = new WpfVisual<string>(s);
            return result;
        }

        public IVisualEdge CreateEdge(object data) {
            IVisualEdge result = new WpfVisualEdge<string>(data.ToString());
            return result;
        }

        public IVisualEdge CreateEdge(IVisual root, IVisual leaf,object data) {
            IVisualEdge result = new WpfVisualEdge<string>(data.ToString(), root, leaf);
            return result;
        }

        public IVisual CreateItem<T>(T data) {
            return new WpfVisual<T> (data);
        }

        public IVisualEdge CreateEdge<T>(T data) {
            return new WpfVisualEdge<T> (data);
        }




        public IEnumerable<Type> KnownClasses {
            get { yield return typeof (WpfVisual<string>);
                  yield return typeof(WpfVisualEdge<string>);
            }
        }


    }
}