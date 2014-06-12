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

    [BackendType (typeof (IToolStripDropDownButtonBackend))]
    public class ToolStripDropDownButton : ToolStripButton, IToolStripItemContainer {

        public ToolStripDropDownButton () { } 

        public ToolStripDropDownButton (IToolStripCommand command) : base (command) { }

        private IToolStripDropDownButtonBackend _backend = null;

        public new virtual IToolStripDropDownButtonBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripDropDownButtonBackend); } 
            set { _backend = value; }
        }

        private ToolStripItemCollection _items;
        public ToolStripItemCollection Items { get { return _items ?? (_items = new ToolStripItemCollection (this)); } }

        public void AddItems (params ToolStripItem[] items) {
            foreach (var item in items)
                Items.Add (item);

        }

        void IToolStripItemContainer.InsertItem (int index, ToolStripItem item) {
            Backend.InsertItem (index, (IToolStripItemBackend) item.Backend);
        }

        void IToolStripItemContainer.RemoveItem (ToolStripItem item) {
            Backend.RemoveItem ((IToolStripItemBackend) item.Backend);
        }

        public override void Dispose () {
        }
    }

    public interface IToolStripDropDownButtonBackend : IToolStripButtonBackend, IToolStripItemBackendContainer { }

}