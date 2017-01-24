/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType (typeof (IToolbarDropDownButtonBackend))]
    public class ToolbarDropDownButton : ToolbarButton, IToolbarItemContainer {

        public ToolbarDropDownButton () { } 

        public ToolbarDropDownButton (IToolbarCommand command) : base (command) { }

        private IToolbarDropDownButtonBackend _backend = null;

        public new virtual IToolbarDropDownButtonBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolbarDropDownButtonBackend); } 
            set { _backend = value; }
        }

        private ToolbarItemCollection _items;
        public ToolbarItemCollection Items { get { return _items ?? (_items = new ToolbarItemCollection (this)); } }

        public void AddItems (params ToolbarItem[] items) {
            foreach (var item in items)
                Items.Add (item);

        }

        public void InsertItem (int index, ToolbarItem item) {
            Backend.InsertItem (index, item.Backend);
        }

        public void RemoveItem (ToolbarItem item) {
            Backend.RemoveItem (item.Backend);
        }

        public override void Dispose () {
        }
    }

    public interface IToolbarDropDownButtonBackend : IToolbarButtonBackend, IToolbarItemBackendContainer { }

}