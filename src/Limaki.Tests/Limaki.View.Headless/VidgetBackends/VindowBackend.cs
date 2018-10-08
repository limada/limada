using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.Headless.VidgetBackends {

    public class VindowBackend : VidgetBackend, IVindowBackend {

        public IVidgetBackend Content { get; set; }

        public virtual void SetContent (IVidget value) {
            Content = value.Backend;
        }

        public new IVindow Frontend { get; protected set; }

        public string Title { get; set; }

        public CursorType Cursor { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend as IVindow;
        }

        public void Show () {
            Content.SetFocus ();
        }
    }
}