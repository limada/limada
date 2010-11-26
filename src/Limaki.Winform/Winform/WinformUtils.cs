/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System.Windows.Forms;
using Limaki.Drawing.UI;
using Limaki.Drawing;
using Limaki.Drawing.GDI;

namespace Limaki.Winform {
    public class WinformUtils {
        public bool IsSameApp(IControl a, IControl b) {
            Control control = a as Control;
            Control thisControl = b as Control;
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