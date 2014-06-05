using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolStripItemBackend))]
    public class ToolStripItem : Vidget {

        private IToolStripItemBackend _backend = null;
        public new virtual IToolStripItemBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripItemBackend); }
            set { _backend = value; }
        }

        public override void Dispose () { }
    }

    public interface IToolStripItemBackend : IVidgetBackend { }

    [BackendType (typeof (IToolStripButtonBackend))]
    public class ToolStripButton : ToolStripItem {
        public override void Dispose () { }
    }

    public interface IToolStripButtonBackend : IToolStripItemBackend { }

    [BackendType (typeof (IToolStripDropDownButtonBackend))]
    public class ToolStripDropDownButton : ToolStripButton {
        public override void Dispose () { }
    }

    public interface IToolStripDropDownButtonBackend : IToolStripButtonBackend { }
}