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

using Limaki.Common.Linqish;
using Limaki.View.Vidgets;
using System.Collections.Specialized;
using System.Linq;
using Xwt;
using System;
using System.Collections;
using System.Reflection;

namespace Limaki.View.XwtBackend {

    public class ComboBoxBackend : VidgetBackend<Xwt.ComboBox>, IComboBoxBackend {

        public new Vidgets.ComboBox Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (Vidgets.ComboBox)frontend;
        }

        protected ListStore Store { get; set; }
        protected IDataField DataField { get; set; }
        protected MethodInfo AddValueMethodInfo { get; set; }

        protected override void Compose () {
            base.Compose ();
        }

        public void ItemCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                CheckItemType (e.NewItems);
                var r = Store.AddRow ();
                e.NewItems.Cast<object> ().ForEach (
                    item =>
                    AddValueMethodInfo.Invoke (Store, new object[] { r, DataField, item })
                );
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                CheckItemType (e.OldItems);
                for (int i = 0; i < e.OldItems.Count; i++)
                    Store.RemoveRow (i);
            } else if (e.Action == NotifyCollectionChangedAction.Reset) {
                if (Store != null)
                    Store.Clear ();
            }
        }

        private void CheckItemType (IList items) {
            if (items.Count == 0)
                return;
            Type itemType = null;
            foreach (var item in items) {
                if (itemType == null) {
                    itemType = item.GetType ();
                } else if (itemType != item.GetType ()) { 
                    throw new ArgumentException ("items must be of same type");
                }
            }
            if (typeof (Xwt.Drawing.Image).IsAssignableFrom (itemType)) {
                itemType = typeof (Xwt.Drawing.Image);
            }

            if (Store == null) {
                var dft = typeof (DataField<>).MakeGenericType (itemType);
                DataField = Activator.CreateInstance (dft) as IDataField;
                Store = new ListStore (DataField);
                AddValueMethodInfo = new MethodExtractor ()
                    .GetMethodInfo<ListStore> (l => l.SetValue (0, new DataField<object> (), null))
                    .GetGenericMethodDefinition ().MakeGenericMethod (itemType);
                Widget.ItemsSource = Store;
                var cellView = CellView.GetDefaultCellView (DataField);
                Widget.Views.Add (cellView);
            } else {
                var dataSource = Store as IListDataSource;
                if (dataSource.ColumnTypes[0] != itemType) {
                    // problem: in Gtk ColumnTypes[0] is ImageDescription
                    //throw new ArgumentException (string.Format ("items must be of type {0}", itemType));
                }
            }
        }

        public void AddSelectionChanged (System.EventHandler value) {
            Widget.SelectionChanged += value;
        }

        public void RemoveSelectionChanged (System.EventHandler value) {
            Widget.SelectionChanged -= value;
        }

        public void SetWidth (double value) {
            Widget.WidthRequest = value;
        }

        public int SelectedIndex {
            get { return Widget.SelectedIndex; }
            set { Widget.SelectedIndex = value; }
        }
    }
}