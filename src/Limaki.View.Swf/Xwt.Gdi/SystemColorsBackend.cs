using Xwt.Backends;
using Xwt.Drawing;
using Xwt.Gdi;

namespace Xwt.Gdi.Backend {

    public class SystemColorsBackend : ISystemColorsBackend {

        public Color ScrollBar { get { return System.Drawing.SystemColors.ScrollBar.ToXwt (); } }
        public Color Background { get { return System.Drawing.SystemColors.Control.ToXwt (); } }
        public Color ActiveCaption { get { return System.Drawing.SystemColors.ActiveCaption.ToXwt (); } }
        public Color InactiveCaption { get { return System.Drawing.SystemColors.InactiveCaption.ToXwt (); } }
        public Color Menu { get { return System.Drawing.SystemColors.Menu.ToXwt (); } }
        public Color Window { get { return System.Drawing.SystemColors.Window.ToXwt (); } }
        public Color WindowFrame { get { return System.Drawing.SystemColors.WindowFrame.ToXwt (); } }
        public Color MenuText { get { return System.Drawing.SystemColors.MenuText.ToXwt (); } }
        public Color WindowText { get { return System.Drawing.SystemColors.WindowText.ToXwt (); } }
        public Color CaptionText { get { return System.Drawing.SystemColors.ActiveCaptionText.ToXwt (); } }
        public Color ActiveBorder { get { return System.Drawing.SystemColors.ActiveBorder.ToXwt (); } }
        public Color InactiveBorder { get { return System.Drawing.SystemColors.InactiveBorder.ToXwt (); } }
        public Color ApplicationWorkspace { get { return System.Drawing.SystemColors.Window.ToXwt (); } }
        public Color Highlight { get { return System.Drawing.SystemColors.Highlight.ToXwt (); } }
        public Color HighlightText { get { return System.Drawing.SystemColors.HighlightText.ToXwt (); } }
        public Color ButtonFace { get { return System.Drawing.SystemColors.ButtonFace.ToXwt (); } }
        public Color ButtonShadow { get { return System.Drawing.SystemColors.ButtonShadow.ToXwt (); } }
        public Color GrayText { get { return System.Drawing.SystemColors.GrayText.ToXwt (); } }
        public Color ButtonText { get { return System.Drawing.SystemColors.WindowText.ToXwt (); } }
        public Color InactiveCaptionText { get { return System.Drawing.SystemColors.InactiveCaptionText.ToXwt (); } }
        public Color ButtonHighlight { get { return System.Drawing.SystemColors.ButtonHighlight.ToXwt (); } }
        public Color TooltipText { get { return System.Drawing.SystemColors.HighlightText.ToXwt (); } }
        public Color TooltipBackground { get { return System.Drawing.SystemColors.Highlight.ToXwt (); } }
        public Color MenuHighlight { get { return System.Drawing.SystemColors.MenuHighlight.ToXwt (); } }
        public Color MenuBar { get { return System.Drawing.SystemColors.MenuBar.ToXwt (); } }

    }

}