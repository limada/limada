using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.XwtBackend {

    public class ToolStripBackendDummy : DummyBackend, IToolStripBackend {
        public void InsertItem (int index, IToolStripItemBackend toolStripItemBackend) {
           
        }

        public void RemoveItem (IToolStripItemBackend toolStripItemBackend) {
          
        }
    }

    public class ArrangerToolStripBackend : ToolStripBackendDummy, IArrangerToolStripBackend {
    }
}