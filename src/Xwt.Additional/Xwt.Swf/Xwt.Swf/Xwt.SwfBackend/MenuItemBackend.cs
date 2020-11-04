using System;
using Xwt.Backends;

namespace Xwt.Swf.Xwt.SwfBackend {
    
    public class MenuItemBackend : Backend<MenuItem>, IMenuItemBackend {
        
        public System.Windows.Forms.ToolStripItem MenuItem { get; protected set; }

        public override void InitializeBackend (object frontend, ApplicationContext context) {
            base.InitializeBackend (frontend, context);
            MenuItem = new System.Windows.Forms.ToolStripMenuItem ();
        }

        private IMenuItemEventSink eventSink;
        public void Initialize (IMenuItemEventSink eventSink) {
            this.eventSink = eventSink;
        }

        public override void EnableEvent (object eventId) {
            if (MenuItem == null)
                return;

            if (eventId is MenuItemEvent) {
                switch ((MenuItemEvent)eventId) {
                    case MenuItemEvent.Clicked:
                        this.MenuItem.Click += MenuItemClickHandler;
                        break;
                }
            }
        }

        public override void DisableEvent (object eventId) {
            if (MenuItem == null)
                return;

            if (eventId is MenuItemEvent) {
                switch ((MenuItemEvent)eventId) {
                    case MenuItemEvent.Clicked:
                        this.MenuItem.Click -= MenuItemClickHandler;
                        break;
                }
            }
        }

        void MenuItemClickHandler (object sender, EventArgs args) {
            Context.InvokeUserCode (eventSink.OnClicked);
        }

        public string Label {
            get { return MenuItem.Text; }
            set { MenuItem.Text = value; }
        }

        public string TooltipText { get; set; }

        public bool Sensitive {
            get { return MenuItem.Enabled; }
            set { MenuItem.Enabled = value; }
        }

        public bool UseMnemonic {
            get { return false; }
            set { }
        }

        public bool Visible {
            get { return MenuItem.Visible; }
            set { MenuItem.Visible = value; }
        }

        public void SetFormattedText (FormattedText text) {
            MenuItem.Text = text.Text;
        }

        public void Dispose () {
            MenuItem.Dispose ();
        }

        public void SetImage (ImageDescription image) {
        }

        public void SetSubmenu (IMenuBackend menu) {
            var backend = (MenuBackend)menu;
            var tsmi = (this.MenuItem as System.Windows.Forms.ToolStripMenuItem);
            foreach (System.Windows.Forms.ToolStripItem item in backend.Menu.Items) {
                item.Owner = null;
                tsmi.DropDownItems.Add (item);
            }
            backend.ItemsCollection = tsmi.DropDownItems;
            //tsmi.DropDownItems.AddRange (backend.Menu.Items);

        }

    }
}