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

    [BackendType (typeof (IToolStripBackend))]
    public class ToolStrip : Vidget, IToolStripItemContainer {

        private ToolStripItemCollection _items;
        public ToolStripItemCollection Items {
            get { return _items ?? (_items = new ToolStripItemCollection (this)); }
        }

        private IToolStripBackend _backend = null;
        public new virtual IToolStripBackend Backend {
            get { return _backend ?? (_backend = BackendHost.Backend as IToolStripBackend); } 
            set { _backend = value; }
        }

        public void AddItems (params ToolStripItem[] items) {
            foreach (var item in items)
                Items.Add (item);

        }

        public override void Dispose () { }

        void IToolStripItemContainer.InsertItem (int index, ToolStripItem item) {
            Backend.InsertItem (index, (IToolStripItemBackend) item.Backend);
        }

        void IToolStripItemContainer.RemoveItem (ToolStripItem item) {
            Backend.RemoveItem ((IToolStripItemBackend) item.Backend);
        }

        private Visibility _vsibility = Visibility.Visible;
        public Visibility Visibility {
            get { return _vsibility; }
            set {
                this.Visibility = value;
                Backend.SetVisibility (value);
            }
        }
    }

    public interface IToolStripItemBackendContainer {
        void InsertItem (int index, IToolStripItemBackend toolStripItemBackend);
        void RemoveItem (IToolStripItemBackend toolStripItemBackend);
    }

    public interface IToolStripBackend : IVidgetBackend, IToolStripItemBackendContainer {
        void SetVisibility (Visibility value);
    }
}