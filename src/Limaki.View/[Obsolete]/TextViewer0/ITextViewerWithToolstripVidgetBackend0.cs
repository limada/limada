using System;

namespace Limaki.View.Vidgets {

    [Obsolete]
    public interface ITextViewerWithToolstripVidgetBackend0 : IVidgetBackend, IZoomTarget {
        bool ToolStripVisible { get; set; }
    }
}