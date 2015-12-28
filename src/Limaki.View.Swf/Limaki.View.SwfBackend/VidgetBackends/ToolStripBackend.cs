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

    public abstract partial class ToolStripBackend : VidgetBackend<SWF.ToolStrip>, IToolStripBackend {

        protected override void Compose () {

            base.Compose ();

            Control.Dock = System.Windows.Forms.DockStyle.None;
            Control.ImageScalingSize = new System.Drawing.Size (20, 20);
            Control.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            Control.Size = new System.Drawing.Size (100, 36);
            Control.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
        }


        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public new LVV.ToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (LVV.ToolStrip)frontend;
        }

        public void InsertItem (int index, IToolStripItemBackend item) {
            var control = item.ToSwf() as SWF.ToolStripItem;
            control.Font = Control.Font;
            Control.Items.Insert (index, control);
        }

        public void RemoveItem (IToolStripItemBackend item) {
            var control = item.ToSwf() as SWF.ToolStripItem;
            Control.Items.Remove (control);
        }

        public void SetVisibility (Visibility value) {
            if (value == Visibility.Visible && !Control.Visible)
                Control.Visible = true;
            else
                Control.Visible = false;
        }
    }
}