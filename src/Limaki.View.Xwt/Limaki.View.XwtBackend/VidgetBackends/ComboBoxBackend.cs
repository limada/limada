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

    public class ComboBoxBackend : Xwt.ComboBox, IComboBoxBackend {

        public Vidgets.ComboBox Frontend { get; protected set; }

        public void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (Vidgets.ComboBox)frontend;
            this.Compose ();
        }

        protected void Compose () {
            
        }

        void IVidgetBackend.Update () { this.VidgetBackendUpdate (); }

        void IVidgetBackend.Invalidate () { this.VidgetBackendInvalidate (); }

        void IVidgetBackend.Invalidate (Rectangle rect) { this.VidgetBackendInvalidate (rect); }

        public void Dispose () { }

        public void ItemCollectionChanged (object sender, NotifyCollectionChangedEventArgs e) {
            if (e.Action == NotifyCollectionChangedAction.Add) {
                e.NewItems.Cast<object> ().ForEach (i => this.Items.Add (i));
            } else if (e.Action == NotifyCollectionChangedAction.Remove) {
                e.OldItems.Cast<object> ().ForEach (i => this.Items.Remove (i));
            } else if (e.Action == NotifyCollectionChangedAction.Reset) {
                this.Items.Clear ();
            }
        }

        public void AddSelectionChanged (System.EventHandler value) {
            this.SelectionChanged += value;
        }

        public void RemoveSelectionChanged (System.EventHandler value) {
            this.SelectionChanged -= value;
        }

        public void SetWidth (double value) {
            this.WidthRequest = value;
        }
    }
}