using System.Linq;
using Xwt;

namespace Limaki.View.XwtBackend {

    public abstract class PanedBackend : VidgetBackend<HPaned>  {

        protected Paned SplitContainer { get { return this.Widget; } }

        protected virtual Widget SetScrollingPanelContent (Widget widget, Panel panel) {
            var panelScroll = panel.Content as ScrollView;
            if (panelScroll != null) {
                panelScroll.Content = widget;
            } else {
                panel.Content = widget.WithScrollView ();
            }
            return panel.Content;
        }

        protected Panel PanelOf (IVidget vidget) {
            var widget = vidget.Backend.ToXwt ();

            if (SplitContainer.Panel1.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel1;
            } else if (SplitContainer.Panel2.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel2;
            }
            return null;
        }

        protected Panel AdjacentPanelOf (IVidget vidget) {
            var widget = vidget.Backend.ToXwt ();

            if (SplitContainer.Panel1.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel2;
            } else if (SplitContainer.Panel2.Content.ScrollPeeledChildren ().Contains (widget)) {
                return SplitContainer.Panel1;
            }
            return null;
        }
    }
}