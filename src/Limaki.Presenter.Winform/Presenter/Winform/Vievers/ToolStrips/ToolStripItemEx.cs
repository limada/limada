using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using LinqKit;
using System.Linq;

namespace Limaki.UseCases.Winform.Viewers.ToolStripViewers {
    public class ToolStripUitils {
        
    }

    public class ToolStripButtonEx : ToolStripButton, IToolStripCommandItem {
        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set {
                if (_command != value) {
                    if (_command != null)
                        _command.DeAttach(this);
                    _command = value;
                    _command.Attach(this);
                }
            }
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
            var area = new Rectangle(this.Width - 10, 0, 10, this.Height);
            DropDownClicked = area.Contains(e.Location);
            this.DropDown.Visible = !DropDownClicked;
            base.OnMouseDown(e);

        }
        
        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set {
                if (_command != value) {
                    if (_command != null)
                        _command.DeAttach(this);
                    _command = value;
                    _command.Attach(this);
                }
            }
        }
        public IToolStripCommandItem ToggleOnClick { get; set; }

        
    }

    public class ToolStripMenuItemEx : ToolStripMenuItem, IToolStripCommandItem {
        public ToolStripCommand _command = null;
        public ToolStripCommand Command {
            get { return _command; }
            set {
                if (_command != value) {
                    if (_command != null)
                        _command.DeAttach(this);
                    _command = value;
                    _command.Attach(this);
                }
            }
        }
        public IToolStripCommandItem ToggleOnClick { get; set; }
    }
}