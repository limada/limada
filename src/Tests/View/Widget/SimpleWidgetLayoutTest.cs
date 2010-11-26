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


using System.Drawing;
using Limaki.Common;
using Limaki.Displays;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Drawing.Shapes;
using NUnit.Framework;
using System.Windows.Forms;
using System;

namespace Limaki.Tests.Widget {
    public class SimpleWidgetLayoutTest:WidgetDisplayTest {
        public SimpleWidgetLayoutTest():base() {}
        public SimpleWidgetLayoutTest(WidgetDisplay display):base(display) {}

        SceneTestBinaryTreeData Data = null;
        public override Scene Scene {
            get {
                if (_scene == null) {
                    base.Scene = new Scene();
                    Data = new SceneTestBinaryTreeData ();
                    Data.Populate(_scene);
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }
    }
}
