using Limaki.Swf.Backends;
using Limaki.Swf.Backends.TextEditor;
using Limaki.Swf.Backends.Viewers;
using Limaki.Swf.Backends.Viewers.ToolStrips;
using Limaki.View.Swf.Backends;
using Limaki.View.Swf.Visualizers;
using Limaki.View.Visuals.Visualizers;
using Limaki.Viewers;
using Limaki.Viewers.StreamViewers;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Viewers.Vidgets;

namespace Limaki.View.Swf {
    public class SwfVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {
            
            base.InitializeBackends();

            RegisterBackend<IImageDisplayBackend, ImageDisplayBackend>();
            RegisterBackend<IVisualsDisplayBackend, VisualsDisplayBackend>();

            RegisterBackend<ITextViewerBackend, TextViewerBackend>();
            RegisterBackend<ITextViewerWithToolstripBackend, TextViewerWithToolstripBackend>();
            RegisterBackend<IWebBrowserBackend, WebBrowserBackend>();

            RegisterBackend<ISplitViewBackend, SplitViewBackend>();

            RegisterBackend<IDigidocViewerBackend, DigidocViewerBackend>();

            RegisterBackend<IArrangerToolStripBackend, ArrangerToolStripBackend>();
            RegisterBackend<IDisplayModeToolStripBackend, DisplayModeToolStripBackend>();
            RegisterBackend<ISplitViewToolStripBackend, SplitViewToolStripBackend>();
            RegisterBackend<ILayoutToolStripBackend, LayoutToolStripBackend>();
            RegisterBackend<IMarkerToolStripBackend, MarkerToolStripBackend>();


        }
    }
}