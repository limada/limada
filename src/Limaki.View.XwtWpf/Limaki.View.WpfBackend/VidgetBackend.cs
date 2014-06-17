using System.Windows.Controls;
using Limaki.View.Vidgets;
using System.Windows;
using System;

namespace Limaki.View.WpfBackend {

    public interface IWpfBackend {
        FrameworkElement Control { get; }
    }

    public abstract class VidgetBackend<T> : IVidgetBackend, IWpfBackend where T : FrameworkElement {

        public abstract void InitializeBackend (IVidget frontend, VidgetApplicationContext context);

        public VidgetBackend () {
            Compose ();
        }

        public virtual void Compose () {
            this.Control = Activator.CreateInstance<T>();
        }

        public virtual T Control { get; protected set; }

        public Xwt.Size Size {
            get { return Control.VidgetBackendSize(); }
            set { Control.VidgetBackendSize (value); }
        }

        public virtual void Invalidate (Xwt.Rectangle rect) {
            Control.VidgetBackendInvalidate(rect);
        }

        public virtual void SetFocus () { Control.Focus (); }

        public virtual void Update () {
            Control.VidgetBackendUpdate ();
        }

        public virtual void Invalidate () {
            Control.VidgetBackendInvalidate ();
        }

        public virtual void Dispose () {
            Control = null;
        }

        FrameworkElement IWpfBackend.Control {
            get { return this.Control; }
        }
    }
}