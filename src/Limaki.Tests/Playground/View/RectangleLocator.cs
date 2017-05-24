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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Tests;
using Limaki.Tests.View;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz.Modelling;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend.Viz;
using NUnit.Framework;
using Xwt;
using Xwt.Drawing;
using Xwt.Html5.Backend;

namespace Limaki.Playground.View {
    public class RectangleLocator : ILocator<Rectangle> {

        public Point GetLocation (Rectangle item) => item.Location;
        public Size GetSize (Rectangle item) => item.Size;
        public bool HasLocation (Rectangle item) => true;
        public bool HasSize (Rectangle item) => true;
        public void SetLocation (Rectangle item, Point location) => item.Location = location;
        public void SetSize (Rectangle item, Size value) => item.Size = value;
    }
    
}