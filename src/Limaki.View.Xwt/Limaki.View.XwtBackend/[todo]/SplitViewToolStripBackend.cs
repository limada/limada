using Limaki.Viewers.ToolStripViewers;

namespace Limaki.View.XwtBackend {
    public class SplitViewToolStripBackend : DummyBackend, ISplitViewToolStripBackend {
        public Viewers.SplitViewMode ViewMode { get; set; }

        public void CheckBackForward (Viewers.ISplitView splitView) {

        }

        public void AttachSheets () {

        }
    }
}