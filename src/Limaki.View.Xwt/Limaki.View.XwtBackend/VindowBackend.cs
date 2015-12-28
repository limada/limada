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

        void IVidgetBackend.Invalidate () { }

        void IVidgetBackend.Invalidate (Rectangle rect) { }

        void IVidgetBackend.SetFocus () { }

        public IVindow Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend as IVindow;
        }

        IVidget IVidgetBackend.Frontend { get { return this.Frontend; } }

        void IVindowBackend.SetContent (IVidget value) {
			this.SetContent (value);
        }

        public string ToolTipText { get; set; }
    }
}
