using System;
using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using ToolStripButton = System.Windows.Forms.ToolStripButton;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public static class ToolStripUtils0 {
        public static void SetCommand (IToolStripCommandToggle0 item, ref ToolStripCommand0 _command, ToolStripCommand0 value) {
            var toolStripItem = item as System.Windows.Forms.ToolStripItem;
            if (_command != value) {
                try {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.SuspendLayout ();
                    if (_command != null)
                        _command.DeAttach (item);
                    _command = value;
                    _command.Attach (item);
                } finally {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.ResumeLayout (true);
                }
            }
        }

    }

    [Obsolete]
    public class ToolStripButtonBackend0 : ToolStripButton, IToolStripCommandToggle0, IToolStripItem0 {

        public ToolStripButtonBackend0 () {
            ImageScaling = ToolStripItemImageScaling.None;
        }

        public ToolStripCommand0 _command = null;
        public ToolStripCommand0 Command {
            get { return _command; }
            set { ToolStripUtils0.SetCommand (this, ref _command, value); }
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
}