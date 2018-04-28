using System;
using Limaki.View.Common;
using Limaki.View.Vidgets;

namespace Limaki.View.XwtBackend {
    [Obsolete]
    public class TextViewerWithToolstripBackend0 : DummyBackend, ITextViewerWithToolstripVidgetBackend0 {
        public bool ToolStripVisible { get; set; }
        public ZoomState ZoomState { get; set; }
        public double ZoomFactor { get; set; }
        public void UpdateZoom () { }
    }
}