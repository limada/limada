using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class PlainTextBoxBackend : VidgetBackend<TextEntry>, IPlainTextBoxVidgetBackend {

        protected override void Compose () {
            base.Compose ();
            Widget.MultiLine = true;
        }

        public new Vidgets.PlainTextBox Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Frontend = (Vidgets.PlainTextBox)frontend;
        }

        public string Text {
            get { return Widget.Text; }
            set { Widget.Text = value; }
        }
        
        public bool ShowFrame { get { return Widget.ShowFrame; } set { Widget.ShowFrame = value; } }
    }
}