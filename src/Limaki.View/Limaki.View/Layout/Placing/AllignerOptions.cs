/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Drawing.Shapes;
using Xwt;
using Limaki.Drawing;
using System;

namespace Limaki.View.Layout {

    public class AlignerOptions {
        public Alignment AlignX { get; set; }
        public Alignment AlignY { get; set; }
        public PointOrder PointOrder { get; set; }
        public Dimension Dimension { get; set; }
        public Size Distance{ get; set; }
        public Collissions Collissions { get; set; }
    }

    public enum Dimension {
        X,Y
    }

    [Flags]
    public enum Collissions {
        None = 0x1 << 0,
        NextFree = 0x1 << 1,
        Toggle = 0x1 << 2,
        PerColumn = 0x1 << 3,
    }
}