/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

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
