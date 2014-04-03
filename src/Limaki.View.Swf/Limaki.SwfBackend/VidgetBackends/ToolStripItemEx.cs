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

namespace Limaki.SwfBackend.VidgetBackends {

    public class ToolStripButtonEx : ToolStripButton, IToolStripCommandItem, IToolStripItem {

        public ToolStripButtonEx () {
            ImageScaling = ToolStripItemImageScaling.None;
        }

        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand(this, ref _command, value); }
        }
        public IToolStripCommandItem ToggleOnClick { get; set; }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt(); }
            set { base.Size = value.ToGdi(); }
        }

        public new Xwt.Drawing.Image Image {
            get { return base.Image.ToXwt(); }
            set { base.Image = value.ToGdi(); }
        }
    }

    public class ToolStripDropDownButtonEx : ToolStripDropDownButton, IToolStripCommandItem, IToolStripItem {
        public ToolStripDropDownButtonEx () {
            ImageScaling = ToolStripItemImageScaling.None;
        }

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

        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand(this, ref _command, value); }
        }

        public IToolStripCommandItem ToggleOnClick { get; set; }

        public new Xwt.Size Size {
            get { return base.Size.ToXwt(); }
            set { base.Size = value.ToGdi(); }
        }

        public new Xwt.Drawing.Image Image {
            get { return base.Image.ToXwt(); }
            set { base.Image = value.ToGdi(); }
        }
    }

    public class ToolStripMenuItemEx : ToolStripMenuItem, IToolStripCommandItem, IToolStripItem {
        public ToolStripMenuItemEx () {
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
        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set {
                ToolStripUtils.SetCommand(this, ref _command, value);
                this.Size = value.Size;
            }
        }
        public IToolStripCommandItem ToggleOnClick { get; set; }

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