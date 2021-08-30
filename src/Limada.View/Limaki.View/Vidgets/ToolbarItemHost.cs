using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolbarItemHostBackend))]
    public class ToolbarItemHost : ToolbarItem {

        private IToolbarItemHostBackend _backend = null;
        public new virtual IToolbarItemHostBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolbarItemHostBackend); }
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

    public interface IToolbarItemHostBackend : IToolbarItemBackend {
        void SetChild (Vidget value);
    }
}