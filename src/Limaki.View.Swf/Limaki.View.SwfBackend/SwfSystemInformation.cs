/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Collections.Generic;
using System.Windows.Forms;
using Limaki.View;
using Xwt;
using Xwt.GdiBackend;

namespace Limaki.View.SwfBackend {

    public class SwfSystemInformation : IUISystemInformation {
        public Size DragSize {
            get {
                return SystemInformation.DragSize.ToXwt ();
            }
        }
        public int DoubleClickTime {
            get {
                return SystemInformation.DoubleClickTime;
            }
        }
        public int VerticalScrollBarWidth {
            get {
                return SystemInformation.VerticalScrollBarWidth;
            }
        }
        public int HorizontalScrollBarHeight {
            get {
                return SystemInformation.HorizontalScrollBarHeight;
            }
        }

        public IEnumerable<string> ImageFormats {
            get {
                yield break;
            }
        }
    }

}