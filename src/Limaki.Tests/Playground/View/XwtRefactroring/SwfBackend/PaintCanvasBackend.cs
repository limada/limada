using Limaki.Painting;
using Xwt.Gdi;
using Xwt.WinformBackend;

namespace Limaki.SWF.Painting {

    public class PaintCanvasBackend : WidgetBackend,IPaintCanvasBackend {
   
        public void OnPreferredSizeChanged() {
            throw new System.NotImplementedException();
        }

        public void QueueDraw() {
            throw new System.NotImplementedException();
        }

        public void QueueDraw(Xwt.Rectangle rect) {
            throw new System.NotImplementedException();
        }

        public void AddChild(Xwt.Backends.IWidgetBackend widget) {
            throw new System.NotImplementedException();
        }

        public void SetChildBounds(Xwt.Backends.IWidgetBackend widget, Xwt.Rectangle bounds) {
            throw new System.NotImplementedException();
        }

        public void RemoveChild(Xwt.Backends.IWidgetBackend widget) {
            throw new System.NotImplementedException();
        }

        




        public void AddChild (Xwt.Backends.IWidgetBackend widget, Xwt.Rectangle bounds) {
            throw new System.NotImplementedException ();
        }
    }
}


