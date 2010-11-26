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
 * 
 */


using System.Drawing;
using Limaki.Drawing.GDI;
using NUnit.Framework;

namespace Limaki.Tests.View.Drawing {
    
    public class RegionTest:DomainTest {
        [Test]
        public void TestEmptyRegionRectangle() {
            Region region = new Region ();
            region.MakeInfinite ();

            RectangleF rect = region.GetBounds (GDIUtils.DeviceContext);
            ReportDetail("Rect of empty region is:\t"+rect);
        }
    }
}
