﻿using Limaki.View.Visualizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {

    public class HeadlessViewport:Viewport {

        private DisplayBackend Backend;

        public HeadlessViewport (DisplayBackend Backend) {
            this.Backend = Backend;
        }
    }

}
