using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Drawing;
using Limaki.Presenter.UI;
using LinqKit;
using System.Linq;
using Limaki.Common;

namespace Limaki.UseCases.Winform.Viewers.ToolStripViewers {
    public class ToolStripUtils {
        static IUISystemInformation _systemInformation = null;
        public static IUISystemInformation SystemInformation {
            get { return _systemInformation ?? (_systemInformation = Registry.Pool.TryGetCreate<IUISystemInformation>()); }
        }

        public static int DropdownWidth = SystemInformation.VerticalScrollBarWidth / 3*2;

        public static void SetCommand(IToolStripCommandItem item, ref ToolStripCommand _command, ToolStripCommand value) {
            var toolStripItem = item as ToolStripItem;
            if (_command != value) {
                try {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.SuspendLayout();
                    if (_command != null)
                        _command.DeAttach(item);
                    _command = value;
                    _command.Attach(item);
                } finally {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.ResumeLayout(true);
                }
            }
        }
    }

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