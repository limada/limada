using Limada.View.Vidgets;
using Limaki.Swf.Backends;
using Limaki.Swf.Backends.TextEditor;
using Limaki.Swf.Backends.Viewers;
using Limaki.Swf.Backends.Viewers.ToolStrips;
using Limaki.Usecases.Vidgets;
using Limaki.View.Swf.Backends;
using Limaki.View.Swf.Visualizers;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using Limaki.View.Viz.Visuals;

namespace Limaki.View.Swf {
    public class SwfVidgetToolkitEngineBackend : VidgetToolkitEngineBackend {

        public override void InitializeBackends () {
            
            base.InitializeBackends();

            RegisterBackend<ISdImageDisplayBackend, SdImageDisplayBackend>();
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

            RegisterBackend<ICanvasVidgetBackend, CanvasVidgetBackend>();

            RegisterBackend<IOpenfileDialogBackend, OpenFileDialogBackend>();
            RegisterBackend<ISavefileDialogBackend, SaveFileDialogBackend>();

            RegisterBackend<IVindowBackend, VindowBackend> ();
        }
    }
}