using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class VindowBackend : Window, IVindowBackend {

        public virtual void SetContent (IVidget value) {
            Content = value.Backend as Widget;
        }

        void IVidgetBackend.Update () { }

        void IVidgetBackend.Invalidate () { }

        void IVidgetBackend.Invalidate (Rectangle rect) { }

        public IVindow Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend as IVindow;
        }

        void IVindowBackend.SetContent (IVidget value) {
            throw new NotImplementedException ();
        }
    }
}
