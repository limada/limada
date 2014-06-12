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


using System;
using System.Windows.Forms;
using System.Drawing;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using SWF = System.Windows.Forms;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ToolStripButtonBackend0 : SWF.ToolStripButton, IToolStripCommandToggle0, IToolStripItem0 {

        public ToolStripButtonBackend0 () {
            ImageScaling = ToolStripItemImageScaling.None;
        }

        public ToolStripCommand0 _command = null;
        public ToolStripCommand0 Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand(this, ref _command, value); }
        }

        public IToolStripCommandToggle0 ToggleOnClick { get; set; }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt(); }
            set { base.Size = value.ToGdi(); }
        }

        public new Xwt.Drawing.Image Image {
            get { return base.Image.ToXwt(); }
            set { base.Image = value.ToGdi(); }
        }


        public string Label { get { return base.Text; } set { base.Text = value; } }
    }

    public class ToolStripDropDownButtonBackend0 : SWF.ToolStripDropDownButton, IToolStripCommandToggle0, IToolStripItem0 {
        
        public ToolStripDropDownButtonBackend0 () {
            ImageScaling = ToolStripItemImageScaling.None;
        }

        public string Label { get { return base.Text; } set { base.Text = value; } }

        protected bool DropDownClicked = false;
        protected override void OnClick (EventArgs e) {

            if (!DropDownClicked)
                base.OnClick(e);
        }

        protected override void OnMouseDown (MouseEventArgs e) {

            var w = ToolStripUtils.DropdownWidth;
            var area = new Rectangle(this.Width - w, 0, w, this.Height);
            DropDownClicked = area.Contains(e.Location);
            this.DropDown.Visible = !DropDownClicked;
            if (Image != null && Text == null && DisplayStyle == ToolStripItemDisplayStyle.Image) {
                var size = DropDown.Size;
                DropDown.AutoSize = false;
                DropDown.Width = (int) Size.Width;
                DropDown.Height = size.Height;
            }
            base.OnMouseDown(e);
            //this.DropDown.Visible = this.Pressed;

        }

        public ToolStripCommand0 _command = null;
        public ToolStripCommand0 Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand(this, ref _command, value); }
        }

        public IToolStripCommandToggle0 ToggleOnClick { get; set; }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt(); }
            set { base.Size = value.ToGdi(); }
        }

        public new Xwt.Drawing.Image Image {
            get { return base.Image.ToXwt(); }
            set { base.Image = value.ToGdi(); }
        }
    }

    public class ToolStripMenuItemBackend0 : ToolStripMenuItem, IToolStripCommandToggle0, IToolStripItem0 {

        public ToolStripMenuItemBackend0 () {
            ImageScaling = ToolStripItemImageScaling.None;
            //    DropDown.AutoSize = false;
            //}
            //public override Size Size {
            //    get {
            //        return base.Size;
            //    }
            //    set {
            //        DropDown.AutoSize = false;
            //        DropDown.Width = Size.Width;
            //        base.Size = value;
            //    }
        }

        public string Label { get { return base.Text; } set { base.Text = value; } }

        public ToolStripCommand0 _command = null;
        public ToolStripCommand0 Command {
            get { return _command; }
            set {
                ToolStripUtils.SetCommand(this, ref _command, value);
                this.Size = value.Size;
            }
        }

        public IToolStripCommandToggle0 ToggleOnClick { get; set; }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt(); }
            set { base.Size = value.ToGdi(); }
        }

        public new Xwt.Drawing.Image Image {
            get { return base.Image.ToXwt(); }
            set { base.Image = value.ToGdi(); }
        }
    }
}