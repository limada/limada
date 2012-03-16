using Xwt.Drawing;
using Xwt.Backends;

namespace Limaki.Drawing.WPF {
    public class SystemColorsBackend : ISystemColorsBackend {
        public Color ScrollBar { get { return System.Windows.SystemColors.ScrollBarColor.ToXwt (); } }
        public Color Background { get { return System.Windows.SystemColors.ControlColor.ToXwt (); } }
        public Color ActiveCaption { get { return System.Windows.SystemColors.ActiveCaptionColor.ToXwt (); } }
        public Color InactiveCaption { get { return System.Windows.SystemColors.InactiveCaptionColor.ToXwt (); } }
        public Color Menu { get { return System.Windows.SystemColors.MenuColor.ToXwt (); } }
        public Color Window { get { return System.Windows.SystemColors.WindowColor.ToXwt (); } }
        public Color WindowFrame { get { return System.Windows.SystemColors.WindowFrameColor.ToXwt (); } }
        public Color MenuText { get { return System.Windows.SystemColors.MenuTextColor.ToXwt (); } }
        public Color WindowText { get { return System.Windows.SystemColors.WindowTextColor.ToXwt (); } }
        public Color CaptionText { get { return System.Windows.SystemColors.ActiveCaptionTextColor.ToXwt (); } }
        public Color ActiveBorder { get { return System.Windows.SystemColors.ActiveBorderColor.ToXwt (); } }
        public Color InactiveBorder { get { return System.Windows.SystemColors.InactiveBorderColor.ToXwt (); } }
        public Color ApplicationWorkspace { get { return System.Windows.SystemColors.WindowColor.ToXwt (); } }
        public Color Highlight { get { return System.Windows.SystemColors.HighlightColor.ToXwt (); } }
        public Color HighlightText { get { return System.Windows.SystemColors.HighlightTextColor.ToXwt (); } }
        public Color ButtonFace { get { return System.Windows.SystemColors.ControlColor.ToXwt (); } }
        public Color ButtonShadow { get { return System.Windows.SystemColors.ControlDarkColor.ToXwt (); } }
        public Color GrayText { get { return System.Windows.SystemColors.GrayTextColor.ToXwt (); } }
        public Color ButtonText { get { return System.Windows.SystemColors.ControlTextColor.ToXwt (); } }
        public Color InactiveCaptionText { get { return System.Windows.SystemColors.InactiveCaptionTextColor.ToXwt (); } }
        public Color ButtonHighlight { get { return System.Windows.SystemColors.ControlLightColor.ToXwt (); } }
        public Color TooltipText { get { return System.Windows.SystemColors.InfoTextColor.ToXwt (); } }
        public Color TooltipBackground { get { return System.Windows.SystemColors.InfoColor.ToXwt (); } }
        public Color MenuHighlight { get { return System.Windows.SystemColors.MenuHighlightColor.ToXwt (); } }
        public Color MenuBar { get { return System.Windows.SystemColors.MenuBarColor.ToXwt (); } }
    }
}