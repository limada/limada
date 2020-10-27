using Gtk;
using Xwt.Backends;

namespace Xwt.GtkBackend {
    
    public partial class TableViewBackend {
        void MapTitle (ListViewColumn col, Gtk.TreeViewColumn tc)
        {
            if (col.HeaderView == null)
                tc.Title = col.Title;
            else {
                var oldWidget = tc.Widget;
                tc.Widget = CellUtil.CreateCellRenderer (ApplicationContext, col.HeaderView);
               
                if (tc.Widget != null) {
                    // lytic: added show; otherwise the header is not visible
                    tc.Widget.Show ();

                    if (oldWidget == null) { // avoid to add events multiple times
                        tc.Clickable = true;
                        var chk = tc.Widget as Gtk.CheckButton;
                        if (chk != null) {
                            tc.Clicked += (s, e) => chk.Click();
                        }
                        else {
                            tc.Clicked += (s, e) => {
                                if (s is TreeViewColumn twc && twc.Widget is Gtk.Widget sw) {
                                    sw.GetPointer(out var x, out var y);
                                    var allocation = sw.Allocation;
                                    if (allocation.Contains(x + allocation.Left, y + allocation.Top)) {
                                        col.HeaderView.RaiseButtonPressed(new ButtonEventArgs {
                                            X = x,
                                            Y = y,
                                            Button = PointerButton.Left
                                        });

                                    };
                                }
                            };
                        }
                    }
                    if (false) {
                        // http://stackoverflow.com/questions/6960243/how-do-you-attach-a-popup-menu-to-a-column-header-button-in-gtk2-using-pygobject
                        var top = tc.Widget;
                        // not working: event never fired
                        while (top.Parent != Widget) {
                            top = top.Parent;
                            top.AddEvents ((int)Gdk.EventMask.ButtonPressMask);
                            top.ButtonPressEvent += (s, e) => {
                                col.HeaderView.RaiseButtonPressed (new ButtonEventArgs {
                                    X = e.Event.X,
                                    Y = e.Event.Y,
                                    Button = (PointerButton)e.Event.Button
                                });
                            };
                        }
                    }
                }
            }
        }
    }
}