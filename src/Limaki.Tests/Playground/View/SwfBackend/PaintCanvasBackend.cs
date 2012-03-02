using Limaki.Painting;
using Xwt.GDI;
using Xwt.WinformBackend;

namespace Limaki.SWF.Painting {

    public class PaintCanvasBackend : WidgetBackend,IPaintCanvasBackend {

        #region IPaintCanvasBackend Member

        public void QueueDraw(Xwt.RectangleD rect) {
            throw new System.NotImplementedException();
        }

        public Xwt.RectangleD Bounds {
            get { throw new System.NotImplementedException(); }
        }

        #endregion

        #region ICanvasBackend Member

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

        Xwt.Rectangle Xwt.Backends.ICanvasBackend.Bounds {
            get { throw new System.NotImplementedException(); }
        }

        #endregion
    }
}


