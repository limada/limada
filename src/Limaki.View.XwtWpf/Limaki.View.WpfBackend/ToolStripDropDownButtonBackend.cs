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

    public class ToolStripDropDownButtonBackend : ToolStripItemBackend<ToolStripDropDownButton>, IToolStripDropDownButtonBackend {

        public LVV.ToolStripDropDownButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolStripDropDownButton)frontend;
        }

        public override void Compose () {
            base.Compose ();
            Control.Click += OnButtonClicked;
        }

        public void SetImage (Xwt.Drawing.Image value) {
            Control.Image = value;
        }

        public void SetLabel (string value) {
            Control.Label = value;
        }


        protected virtual void OnButtonClicked (object sender, EventArgs e) {
            if (_action != null)
                _action (this);
        }

        public void InsertItem (int index, IToolStripItemBackend backend) {
            Control.Children.Insert (index, backend.ToWpf());
        }

        public void RemoveItem (IToolStripItemBackend backend) {
            Control.Children.Remove (backend.ToWpf());
        }
    }
}