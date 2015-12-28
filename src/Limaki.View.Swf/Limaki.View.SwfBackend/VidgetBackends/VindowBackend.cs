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

using System.Windows.Forms;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class VindowBackend : Form, IVindowBackend {

        public void SetContent (IVidget value) {
            var backend = value.Backend.ToSwf ();
            if (!this.Controls.Contains (backend)) {
                backend.Dock = DockStyle.Fill;
                this.Controls.Add (backend);
            }
        }

        #region IVidgetBackend-Implementation

        public IVindow Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (IVindow)frontend;
        }

        IVidget IVidgetBackend.Frontend { get { return this.Frontend; } }

        Xwt.Size IVidgetBackend.Size { get { return this.Size.ToXwt (); } }

        Xwt.Size IVindowBackend.Size {
            get { return this.Size.ToXwt (); }
            set { this.Size = value.ToGdi (); }
        }

        public string ToolTipText { get; set; }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion

    }
}