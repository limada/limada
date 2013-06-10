using Limaki.View;
using System.Diagnostics;
using Xwt;

namespace Limaki.Viewers.ToolStripViewers {

   
    public abstract class ToolStripViewer0<TDisplay, TBackend>:IVidget
        where TDisplay : class
        where TBackend : IToolStripViewerBackend {

        public virtual TBackend Backend { get; set; }
        public virtual TDisplay CurrentDisplay { get; set; }

        public virtual void Attach (object sender) {
            var display = sender as TDisplay;
            if (display != null)
                CurrentDisplay = display;
            else
                Trace.WriteLine(this.GetType().Name + ": display not set");
        }

        public abstract void Detach (object sender);


        public void Dispose () {
            CurrentDisplay = null;
        }

        public Size DefaultSize = new Size(36, 36);
    }
}