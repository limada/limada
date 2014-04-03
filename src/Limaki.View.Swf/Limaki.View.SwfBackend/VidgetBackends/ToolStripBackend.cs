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
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Xwt.GdiBackend;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public abstract partial class ToolStripBackend : ToolStrip, IDisplayToolStripBackend {

        #region IVidgetBackend Member

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

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion



    }
}