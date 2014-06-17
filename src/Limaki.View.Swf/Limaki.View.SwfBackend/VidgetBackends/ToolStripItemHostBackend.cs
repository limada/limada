using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Limaki.Common.Linqish;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;

namespace Limaki.View.SwfBackend.VidgetBackends {
    public class ToolStripItemHostBackend : ToolStripItemBackend<ToolStripItemHostBackend.ToolStripControlHost>, IToolStripItemHostBackend {

        public class ToolStripControlHost : System.Windows.Forms.ToolStripControlHost {

            public ToolStripControlHost (Control control) : base (control) { }

            public override System.Drawing.Font Font {
                get { return Control.Font; }
                set {
                    Control.Font = value;
                    var panel = Control as System.Windows.Forms.Panel;
                    panel.SuspendLayout ();
                    panel.Font = value;
                    panel.Controls.Cast<Control> ().ForEach (c => c.Font = value);
                    panel.ResumeLayout ();
                }
            }

            protected override void SetBounds (System.Drawing.Rectangle bounds) {
                if (Control != null)
                    Control.SetBounds (bounds.X, bounds.Y, bounds.Width, bounds.Height);
            }
        }

        protected override void Compose () {
            this.Control = new ToolStripItemHostBackend.ToolStripControlHost (
                new System.Windows.Forms.Panel {
                                  Dock = DockStyle.Fill,
                                  BorderStyle = BorderStyle.None,
                                  Margin = new Padding (),
                                  Padding = new Padding (),
                              });

            Control.Overflow = ToolStripItemOverflow.AsNeeded;
        }

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public Vidgets.ToolStripItemHost Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (Vidgets.ToolStripItemHost)frontend;
        }

        public void SetChild (Vidget value) {
            var control = value.ToSwf ();
            var panel = Control.Control as System.Windows.Forms.Panel;
            panel.SuspendLayout ();
            panel.Controls.Clear ();
            panel.Controls.Add (control);
            Control.Size = value.Size.ToGdi ();
            panel.ResumeLayout ();
        }

    }
}