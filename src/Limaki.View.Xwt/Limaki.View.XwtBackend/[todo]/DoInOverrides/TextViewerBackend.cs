using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {
    public class TextViewerBackend : DummyBackend, ITextViewerBackend {
        public bool Multiline { get; set; }

        public bool EnableAutoDragDrop { get; set; }

        public bool ReadOnly { get; set; }

        public bool Modified { get; set; }

        public VidgetBorderStyle BorderStyle { get; set; }

        public Point AutoScrollOffset { get; set; }

        public void Save (System.IO.Stream stream, TextViewerTextType textType) { }

        public void Load (System.IO.Stream stream, TextViewerTextType textType) { }

        public Drawing.ZoomState ZoomState { get; set; }

        public double ZoomFactor { get; set; }
        public void UpdateZoom () {

        }
    }
}