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

    public class ToolbarItemHostBackend : ToolbarItemBackend<Gtk.ToolItem>, IToolbarItemHostBackend {

        public new Vidgets.ToolbarItemHost Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (Vidgets.ToolbarItemHost)frontend;
        }

        public void SetChild (Vidget value) {
            var child = value.Backend.ToGtk ();
            if (child != null) {
                if (Widget.Child != child) {
					if(Widget.Child != null)
                    	Widget.Remove (Widget.Child);
					child.Show ();
                    Widget.Child = child;
                }
            }
        }
    }
}