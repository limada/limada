using Limaki.View.Vidgets;
using Xwt;
using SWF = System.Windows.Forms;
using Xwt.GdiBackend;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class PlainTextBoxBackend : VidgetBackend<SWF.TextBox>, IPlainTextBoxVidgetBackend {

        protected override void Compose () {
            base.Compose ();
            Control.Multiline = true;
            Control.Font = Xwt.Drawing.Font.SystemSerifFont.WithSize(10).ToGdi ();
        }

        public new PlainTextBox Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Frontend = (Vidgets.PlainTextBox)frontend;
        }

        public string Text {
            get { return Control.Text; }
            set { Control.Text = value; }
        }


        public bool ShowFrame {
            get { return Control.BorderStyle != SWF.BorderStyle.None; }
            set {
                if (value)
                    Control.BorderStyle = SWF.BorderStyle.FixedSingle;
                else
                    Control.BorderStyle = SWF.BorderStyle.None;
            }
        }
    }
}