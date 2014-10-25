using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using ToolStripButton = System.Windows.Forms.ToolStripButton;

namespace Limaki.View.SwfBackend.VidgetBackends {
    public class ToolStripButtonBackend0 : ToolStripButton, IToolStripCommandToggle0, IToolStripItem0 {

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
}