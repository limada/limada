using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Limaki.Common.Linqish;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using SWF = System.Windows.Forms;
using SD = System.Drawing;
using System.Diagnostics;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class ToolbarItemHostBackend : ToolStripItemBackend<ToolbarItemHostBackend.ToolStripControlHost>, IToolbarItemHostBackend {

        public class ToolStripControlHost : SWF.ToolStripControlHost {

            public ToolStripControlHost (Control control) : base (control) { }

            public override SD.Font Font {
                get { return Control.Font; }
                set {
                    Control.Font = value;
                    Control.SuspendLayout ();
                    Control.Font = value;
                    Control.Controls.Cast<Control> ().ForEach (c => c.Font = value);
                    Control.ResumeLayout ();
                }
            }

        }

        protected override void Compose () {
            this.Control = new ToolbarItemHostBackend.ToolStripControlHost (
                new SWF.Panel {
                    Dock = DockStyle.Fill,
                    BorderStyle = BorderStyle.None,
                    Margin = new Padding (),
                    Padding = new Padding (),
                });

            Control.Overflow = ToolStripItemOverflow.AsNeeded;
        }

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public new Vidgets.ToolbarItemHost Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (Vidgets.ToolbarItemHost)frontend;
        }

        public void SetChild (Vidget value) {
            var child = value.ToSwf ();

            var parent = this.Control.GetCurrentParent ();
            var controlHost = new ToolbarItemHostBackend.ToolStripControlHost (child) {
                Size = child.Size,
                Overflow = ToolStripItemOverflow.AsNeeded,
            };
            if (parent != null) {
                var i = parent.Items.IndexOf (this.Control);
                parent.SuspendLayout ();
                parent.Items.RemoveAt (i);
                parent.Items.Insert (i, controlHost);
                parent.ResumeLayout ();
            } 
            this.Control = controlHost;

        }

    }
}