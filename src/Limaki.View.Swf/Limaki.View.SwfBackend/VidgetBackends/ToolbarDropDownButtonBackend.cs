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
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Xwt.GdiBackend;
using LVV = Limaki.View.Vidgets;
using SWF = System.Windows.Forms;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ToolbarDropDownButtonBackend : ToolStripItemBackend<ToolbarDropDownButtonBackend.ToolStripDropDownButton>, IToolbarDropDownButtonBackend {

        public class ToolStripDropDownButton : SWF.ToolStripDropDownButton {
            protected bool DropDownClicked = false;
            protected override void OnClick (EventArgs e) {

                if (!DropDownClicked)
                    base.OnClick (e);
            }

            protected override void OnMouseDown (MouseEventArgs e) {

                var w = ToolStripUtils.DropdownWidth;
                var area = new System.Drawing.Rectangle (Width - w, 0, w, Height);
                DropDownClicked = area.Contains (e.Location);
                DropDown.PerformLayout ();
                var size = DropDown.Size;
                DropDown.Visible = !DropDownClicked;
                if (Image != null && string.IsNullOrEmpty (Text) && DisplayStyle == ToolStripItemDisplayStyle.Image) {
                    DropDown.AutoSize = false;
                    DropDown.Width = (int)Size.Width;
                    DropDown.Height = size.Height;
                }
                base.OnMouseDown (e);

                //this.DropDown.Visible = this.Pressed;

            }

            public virtual void SetImage (Xwt.Drawing.Image image) {
                if (this.Parent != null)
                    this.Parent.SuspendLayout ();
                this.Image = image.ToGdi ();
                if (this.Parent != null)
                    this.Parent.ResumeLayout ();
            }

            internal bool? IsChecked () { return false; }

            internal void IsChecked (bool? value) { }

            public bool IsCheckable { get { return false; } set { } }
        }

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public new LVV.ToolbarDropDownButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (LVV.ToolbarDropDownButton)frontend;
        }

        protected override void Compose () {
            base.Compose ();
            Control.ImageScaling = ToolStripItemImageScaling.None;
            Control.DisplayStyle = ToolStripItemDisplayStyle.Image;
            Control.Click += ClickAction;
            ((ToolStripDropDownMenu)Control.DropDown).ShowImageMargin = false;
            ((ToolStripDropDownMenu)Control.DropDown).ShowCheckMargin = false;
        }

        public void InsertItem (int index, IToolbarItemBackend backend) {
            Control.DropDownItems.Insert (index, backend.ToSwf());
        }

        public void RemoveItem (IToolbarItemBackend backend) {
            Control.DropDownItems.Remove (backend.ToSwf ());
        }

        public override void SetImage (Xwt.Drawing.Image image) {
            Control.SetImage (image);
        }

        public bool? IsChecked {
            get { return Control.IsChecked (); }
            set { Control.IsChecked (value); }
        }

        public bool IsCheckable {
            get { return Control.IsCheckable; }
            set { Control.IsCheckable = value; }
        }
    }
}