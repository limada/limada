using System.Windows.Forms;
using Limaki.View;
using Xwt.Gdi.Backend;
using Limaki.Viewers.ToolStripViewers;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public partial class ToolStripBackend : ToolStrip, IToolStripViewerBackend {
        #region IVidgetBackend Member

        Xwt.Rectangle IVidgetBackend.ClientRectangle {
            get { return this.ClientRectangle.ToXwt(); }
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.Update () {
            this.Update();
        }

        void IVidgetBackend.Invalidate () {
            this.Invalidate();
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IVidgetBackend.PointToClient (Xwt.Point source) {
            return this.PointToClient(source.ToGdi()).ToXwt();
        }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            throw new System.NotImplementedException();
        }

        #endregion


      
    }
}