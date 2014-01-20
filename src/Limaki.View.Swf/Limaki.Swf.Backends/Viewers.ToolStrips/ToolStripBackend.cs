/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Windows.Forms;
using Limaki.View;
using Xwt.Gdi.Backend;
using Limaki.Viewers.ToolStripViewers;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public abstract partial class ToolStripBackend : ToolStrip, IToolStripViewerBackend {
        #region IVidgetBackend Member

        Xwt.Rectangle IVidgetBackend.ClientRectangle {
            get { return this.ClientRectangle.ToXwt(); }
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.Update () {
            this.Update();
        }

        void IVidgetBackend.Invalidate () {
            this.Invalidate();
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        public abstract void InitializeBackend(IVidget frontend, VidgetApplicationContext context);


        #endregion



    }
}