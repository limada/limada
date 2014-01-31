using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class DummyBackend : Frame, IVidgetBackend {

        void IVidgetBackend.Update () { XwtBackendHelper.VidgetBackendUpdate(this); }

        void IVidgetBackend.Invalidate () { XwtBackendHelper.VidgetBackendInvalidate(this); }

        void IVidgetBackend.Invalidate (Rectangle rect) { XwtBackendHelper.VidgetBackendInvalidate(this, rect); }

        private IVidget frontend;
        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.frontend = frontend;
        }
    }
}
