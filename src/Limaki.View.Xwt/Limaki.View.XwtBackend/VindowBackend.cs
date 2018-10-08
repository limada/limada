using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class VindowBackend : Window, IVindowBackend {

        public virtual void SetContent (IVidget value) {
            Content = value.Backend.ToXwt ();
        }

        void IVidgetBackend.Update () { }

        void IVidgetBackend.QueueDraw () { }

        void IVidgetBackend.QueueDraw (Rectangle rect) { }

        void IVidgetBackend.SetFocus () { }

        public IVindow Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend as IVindow;
        }

        IVidget IVidgetBackend.Frontend { get { return this.Frontend; } }

        void IVindowBackend.SetContent (IVidget value) {
            this.SetContent (value);
        }

        IVidgetEventSink EventSink { get; set; }
        public void InitializeEvents (IVidgetEventSink eventSink) {
            EventSink = eventSink;
        }

        public string ToolTipText { get; set; }
        // TODO:
        CursorType IVindowBackend.Cursor {
            get => Content?.Cursor ?? CursorType.Arrow;
            set {
                if (Content != null)
                    Content.Cursor = value;
            }
        }
    }
}
