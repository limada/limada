using Xwt.Engine;
using Xwt.Backends;
using System;
using SWF=System.Windows.Forms;

namespace Xwt.WinformBackend {

    public class WindowFrameBackend : IWindowFrameBackend {

        SWF.Form form;
        IWindowFrameEventSink eventSink;
        WindowFrame frontend;

        public WindowFrameBackend() {
        }

        void IBackend.Initialize(object frontend) {
            this.frontend = (WindowFrame)frontend;
        }

        void IWindowFrameBackend.Initialize(IWindowFrameEventSink eventSink) {
            this.eventSink = eventSink;
            Initialize();
        }

        public virtual void Initialize() {
        }

        public virtual void Dispose(bool disposing) {
            Form.Close();
        }

        public SWF.Form Form {
            get { return form; }
            set { form = value; }
        }

        protected WindowFrame Frontend {
            get { return frontend; }
        }

        public IWindowFrameEventSink EventSink {
            get { return eventSink; }
        }

        bool IWindowFrameBackend.Decorated {
            get { return form.FormBorderStyle != SWF.FormBorderStyle.Sizable; }
            set { form.FormBorderStyle = value ? SWF.FormBorderStyle.Sizable : SWF.FormBorderStyle.None; }
        }

        bool IWindowFrameBackend.ShowInTaskbar {
            get { return form.ShowInTaskbar; }
            set { form.ShowInTaskbar = value; }
        }

        string IWindowFrameBackend.Title {
            get { return form.Text; }
            set { form.Text = value; }
        }

        bool IWindowFrameBackend.Visible {
            get { return form.Visible; }
            set { form.Visible = value; }
        }

        public Rectangle Bounds {
            get {
                return new Rectangle(form.Left, form.Top, form.Width, form.Height);
            }
            set {
                form.Top = (int)value.Top;
                form.Left = (int)value.Left;
                form.Width = (int)value.Width;
                form.Height = (int)value.Height;
                Toolkit.Invoke(delegate {
                                   eventSink.OnBoundsChanged(Bounds);
                               });
            }
        }

        public virtual void EnableEvent(object eventId) {
            if (eventId is WindowFrameEvent) {
                switch ((WindowFrameEvent)eventId) {
                    case WindowFrameEvent.BoundsChanged:
                        form.LocationChanged += BoundsChangedHandler;
                        form.SizeChanged += BoundsChangedHandler;
                        break;
                }
            }
        }

        public virtual void DisableEvent(object eventId) {
            if (eventId is WindowFrameEvent) {
                switch ((WindowFrameEvent)eventId) {
                    case WindowFrameEvent.BoundsChanged:
                        form.LocationChanged -= BoundsChangedHandler;
                        form.SizeChanged -= BoundsChangedHandler;
                        break;
                }
            }
        }

        void BoundsChangedHandler(object o, EventArgs args) {
            Toolkit.Invoke(delegate() {
                eventSink.OnBoundsChanged(Bounds);
            });
        }
    }
}