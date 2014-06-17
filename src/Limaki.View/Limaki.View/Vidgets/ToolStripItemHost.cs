using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolStripItemHostBackend))]
    public class ToolStripItemHost : ToolStripItem {

        private IToolStripItemHostBackend _backend = null;
        public new virtual IToolStripItemHostBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripItemHostBackend); }
            set { _backend = value; }
        }

        private Vidget _child;
        public Vidget Child {
            get { return _child; }
            set {
                _child = value;
                Backend.SetChild (value);
            }
        }
    }

    public interface IToolStripItemHostBackend : IToolStripItemBackend {
        void SetChild (Vidget value);
    }
}