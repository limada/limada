/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 - 2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.ObjectModel;
using Limaki.View.Common;

namespace Limaki.View.Vidgets {

    public interface IItemContainer<T> {
        void InsertItem (int index, T item);
        void RemoveItem (T item);
    }

    public class ContainerItemCollection<T> : Collection<T> {
        
        IItemContainer<T> parent;

        internal ContainerItemCollection (IItemContainer<T> parent) {
            this.parent = parent;
        }

        protected override void InsertItem (int index, T item) {
            base.InsertItem (index, item);
            parent.InsertItem (index, item);
        }

        protected override void RemoveItem (int index) {
            var item = this [index];
            base.RemoveItem (index);
            parent.RemoveItem (item);
        }

        protected override void SetItem (int index, T item) {
            var oldItem = this [index];
            base.SetItem (index, item);
            parent.RemoveItem (oldItem);
            parent.InsertItem (index, item);
        }

        protected override void ClearItems () {
            foreach (var item in this)
                parent.RemoveItem (item);
            base.ClearItems ();
        }
    }

    public class ContainerVidget<T, B> : Vidget, IItemContainer<T> where T : IVidget where B:IVidgetBackend  {

        private IContainerVidgetBackend<B> _backend = null;
        public new virtual IContainerVidgetBackend<B> Backend {
            get { return _backend ?? (_backend = (BackendHost.Backend as IContainerVidgetBackend<B>)); }
            set { _backend = value; }
        }

        private ContainerItemCollection<T> _items;
        public ContainerItemCollection<T> Items {
            get { return _items ?? (_items = new ContainerItemCollection<T> (this)); }
        }

        public void AddItems (params T [] items) {
            foreach (var item in items)
                Items.Add (item);

        }

        public override void Dispose () { }

        void IItemContainer<T>.InsertItem (int index, T item) {
            Backend.InsertItem (index, (B) item.Backend);
        }

        void IItemContainer<T>.RemoveItem (T item) {
            Backend.RemoveItem ((B) item.Backend);
        }

        private Visibility _visibility = Visibility.Visible;
        public Visibility Visibility {
            get { return _visibility; }
            set {
                _visibility = value;
                Backend.SetVisibility (value);
            }
        }

    }

    public interface IItemBackendContainer<B> : IItemContainer<B> { }

    public interface IContainerVidgetBackend<B> : IVidgetBackend, IItemBackendContainer<B>  {
        void SetVisibility (Visibility value);
    }
}