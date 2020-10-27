using Xwt.Backends;

namespace Xwt.GtkBackend {

    public partial class CanvasBackend {
        public void AddChild (IWidgetBackend widget, Rectangle bounds)
        {
            if (widget is IGtkWidgetBackend bk) {
                bk.AllocEventBox ();
                var w = bk.Widget;
                Widget.Add (w);
                Widget.SetAllocation (w, bounds);
            }
        }
    }

}