/*
 * Limaki 
 * Version 0.07
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
using Limaki.Winform.Displays;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Drawing.Shapes;
using NUnit.Framework;
using System.Windows.Forms;
using System;
using Limaki.Actions;

namespace Limaki.Tests.Widget {
    public class SimpleWidgetLayoutTest : WidgetDisplayTest {
        public SimpleWidgetLayoutTest() : base() { }
        public SimpleWidgetLayoutTest(WidgetDisplay display) : base(display) { }

        ISceneTestData Data = null;
        public override Scene Scene {
            get {
                if (_scene == null) {
                    base.Scene = new Scene();
                    Data = new BinaryTree();
                    Data.Populate(_scene);
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }
    }

    public class LineTextTest : WidgetDisplayTest {
        public LineTextTest() : base() { }
        public LineTextTest(WidgetDisplay display) : base(display) { }
        SimpleGraph Data = null;
        public override Scene Scene {
            get {
                if (_scene == null) {
                    Data = new SimpleGraph();
                    base.Scene = Data.Scene;
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }
        WidgetBoundsLayer widgetBoundsLayer = null;
        public override void Setup() {
            base.Setup();
            if (!Display.EventControler.Actions.ContainsKey(typeof(WidgetBoundsLayer))) {
                widgetBoundsLayer = new WidgetBoundsLayer(Display, Display);
                Display.EventControler.Add (widgetBoundsLayer);
            } else {
                widgetBoundsLayer = Display.EventControler.GetAction<WidgetBoundsLayer> ();
            }
            widgetBoundsLayer.Data = Display.Data;
            widgetBoundsLayer.Layout = (ILayout<Scene, IWidget>)Display.LayoutControler.Layout;
            
        }
        public override void TearDown() {
            base.TearDown();
            //Display.EventControler.Remove (widgetBoundsLayer);
        }
        [Test]
        public void LineTextHover() {
            this.ReportDetail ("***** LineTextHover start");
            Display.ZoomAction.ZoomIn();
            Display.ZoomAction.ZoomIn();
            Display.UpdateZoom();
            NeutralPosition();
            this.ReportDetail ("***** Zoomend and Neutral");
            Point lineCenter = Data.Link1.Shape[Anchor.Center];
            lineCenter = Display.DataLayer.Camera.FromSource (lineCenter);
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Left, 0, lineCenter.X, lineCenter.Y, 0);
            //Display.EventControler.OnMouseDown(e);
            //Application.DoEvents();
            this.ReportDetail("***** MouseMove");
            Display.EventControler.OnMouseMove(e);
            Display.Invalidate ();
            Application.DoEvents();
            //Display.EventControler.OnMouseUp(e);
            //Application.DoEvents();
            //NeutralPosition();
            
        }

        void Display_Paint(object sender, PaintEventArgs e) {
            ILayout<Scene, IWidget> layout = (ILayout<Scene, IWidget>) Display.LayoutControler.Layout;
            Point[] hull = layout.GetDataHull (Data.Link1, Display.DataLayer.Camera.Matrice, 0, true);

        }
    }

}
