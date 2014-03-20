using System.Collections.Generic;
using System;

namespace Limaki.View.Visuals {

    public class VisualFactory : IVisualFactory {
        
        public static string NullString = ((char)(0x2260)).ToString(); // not equal

        public IVisual CreateItem<T>(T data) {
            return new Visual<T> (data);
        }

        public IVisualEdge CreateEdge<T>(T data) {
            return new VisualEdge<T>(data);
        }

        public IVisual CreateItem(object data) {
            string s = NullString;
            if (data != null)
                s = data.ToString ();
            IVisual result = new Visual<string>(s);
            return result;
        }

        public IVisualEdge CreateEdge(object data) {
            IVisualEdge result = new VisualEdge<string>(data.ToString());
            return result;
        }

        public IVisualEdge CreateEdge(IVisual root, IVisual leaf,object data) {
            IVisualEdge result = new VisualEdge<string>(data.ToString(), root,leaf);
            return result;
        }


        public IEnumerable<Type> KnownClasses {
            get { yield return typeof (Visual<string>);
                  yield return typeof (VisualEdge<string>);
            }
        }



        public Graphs.IGraph<IVisual, IVisualEdge> Graph () {
            throw new NotImplementedException ();
        }

        public IVisualEdge CreateEdge (IVisual root, IVisual leaf) {
            throw new NotImplementedException ();
        }

        public IVisualEdge CreateEdge () {
            throw new NotImplementedException ();
        }

        public void SetData (IVisual item, object data) {
            throw new NotImplementedException ();
        }

        public object GetData (IVisual item) {
            throw new NotImplementedException ();
        }
    }
}