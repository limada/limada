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

using System.Collections.Generic;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtSystemInformation : IUISystemInformation {

        public Size DragSize {
            get { return new Size(1,1); }
        }

        public int DoubleClickTime {
            get { return 12; }
        }

        public int VerticalScrollBarWidth {
            get { return 10; }
        }

        public int HorizontalScrollBarHeight {
            get { return 10; }
        }

        public IEnumerable<string> ImageFormats {
            get {
                yield break;
            }
        }
    }
}
