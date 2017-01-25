using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.Toolbars;

namespace Limaki.View.GtkBackend {

    public class ArrangerToolbarBackend : ToolbarBackend, IArrangerToolbarBackend { }

    public class DisplayModeToolbarBackend : ToolbarBackend, IDisplayModeToolbarBackend { }

    public class SplitViewToolbarBackend : ToolbarBackend, ISplitViewToolbarBackend { }

    public class MarkerToolbarBackend : ToolbarBackend, IMarkerToolbarBackend { }

    public class LayoutToolbarBackend : ToolbarBackend, ILayoutToolbarBackend { }
    
}