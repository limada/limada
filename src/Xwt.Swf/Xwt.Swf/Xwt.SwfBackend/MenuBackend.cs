using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xwt.Backends;
using SWF = System.Windows.Forms;
using SD = System.Drawing;

namespace Xwt.Swf.Xwt.SwfBackend {

    public class MenuBackend : Backend<Menu>, IMenuBackend {
        public SWF.MenuStrip Menu { get; protected set; }

        internal SWF.ToolStripItemCollection ItemsCollection { get; set; }

        ObservableCollection<IMenuItemBackend> _items = new ObservableCollection<IMenuItemBackend> (); 
        public override void InitializeBackend (object frontend, ApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Menu = new SWF.MenuStrip ();
            ItemsCollection = Menu.Items;
            _items.CollectionChanged += (sender,args) => {
                var backend = args.NewItems.Cast<MenuItemBackend> ().FirstOrDefault();
                if (args.Action == NotifyCollectionChangedAction.Add) {
                    ItemsCollection.Insert (args.NewStartingIndex, backend.MenuItem);
                }
                if (args.Action == NotifyCollectionChangedAction.Remove) {
                    ItemsCollection.Remove (backend.MenuItem);
                }
            };
        }

        public object Font {
            get { return Menu.Font; }
            set { Menu.Font = (SD.Font)value; }
        }


        public override void DisableEvent (object eventId) {
        }

        public override void EnableEvent (object eventId) {
        }

        public void InsertItem (int index, IMenuItemBackend menuItem) {
           _items.Add (menuItem);
            
        }

        public void RemoveItem (IMenuItemBackend menuItem) {
            _items.Remove (menuItem);
        }

        public void Popup () {
        }

        public void Popup (IWidgetBackend widget, double x, double y) {
        }


    }
}

