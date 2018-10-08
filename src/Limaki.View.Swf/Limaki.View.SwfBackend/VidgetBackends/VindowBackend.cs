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
using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class VindowBackend : VidgetBackend<Form>, IVindowBackend {
        
        public string Title { get => Control.Text; set => Control.Text = value; }
        // TODO:
        public CursorType Cursor { get; set; } = CursorType.Arrow;

        public void SetContent (IVidget value) {
            var backend = value.Backend.ToSwf ();
            if (!Control.Controls.Contains (backend)) {
                backend.Dock = DockStyle.Fill;
                Control.Controls.Add (backend);
            }
        }

        public void Show () {
            Control.Show ();
        }
    }

}