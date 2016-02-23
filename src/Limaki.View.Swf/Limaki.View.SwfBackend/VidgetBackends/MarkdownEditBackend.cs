using System;
using System.Windows.Forms;
using Limaki.View.Vidgets;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class MarkdownEditBackend : VidgetBackend<MarkdownEditBackend.MDBox>, IMarkdownEditBackend {

        public class MDBox : Panel {
            public void RaiseFocus (object sender, EventArgs e) {
                base.OnGotFocus (e);
            }
        }

        protected override void Compose () {
            base.Compose ();
            Control.Dock = System.Windows.Forms.DockStyle.Fill;
        }
        public new Vidgets.MarkdownEdit Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Frontend = (Vidgets.MarkdownEdit) frontend;
            IsEmpty = true;
            //Widget.KeyPressed += ToggleEditMode;
        }

        public bool IsEmpty { get; protected set; }

        public void Compose (IMarkdownViewer viewer) {
            var vidget = viewer as IVidget;
            var viewerBackend = vidget.Backend.ToSwf ();
            viewerBackend.GotFocus -= Control.RaiseFocus;
            viewerBackend.GotFocus += Control.RaiseFocus;
            viewerBackend.KeyUp -= ToggleEditMode;
            viewerBackend.KeyUp += ToggleEditMode;
        }

        public void Activate (IMarkdownViewer viewer) {
            Control.SuspendLayout ();
            Control.Controls.Clear ();
            var vidget = viewer as IVidget;
            var viewerBackend = vidget.ToSwf ();
            viewerBackend.Dock = System.Windows.Forms.DockStyle.Fill;
            Control.Controls.Add (viewerBackend);
            Control.ResumeLayout ();
            var wb = viewerBackend as IWebBrowser;
            if (wb != null)
                wb.MakeReady ();
            viewerBackend.Focus ();
            IsEmpty = false;
        }

        private void ToggleEditMode (object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == System.Windows.Forms.Keys.F2) {
                Frontend.InEdit = !Frontend.InEdit;
            }
        }
    }
}