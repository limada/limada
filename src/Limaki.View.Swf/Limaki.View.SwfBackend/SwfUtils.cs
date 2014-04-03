/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Windows.Forms;
using Limaki.View;

namespace Limaki.View.SwfBackend {

    public class SwfUtils {

        public void SetFont(System.Drawing.Font font, Control control) {
            control.Font = font;
            foreach (Control child in control.Controls) {
                SetFont(font, child);
            }
        }

        public bool IsSameApp(IVidgetBackend a, IVidgetBackend b) {
            var control = a as Control;
            var thisControl = b as Control;
            if (control != null) {
                Form form = control.FindForm();
                bool sameApp = false;
                foreach (Form iForm in Application.OpenForms) {
                    if (iForm == form) {
                        sameApp = true;
                        break;
                    }
                }
                return sameApp;
            }
            return false;
        }

     }
}