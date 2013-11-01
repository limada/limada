using Xwt.Backends;
using Xwt.Drawing;
using Xwt.Gdi;

namespace Xwt.Gdi.Backend {

    public class SystemColorsGdiBackend : SystemColorsBackend {

        public override Color ScrollBar { get { return System.Drawing.SystemColors.ScrollBar.ToXwt (); } }
        public override Color Background { get { return System.Drawing.SystemColors.Control.ToXwt (); } }
        public override Color ActiveCaption { get { return System.Drawing.SystemColors.ActiveCaption.ToXwt (); } }
        public override Color InactiveCaption { get { return System.Drawing.SystemColors.InactiveCaption.ToXwt (); } }
        public override Color Menu { get { return System.Drawing.SystemColors.Menu.ToXwt (); } }
        public override Color Window { get { return System.Drawing.SystemColors.Window.ToXwt (); } }
        public override Color WindowFrame { get { return System.Drawing.SystemColors.WindowFrame.ToXwt (); } }
        public override Color MenuText { get { return System.Drawing.SystemColors.MenuText.ToXwt (); } }
        public override Color WindowText { get { return System.Drawing.SystemColors.WindowText.ToXwt (); } }
        public override Color CaptionText { get { return System.Drawing.SystemColors.ActiveCaptionText.ToXwt (); } }
        public override Color ActiveBorder { get { return System.Drawing.SystemColors.ActiveBorder.ToXwt (); } }
        public override Color InactiveBorder { get { return System.Drawing.SystemColors.InactiveBorder.ToXwt (); } }
        public override Color ApplicationWorkspace { get { return System.Drawing.SystemColors.Window.ToXwt (); } }
        public override Color Highlight { get { return System.Drawing.SystemColors.Highlight.ToXwt (); } }
        public override Color HighlightText { get { return System.Drawing.SystemColors.HighlightText.ToXwt (); } }
        public override Color ButtonFace { get { return System.Drawing.SystemColors.ButtonFace.ToXwt (); } }
        public override Color ButtonShadow { get { return System.Drawing.SystemColors.ButtonShadow.ToXwt (); } }
        public override Color GrayText { get { return System.Drawing.SystemColors.GrayText.ToXwt (); } }
        public override Color ButtonText { get { return System.Drawing.SystemColors.WindowText.ToXwt (); } }
        public override Color InactiveCaptionText { get { return System.Drawing.SystemColors.InactiveCaptionText.ToXwt (); } }
        public override Color ButtonHighlight { get { return System.Drawing.SystemColors.ButtonHighlight.ToXwt (); } }
        public override Color TooltipText { get { return System.Drawing.SystemColors.HighlightText.ToXwt (); } }
        public override Color TooltipBackground { get { return System.Drawing.SystemColors.Highlight.ToXwt (); } }
        public override Color MenuHighlight { get { return System.Drawing.SystemColors.MenuHighlight.ToXwt (); } }
        public override Color MenuBar { get { return System.Drawing.SystemColors.MenuBar.ToXwt (); } }

    }

}