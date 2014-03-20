using System.Linq;
using System.Text;
using System.Windows.Controls;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.WpfBackends {

    public abstract class ToolStripBackend : ToolBar, IDisplayToolStripBackend {
        
        public abstract void InitializeBackend (IVidget frontend, VidgetApplicationContext context);

        public Xwt.Size Size { get { return this.VidgetBackendSize(); } }

        public void Update () { this.VidgetBackendUpdate(); }

        public void Invalidate () { this.VidgetBackendInvalidate(); }

        public void SetFocus() { this.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate(rect); }

        public void Dispose () {

        }

        protected virtual void Compose () {
           
        }
    }
}
