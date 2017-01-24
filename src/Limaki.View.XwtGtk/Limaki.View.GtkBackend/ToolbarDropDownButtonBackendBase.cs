using System.Collections.Generic;
using System.Linq;
using Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {
    /// <summary>
    /// ToolbarDropDownButton with Popup-Menu
    /// </summary>
	/// <remarks>has problem on gkt3 and ubuntu cause icons are not shown any more in toolitems</remarks>
    /// <typeparam name="T"></typeparam>
    public class ToolbarDropDownButtonBackendBase<T> : ToolbarButtonBackendBase<T>, IToolbarDropDownButtonBackend where T : Gtk.ToolItem, new () {

        public new Vidgets.ToolbarDropDownButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (Vidgets.ToolbarDropDownButton)frontend;
        }

        protected override Xwt.ButtonType ButtonType { get { return Xwt.ButtonType.DropDown; } }

        private IList<IToolbarItemBackend> _children = null;
        public IList<IToolbarItemBackend> Children { get { return _children ?? (_children = new List<IToolbarItemBackend> ()); } }

        public void AddItems (params IToolbarItemBackend[] children) {
            foreach (var child in children)
                Children.Add (child);
        }

        protected override void DropDownPressed (object o, Gtk.ButtonPressEventArgs e) {
            base.DropDownPressed (o, e);
            if (e.Event.Button != 1)
                return;
            if (PopupMenu == null) 
                ShowDropDown ();

        }

        protected virtual void ShowDropDown () {
            if (HasChildren) {
                PopupMenu = new Gtk.Menu ();
                PopupMenu.Hidden += (s, o) =>
                    HideDropDown ();
                PopupMenu.SelectionDone += (s, e) => {
                                               var i = 0;
                                           };
                Populate (PopupMenu);
                PopupMenu.ShowAll ();

                PopupMenu.Popup (null, null, delegate (Gtk.Menu menu, out int x, out int y, out bool push_in) {
                                                 var all = ContentWidget.Allocation;
                                                 var loc = GtkBackendHelper.ConvertToScreenCoordinates (ContentWidget, new Xwt.Point(0,all.Height));
                                                 x = (int) loc.X;
                                                 y = (int) loc.Y;
                                                 push_in = true;
                                             }, 0, 
                    Gtk.Global.CurrentEventTime);

                PopupMenu.WidthRequest = this.Widget.Allocation.Width;
                
            }
        }

        protected virtual void HideDropDown () {
            if (PopupMenu == null)
                return;
            PopupMenu.Hide ();
            foreach (var w in PopupMenu.Children.ToArray ().OfType<Gtk.MenuItem> ()) {
                var c = w.Children.FirstOrDefault ();
                if (c != null)
                    w.Remove (c);
                var imageItem = w as Gtk.ImageMenuItem;
                if (imageItem != null)
                    imageItem.Image = null;
                PopupMenu.Remove (w);
            }
            PopupMenu = null;
        }

        public bool HasChildren { get { return _children != null && _children.Count > 0; } }

        protected virtual Gtk.Menu PopupMenu { get; set; }

        protected virtual void Populate (Gtk.Menu menu) {

            foreach (var w in menu.Children.ToArray ())
                menu.Remove (w);

            foreach (var child in Children) {
                var wGtk = child.ToGtk () as Gtk.Widget;
                var image = wGtk;
                var ti = wGtk as Gtk.ToolItem;
                if (ti != null && ti.Child is Gtk.EventBox) {
                    ((Gtk.EventBox) ti.Child).VisibleWindow = false;
                }
                var but = wGtk as Gtk.ToolButton;
                if (but != null)
                    image = but.IconWidget;

                var item = new Gtk.ImageMenuItem ("") {
                                                          Image = image,
                                                          TooltipText = wGtk.TooltipText
                                                      };
                item.Show ();

                menu.Add (item);
                var b = child as IGtkToolbarItemBackend;
                if (b != null) {
                    item.ButtonPressEvent += (s, e) => b.Click (s, e);
                }

                item.ModifyBg (Gtk.StateType.Prelight, Notifycolor);

            }

        }
        
        protected void ChildClicked (object sender, System.EventArgs e) {
            HideDropDown ();
        }
        
        public void InsertItem (int index, IToolbarItemBackend backend) {
            Children.Insert (index, backend);
        }

        public void RemoveItem (IToolbarItemBackend backend) {
            Children.Remove (backend);
        }
    }
}