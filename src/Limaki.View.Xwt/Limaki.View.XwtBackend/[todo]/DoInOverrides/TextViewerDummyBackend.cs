using System.Collections.Generic;
using Limaki.View.Vidgets;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class TextViewerDummyBackend : DummyBackend, ITextViewerVidgetBackend {

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
        public void UpdateZoom () { }

        public void SetAttribute (TextAttribute attribute) { }

        public IEnumerable<TextAttribute> GetAttributes () { yield break; }


        public event System.EventHandler SelectionChanged;
    }
}