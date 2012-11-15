using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using LinqKit;
using System.Linq;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {

    public class ToolStripButtonEx : ToolStripButton, IToolStripCommandItem {
        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand(this, ref _command, value); }
        }
        public IToolStripCommandItem ToggleOnClick { get; set; }
    }

    public class ToolStripDropDownButtonEx : ToolStripDropDownButton, IToolStripCommandItem {
        protected bool DropDownClicked = false;
        protected override void OnClick(EventArgs e) {
            if (!DropDownClicked)
                base.OnClick(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            var w = ToolStripUtils.DropdownWidth;
            var area = new Rectangle(this.Width - w, 0, w, this.Height);
            DropDownClicked = area.Contains(e.Location);
            this.DropDown.Visible = !DropDownClicked;
            
            base.OnMouseDown(e);
            //this.DropDown.Visible = this.Pressed;

        }
        
        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand(this, ref _command, value); }
        }
        public IToolStripCommandItem ToggleOnClick { get; set; }

        
    }

    public class ToolStripMenuItemEx : ToolStripMenuItem, IToolStripCommandItem {
        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand(this, ref _command, value); }
        }
        public IToolStripCommandItem ToggleOnClick { get; set; }
    }
}