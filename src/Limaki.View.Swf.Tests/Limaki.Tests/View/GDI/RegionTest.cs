/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.Drawing;
using Limaki.View.GdiBackend;
using NUnit.Framework;
using System.Diagnostics;

namespace Limaki.Tests.View.GDI {
    public class RegionTest:DomainTest {
        [Test]
        public void TestEmptyRegionRectangle() {
            Region region = new Region ();
            region.MakeInfinite ();

            RectangleF rect = region.GetBounds (GdiUtils.DeviceContext);
            ReportDetail("Rect of empty region is:\t"+rect);
        }
    }
}