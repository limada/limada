using System.IO;
using Limaki.View;
using Xwt;

namespace Limaki.Viewers.Vidgets {

    public interface ITextViewerBackend : IVidgetBackend, IZoomTarget {
        
        bool Multiline { get; set; }

        bool EnableAutoDragDrop { get; set; }
        bool ReadOnly { get; set; }
        bool Modified { get; set; }

        VidgetBorderStyle BorderStyle { get; set; }
        Point AutoScrollOffset { get; set; }

        void Save (Stream stream, TextViewerRtfType rtfType);
        void Load (Stream stream, TextViewerRtfType rtfType);
        void Clear ();
    }
}