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

using Limaki.View.Vidgets;
using System;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public class ToolStripDropDownButtonBackend : ToolStripDropDownButton, IToolStripDropDownButtonBackend {

        #region IVidgetBackend Member

        public LVV.ToolStripDropDownButton Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStripDropDownButton)frontend;
        }

        public Xwt.Size Size { get { return this.VidgetBackendSize (); } }

        public void Update () { this.VidgetBackendUpdate (); }

        public void Invalidate () { this.VidgetBackendInvalidate (); }

        public void SetFocus () { this.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate (rect); }

        #endregion

        public void SetImage (Xwt.Drawing.Image value) {
            base.Image = value;
        }

        public void SetLabel (string value) {
            base.Label = value;
        }

        public void SetToolTip (string value) {
            base.ToolTip = value;
        }

        private Action<object> _action;
        public void SetAction (Action<object> value) {
            _action = value;
        }

        protected override void OnButtonClicked (object sender, System.Windows.RoutedEventArgs e) {
            base.OnButtonClicked (sender, e);
            if (_action != null)
                _action (this);
        }

        public void Dispose () {

        }

        public void InsertItem (int index, IToolStripItemBackend backend) {
            Children.Insert (index, (System.Windows.UIElement)backend);
        }

        public void RemoveItem (IToolStripItemBackend backend) {
            Children.Remove ((System.Windows.UIElement)backend);
        }
    }
}