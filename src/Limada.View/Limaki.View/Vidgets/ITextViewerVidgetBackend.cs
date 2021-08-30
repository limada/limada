using System.Collections.Generic;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    public interface ITextViewerVidgetBackend : ITextViewerBackend, IVidgetBackend, IZoomTarget {

        VidgetBorderStyle BorderStyle { get; set; }

    }
}