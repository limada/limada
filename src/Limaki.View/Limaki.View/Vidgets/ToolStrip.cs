using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolStripBackend))]
    public class ToolStrip : Vidget, IToolStripItemContainer {

        private ToolStripItemCollection _items;
        public ToolStripItemCollection Items {
            get { return _items ?? (_items = new ToolStripItemCollection (this)); }
        }

        private IToolStripBackend _backend = null;
        public new virtual IToolStripBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripBackend); } 
            set { _backend = value; }
        }

        public void AddItems (params ToolStripItem[] items) {
            foreach (var item in items)
                Items.Add (item);

        }

        public override void Dispose () { }

        void IToolStripItemContainer.InsertItem (int index, ToolStripItem item) {
            Backend.InsertItem (index, (IToolStripItemBackend) item.Backend);
        }

        void IToolStripItemContainer.RemoveItem (ToolStripItem item) {
            Backend.RemoveItem ((IToolStripItemBackend) item.Backend);
        }
    }

    public interface IToolStripBackend : IVidgetBackend {
        void InsertItem (int index, IToolStripItemBackend toolStripItemBackend);
        void RemoveItem (IToolStripItemBackend toolStripItemBackend);
    }
}