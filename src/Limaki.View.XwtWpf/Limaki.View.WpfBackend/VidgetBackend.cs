using System.Windows.Controls;
using Limaki.View.Vidgets;
using System.Windows;
using System;
using Limaki.View.XwtBackend;

namespace Limaki.View.WpfBackend {

    public interface IWpfBackend {
        FrameworkElement Control { get; }
    }

    public abstract class VidgetBackend<T> : IVidgetBackend, IWpfBackend where T : FrameworkElement {

        public IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }
        
        protected IVidgetEventSink EventSink { get; set; }

        public virtual void InitializeEvents (IVidgetEventSink eventsink) {
            this.EventSink = eventsink;
            Control.GotFocus += (s, e) => EventSink?.OnEvent (nameof (IVidget.GotFocus), new EventArgs ());
            Control.MouseUp += (s, e) => EventSink?.OnEvent (nameof (IVidget.ButtonReleased), WpfConverter.ToXwtButtonArgs ((Control) s, e).ToLmk ());
        }

        public VidgetBackend () {
            Compose ();
        }

        public virtual void Compose () {
            Control = Activator.CreateInstance<T>();

        }

        public virtual T Control { get; protected set; }

        public Xwt.Size Size {
            get { return Control.VidgetBackendSize(); }
            set { Control.VidgetBackendSize (value); }
        }

        public string ToolTipText {
            get { return Control.ToolTip as string; }
            set { Control.ToolTip = value; }
        }

        public virtual void QueueDraw (Xwt.Rectangle rect) {
            Control.VidgetBackendInvalidate(rect);
        }

        public virtual void SetFocus () { Control.Focus (); }

        public virtual void Update () {
            Control.VidgetBackendUpdate ();
        }

        public virtual void QueueDraw () {
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