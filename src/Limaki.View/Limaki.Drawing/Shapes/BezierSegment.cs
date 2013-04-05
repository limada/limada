/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Xwt;
using System.Collections.Generic;

namespace Limaki.Drawing.Shapes {

    public class BezierSegment {
        public BezierSegment (Point start, Point cp1, Point cp2, Point end) {
            this.Start = start;
            this.Cp1 = cp1;
            this.Cp2 = cp2;
            this.End = end;
        }

        public Point Start { get; set; }

        public Point Cp1 { get; set; }

        public Point Cp2 { get; set; }

        public Point End { get; set; }

        public IList<Point> ToList () { return new Point[] {Start, Cp1, Cp2, End}; }
    }
}