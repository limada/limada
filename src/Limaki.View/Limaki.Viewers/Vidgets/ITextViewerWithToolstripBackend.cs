using Limaki.View;

namespace Limaki.Viewers.Vidgets {
    public interface ITextViewerWithToolstripBackend : IVidgetBackend, IZoomTarget {
        bool ToolStripVisible { get; set; }
    }
}