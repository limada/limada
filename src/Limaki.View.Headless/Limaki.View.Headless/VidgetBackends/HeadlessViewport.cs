using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View.Viz;

namespace Limaki.View.Headless.VidgetBackends {

    public class HeadlessViewport:Viewport {

        private DisplayBackend Backend;

        public HeadlessViewport (DisplayBackend Backend) {
            this.Backend = Backend;
        }
    }

}
