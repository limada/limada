using Limada.View.Vidgets;
using Limaki.Usecases.Vidgets;

namespace Limaki.View.XwtBackend {

    public class SplitViewToolStripBackend : ToolStripBackendDummy, ISplitViewToolStripBackend {

        public SplitViewMode ViewMode { get; set; }

        public void CheckBackForward (ISplitView splitView) {

        }

        public void AttachSheets () {

        }
    }
}