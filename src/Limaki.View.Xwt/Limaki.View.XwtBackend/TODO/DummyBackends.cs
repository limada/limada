using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.View;
using Limaki.Viewers.StreamViewers;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Viewers.Vidgets;
using Limaki.Visuals;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class DummyBackend : Frame, IVidgetBackend {

        void IVidgetBackend.Update () { XwtBackendHelper.VidgetBackendUpdate(this); }

        void IVidgetBackend.Invalidate () { XwtBackendHelper.VidgetBackendInvalidate(this); }

        void IVidgetBackend.Invalidate (Rectangle rect) { XwtBackendHelper.VidgetBackendInvalidate(this, rect); }

        private IVidget frontend;
        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.frontend = frontend;
        }
    }

    public class ArrangerToolStripBackend : DummyBackend, IArrangerToolStripBackend {
    }

    public class TextViewerWithToolstripBackend : DummyBackend, ITextViewerWithToolstripBackend {
        public bool ToolStripVisible { get; set; }
        public Drawing.ZoomState ZoomState { get; set; }
        public double ZoomFactor { get; set; }
        public void UpdateZoom () { }
    }

    public class TextViewerBackend : DummyBackend, ITextViewerBackend {
        public bool Multiline { get; set; }

        public bool EnableAutoDragDrop { get; set; }

        public bool ReadOnly { get; set; }

        public bool Modified { get; set; }

        public VidgetBorderStyle BorderStyle { get; set; }

        public Point AutoScrollOffset { get; set; }

        public void Save (System.IO.Stream stream, TextViewerRtfType rtfType) { }

        public void Load (System.IO.Stream stream, TextViewerRtfType rtfType) { }

        public Drawing.ZoomState ZoomState { get; set; }

        public double ZoomFactor { get; set; }
        public void UpdateZoom () {

        }
    }

    public class SplitViewToolStripBackend : DummyBackend, ISplitViewToolStripBackend {
        public Viewers.SplitViewMode ViewMode { get; set; }

        public void CheckBackForward (Viewers.ISplitView splitView) {

        }

        public void AttachSheets () {

        }
    }

    public class MarkerToolStripBackend : DummyBackend, IMarkerToolStripBackend {
        public void Attach (Drawing.IGraphScene<IVisual, IVisualEdge> scene) {

        }

        public void Detach (Drawing.IGraphScene<Limaki.Visuals.IVisual, Limaki.Visuals.IVisualEdge> oldScene) {

        }
    }

    public class LayoutToolStripBackend : DummyBackend, ILayoutToolStripBackend {
        public void AttachStyleSheet (string sheetName) {

        }

        public void DetachStyleSheet (string sheetName) {

        }
    }

    public class DisplayModeToolStripBackend : DummyBackend, IDisplayModeToolStripBackend {
    }

    public class DigidocViewerBackend : DummyBackend, IDigidocViewerBackend {
    }
}
