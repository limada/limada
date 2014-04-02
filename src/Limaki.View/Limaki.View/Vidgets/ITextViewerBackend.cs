using System.IO;
using Xwt;

namespace Limaki.View.Vidgets {

    public interface ITextViewerBackend : IVidgetBackend, IZoomTarget {

        bool EnableAutoDragDrop { get; set; }
        bool ReadOnly { get; set; }
        bool Modified { get; set; }

        VidgetBorderStyle BorderStyle { get; set; }
        Point AutoScrollOffset { get; set; }

        void Save (Stream stream, TextViewerTextType textType);
        void Load (Stream stream, TextViewerTextType textType);

        void Clear ();
    }
}