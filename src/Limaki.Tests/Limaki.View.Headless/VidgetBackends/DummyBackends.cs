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
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using Xwt;

namespace Limaki.View.Headless.VidgetBackends {
    public class DummyBackend : IVidgetBackend {

        public void Update () {
            this.Invalidate();
        }

        public void Invalidate () {
            if (Frontend is IDisplay) {
                ((IDisplay)Frontend).BackendRenderer.Render ();

            }
        }

        public void SetFocus () { }

        public void Invalidate (Rectangle rect) {
            if(Frontend is IDisplay) {
                ((IDisplay) Frontend).BackendRenderer.Render();

            }
        }

        public IVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend;
        }

        public virtual Size Size { get; set; }

        public void Dispose () {
           
        }
    }
}
