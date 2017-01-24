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

using Limaki.View.Vidgets;
using System.ComponentModel;
using System.Windows.Forms;
using Xwt.GdiBackend;
using LVV = Limaki.View.Vidgets;
using SWF = System.Windows.Forms;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ToolbarButtonBackend : ToolStripItemBackend<ToolbarButtonBackend.ToolStripButton>, IToolbarButtonBackend {

        public class ToolStripButton : SWF.ToolStripButton {
            public virtual void SetImage (Xwt.Drawing.Image image) {
                if (this.Parent != null)
                    this.Parent.SuspendLayout ();
                this.Image = image.ToGdi ();
                if (this.Parent != null)
                    this.Parent.ResumeLayout ();
            }

        }

        protected override void Compose () {
            base.Compose ();
            Control.ImageScaling = ToolStripItemImageScaling.None;
            Control.Click += ClickAction;
        }

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public new LVV.ToolbarButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (LVV.ToolbarButton)frontend;
        }

        public override void SetImage (Xwt.Drawing.Image image) {
            Control.SetImage (image);
        }

        public bool? IsChecked {
            get { return Control.IsChecked (); }
            set { Control.IsChecked (value); }
        }

        public bool IsCheckable {
            get { return Control.CheckOnClick; }
            set { Control.CheckOnClick = value; }
        }
    }
}