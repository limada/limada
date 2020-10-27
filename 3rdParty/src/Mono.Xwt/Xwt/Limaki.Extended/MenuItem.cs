using System;
using Xwt.Drawing;

namespace Xwt {
    public partial class MenuItem : XwtComponent, ICellContainer {
        public MenuItem (string label, Image image, EventHandler clicked):this(label) {
            Clicked += clicked;
            Image = image;
        }

        public MenuItem (string label, Image image, EventHandler clicked, params MenuItem[] subItems)
            : this(label, image, clicked) {

            if (subItems.Length == 0)
                return;
            SubMenu = new Menu();
            for (int i = 0; i < subItems.Length; i++)
                if(subItems [i]!=null)
                    SubMenu.InsertItem(i, subItems[i]);
        }
    }

    public partial class Menu {
        public Menu (params MenuItem[] subItems):this() { AddItems(subItems); }
        
        public void AddItems (params MenuItem[] subItems) {
            foreach (var item in subItems)
                items.Add(item);
        }
    }
}


