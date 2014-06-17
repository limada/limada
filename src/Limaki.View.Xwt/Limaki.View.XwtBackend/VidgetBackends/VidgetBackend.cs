using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public abstract class VidgetBackend<T> : IVidgetBackend, IXwtBackend where T : Widget, new () {

        public abstract void InitializeBackend (IVidget frontend, VidgetApplicationContext context);

        public VidgetBackend () {
            Compose ();
        }

        protected virtual void Compose () {
            this.Widget = new T ();
        }

        public T Widget { get; protected set; }

        public Size Size {
            get { return Widget.Size; }
            set {
                Widget.WidthRequest = value.Width;
                Widget.HeightRequest = value.Height;
            }
        }

        public void Update () { Widget.VidgetBackendUpdate (); }

        public void Invalidate () { Widget.VidgetBackendInvalidate (); }

        public void SetFocus () { Widget.SetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { Widget.VidgetBackendInvalidate (rect); }

        public virtual void Dispose () {
            Widget.Dispose ();
        }

        Widget IXwtBackend.Widget {
            get { return this.Widget; }
        }
    }
}