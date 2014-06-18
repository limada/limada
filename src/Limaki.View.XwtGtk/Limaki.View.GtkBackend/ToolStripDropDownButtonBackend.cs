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

using System;
using Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {

    public class ToolStripDropDownButtonBackend : ToolStripItemBackend<ToolStripDropDownButton>, IToolStripDropDownButtonBackend {


        public Vidgets.ToolStripDropDownButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (Vidgets.ToolStripDropDownButton)frontend;
        }

        public bool? IsChecked {
            get { return Widget.IsChecked; }
            set { Widget.IsChecked = value; }
        }

        public void InsertItem (int index, IToolStripItemBackend backend) {
            Widget.Children.Insert (index, backend.ToGtk());
        }

        public void RemoveItem (IToolStripItemBackend backend) {
            Widget.Children.Remove (backend.ToGtk ());
        }
    }
}