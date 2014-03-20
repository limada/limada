namespace Limaki.View.Vidgets {
    public interface ITextViewerWithToolstripBackend : IVidgetBackend, IZoomTarget {
        bool ToolStripVisible { get; set; }
    }
}