using Xwt.Backends;

namespace Limaki.View.Vidgets {

    public interface IVindow : IVidget {
        IVidget Content { get; set; }
        void Show();
    }

    public interface IVindowBackend : IVidgetBackend {
        void SetContent (IVidget value);
        void Show ();
    }

    [BackendType (typeof (IVindowBackend))]
    public class Vindow : Vidget, IVindow {

        public Vindow () : base () { }
        public Vindow (IVindowBackend customBackend) : this() {
            BackendHost.SetCustomBackend (customBackend);
        }

        public virtual new IVindowBackend Backend { get { return BackendHost.Backend as IVindowBackend; } }

        IVidget _content = null;
        public IVidget Content {
            get { return _content; }
            set { _content = value;
                Backend.SetContent (value);
            }
        }

        public override void Dispose() { }

        public void Show () {
            Backend.Show();
        }
    }
}