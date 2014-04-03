using System.Windows.Controls;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.WpfBackend {

    public class TextViewerBackend : RichTextBox, ITextViewerBackend {

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
        public void Clear () { }

        #region IVidgetBackend

        public TextViewer Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (TextViewer) frontend;
        }

        public Xwt.Size Size { get { return this.VidgetBackendSize (); } }

        public void Update () { this.VidgetBackendUpdate (); }

        public void Invalidate () { this.VidgetBackendInvalidate (); }

        public void SetFocus () { this.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate (rect); }

        #endregion

        public void Dispose () { }
    }
}