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

using Limaki.View;
using Xwt;

namespace Limaki.View.Headless.VidgetBackends {
    public class DummyBackend : IVidgetBackend {

        public void Update () {
            this.Invalidate();
        }

        public void Invalidate () {
            if (frontend is IDisplay) {
                ((IDisplay)frontend).BackendRenderer.Render ();

            }
        }

        public void Invalidate (Rectangle rect) {
            if(frontend is IDisplay) {
                ((IDisplay) frontend).BackendRenderer.Render();

            }
        }

        private IVidget frontend;
        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.frontend = frontend;
        }

        public virtual Size Size { get; set; }

        public void Dispose () {
           
        }
    }
}
