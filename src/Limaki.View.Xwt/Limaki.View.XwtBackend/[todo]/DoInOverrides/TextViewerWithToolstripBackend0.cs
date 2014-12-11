using Limaki.View.Vidgets;

namespace Limaki.View.XwtBackend {

    public class TextViewerWithToolstripBackend0 : DummyBackend, ITextViewerWithToolstripVidgetBackend0 {
        public bool ToolStripVisible { get; set; }
        public Drawing.ZoomState ZoomState { get; set; }
        public double ZoomFactor { get; set; }
        public void UpdateZoom () { }
    }
}