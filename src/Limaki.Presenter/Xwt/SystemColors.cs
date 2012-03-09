using Xwt.Backends;
using Xwt.Engine;
namespace Xwt.Drawing {
    public static class SystemColors {

        static SystemColors() {
            backend= WidgetRegistry.CreateBackend<ISystemColorsBackend> (typeof (SystemColors));
        }

        static ISystemColorsBackend backend;

        public static Color ScrollBar { get { return backend.ScrollBar; } }
        public static Color Background { get { return backend.Background; } }
        public static Color ActiveCaption { get { return backend.ActiveCaption; } }
        public static Color InactiveCaption { get { return backend.InactiveCaption; } }
        public static Color Menu { get { return backend.Menu; } }
        public static Color Window { get { return backend.Window; } }
        public static Color WindowFrame { get { return backend.WindowFrame; } }
        public static Color MenuText { get { return backend.MenuText; } }
        public static Color WindowText { get { return backend.WindowText; } }
        public static Color CaptionText { get { return backend.CaptionText; } }
        public static Color ActiveBorder { get { return backend.ActiveBorder; } }
        public static Color InactiveBorder { get { return backend.InactiveBorder; } }
        public static Color ApplicationWorkspace { get { return backend.ApplicationWorkspace; } }
        public static Color Highlight { get { return backend.Highlight; } }
        public static Color HighlightText { get { return backend.HighlightText; } }
        public static Color ButtonFace { get { return backend.ButtonFace; } }
        public static Color ButtonShadow { get { return backend.ButtonShadow; } }
        public static Color GrayText { get { return backend.GrayText; } }
        public static Color ButtonText { get { return backend.ButtonText; } }
        public static Color InactiveCaptionText { get { return backend.InactiveCaptionText; } }
        public static Color ButtonHighlight { get { return backend.ButtonHighlight; } }
        public static Color TooltipText { get { return backend.TooltipText; } }
        public static Color TooltipBackground { get { return backend.TooltipBackground; } }
        public static Color MenuHighlight { get { return backend.MenuHighlight; } }
        public static Color MenuBar { get { return backend.MenuBar; } }

    }
}

