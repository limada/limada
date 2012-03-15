using Xwt.Drawing;

namespace Xwt.Backends {

    public interface ISystemColorsBackend {
        Color ScrollBar { get; }
        Color Background { get; }
        Color ActiveCaption { get; }
        Color InactiveCaption { get; }
        Color Menu { get; }
        Color Window { get; }
        Color WindowFrame { get; }
        Color MenuText { get; }
        Color WindowText { get; }
        Color CaptionText { get; }
        Color ActiveBorder { get; }
        Color InactiveBorder { get; }
        Color ApplicationWorkspace { get; }
        Color Highlight { get; }
        Color HighlightText { get; }
        Color ButtonFace { get; }
        Color ButtonShadow { get; }
        Color GrayText { get; }
        Color ButtonText { get; }
        Color InactiveCaptionText { get; }
        Color ButtonHighlight { get; }
        Color TooltipText { get; }
        Color TooltipBackground { get; }
        Color MenuHighlight { get; }
        Color MenuBar { get; }
    }

}