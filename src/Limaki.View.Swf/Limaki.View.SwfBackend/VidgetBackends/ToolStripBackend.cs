/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.ComponentModel;
using System.Windows.Forms;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Xwt.GdiBackend;
using SWF = System.Windows.Forms;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public abstract partial class ToolStripBackend : SWF.ToolStrip, IToolStripBackend {

        public ToolStripBackend () {
            Compose ();
        }

        protected virtual void Compose () {
            this.Dock = System.Windows.Forms.DockStyle.None;
            this.ImageScalingSize = new System.Drawing.Size (20, 20);
            this.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.Size = new System.Drawing.Size (100, 36);
        }

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

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public LVV.ToolStrip Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStrip) frontend;
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion
        
        public void InsertItem (int index, IToolStripItemBackend item) {
            base.Items.Insert (index, (SWF.ToolStripItem) item);
        }

        public void RemoveItem (IToolStripItemBackend item) {
            base.Items.Remove ((SWF.ToolStripItem) item);
        }
    }
}