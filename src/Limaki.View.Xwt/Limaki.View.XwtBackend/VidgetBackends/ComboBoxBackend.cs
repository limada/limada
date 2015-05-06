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

namespace Limaki.View.XwtBackend {

    public class ComboBoxBackend : VidgetBackend<Xwt.ComboBox>, IComboBoxBackend {

        public Vidgets.ComboBox Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (Vidgets.ComboBox)frontend;
        }

        protected override void Compose () {
            base.Compose ();
        }

        public void ItemCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                e.NewItems.Cast<object> ().ForEach (i => Widget.Items.Add (i));
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                e.OldItems.Cast<object> ().ForEach (i => Widget.Items.Remove (i));
            } else if (e.Action == NotifyCollectionChangedAction.Reset) {
                Widget.Items.Clear ();
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