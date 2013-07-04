using System;
using System.Windows.Forms;
using Limaki.Common;
using Xwt.Gdi.Backend;

namespace Limaki.View.Ui.DragDrop1 {

    public class ContextEventArgs : EventArgs {
        public ContextEventArgs (Xwt.Drawing.Context context) { this.Context = context; }
        public Xwt.Drawing.Context Context { get; protected set; }
    }

    public interface ICanvasVidget:IVidget {
        void PaintContext (object sender, ContextEventArgs e);
    }

    public class CanvasVidgetBackend : UserControl, IVidgetBackend {

        protected override void OnPaint (PaintEventArgs e) {

            base.OnPaint(e);

            using (var graphics = new GdiContext(e.Graphics)) {

                OnPaintContext(new ContextEventArgs(new Xwt.Drawing.Context(GdiEngine.Registry, graphics)));
            }
        }

        protected virtual void OnPaintContext (ContextEventArgs e) {
            Frontend.PaintContext(this, e);
        }

        #region IVidgetBackend-Implementation

        public ICanvasVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (ICanvasVidget) frontend;
        }

        Xwt.Rectangle IVidgetBackend.ClientRectangle {
            get { return this.ClientRectangle.ToXwt(); }
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }


        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IVidgetBackend.PointToClient (Xwt.Point source) { return PointToClient(source.ToGdi()).ToXwt(); }

        #endregion
    }
}