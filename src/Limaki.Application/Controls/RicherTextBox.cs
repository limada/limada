/*
 * Limaki
 * Version 0.08
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Winform.Controls {
    public class RicherTextBox : RichTextBox, IZoomTarget {

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return ZoomState.Original; }
            set {
                if (value == ZoomState.Original) {
                    ZoomFactor = 1.0f;
                }
            }
        }

        //public override float ZoomFactor {
        //    get { return base.Window.TextZoom; }
        //    set { base.Window.TextZoom = value; }
        //}

        public void UpdateZoom() {
            //base.Window.TextZoom = ZoomFactor;
        }

        #endregion
    }
}
