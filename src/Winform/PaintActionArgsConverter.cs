/*
 * Limaki 
 * Version 0.064
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Limaki.Actions;
using System.Drawing;

namespace Limaki.Winform {
    /// <summary>
    /// Converts Windows.Forms.PaintEventArgs and Limaki.PaintActionEventArgs
    /// </summary>
    public class Converter {
        public static PaintActionEventArgs Convert(PaintEventArgs e) {
            return new PaintActionEventArgs(e.Graphics, e.ClipRectangle);
        }
        public static PaintActionEventArgs Convert(PaintEventArgs e, Region clipRegion) {
            return new PaintActionEventArgs(e.Graphics, e.ClipRectangle,clipRegion);
        }
        public static PaintEventArgs Convert(PaintActionEventArgs e) {
            return new PaintEventArgs(e.Graphics, e.ClipRectangle);
        }
            

    }
}
