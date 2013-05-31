using System.Diagnostics;

namespace Limaki.Viewers.ToolStripViewers {
    public interface IToolStripViewerBackend {}

    public abstract class ToolStripViewer<TDisplay, TBackend>
        where TDisplay : class
        where TBackend : IToolStripViewerBackend {

        public virtual TBackend Backend { get; set; }
        public virtual TDisplay CurrentDisplay { get; set; }

        public virtual void Attach (object sender) {
            var display = sender as TDisplay;
            if (display != null)
                CurrentDisplay = display;
            else
                Trace.WriteLine("ToolStripViewer: display not set");
        }

        public abstract void Detach (object sender);

    }
}