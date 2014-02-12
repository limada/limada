using Limaki.Viewers.ToolStripViewers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Limaki.View.WpfBackends {

    public abstract class ToolStripBackend : ToolBar, IToolStripViewerBackend {
        
        public abstract void InitializeBackend (IVidget frontend, VidgetApplicationContext context);

        public Xwt.Size Size { get { return this.VidgetBackendSize(); } }

        public void Update () { this.VidgetBackendUpdate(); }

        public void Invalidate () { this.VidgetBackendInvalidate(); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate(rect); }

        public void Dispose () {

        }

        protected virtual void Compose () {
           
        }
    }
}
