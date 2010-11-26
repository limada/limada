/*
 * Limaki 
 * Version 0.081
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


using System;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.UI;
using Limaki.UnitTest;
using Limaki.Widgets.Paint;
using Limaki.Winform.Displays;
using NUnit.Framework;

namespace Limaki.Tests.Display {
    public abstract class DisplayTest<TDisplay,TData>:DomainTest 
        where TDisplay:Control,IDisplayBase
        where TData:class {
        
        private TDisplay _display = default(TDisplay);
        public virtual TDisplay Display {
            get {
                if (_display ==null) {
                    try {
                        _display = Activator.CreateInstance<TDisplay> ();
                    } catch (Exception e) {
                        this.ReportDetail (e.Message);
                        throw e;
                    }
                }
                return _display;
            }
            set { _display = value; }
        }

        public DisplayTest() {}
        public DisplayTest(TDisplay display) {
            this.Display = display;
            this.testForm = display.FindForm();
        }

        public Form testForm = null;
        protected FrameTicker ticker = new FrameTicker();

        public override void  Setup(){
            base.Setup();
            if (testForm == null) {
                try {
                    testForm = new TestForm<TDisplay> (Display);
                    testForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                    testForm.BackColor = System.Drawing.Color.WhiteSmoke;
                    Application.DoEvents ();
                    testForm.Show ();
                    Application.DoEvents ();
                } catch (Exception e) {
                    this.ReportDetail(e.GetType()+"\t"+e.Message);
                    throw e;
                }
                
 	        }
            if ( Display != null )
                ticker.Instrument(Display);
            
        }

        public override void TearDown () {
            if ( Display != null )
                ticker.Disinstrument(Display);
            base.TearDown();
        }
        public int secToTest = 5;


        [Test]
        public virtual void RunSelectorTest() {
            bool zoomEnabled = Display.ZoomAction.Enabled;
            Display.ZoomAction.Enabled= false;
            bool trackerEnabled = Display.ScrollAction.Enabled;
            Display.ScrollAction.Enabled = false;

            SelectionBase selection = Display.SelectAction;
            selection.Enabled = true;
            PointI position = new PointI(0, 0);

            PointI max = new PointI(Display.ClientRectangle.Right, Display.ClientRectangle.Bottom);
            this.ReportDetail("Selector runs to " + max);

            ticker.Start();

            MouseActionEventArgs e = 
                new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None,
                    0, position.X, position.Y, 0);
            selection.OnMouseHover(e);
            selection.OnMouseDown(e);

            // start with 50
            //position = new Point(50, 50);
            //e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            //selection.OnMouseMove(e);
            //Application.DoEvents();

            while ((position.X < max.X) && (position.Y < max.Y)) {
                position.X += 1;
                position.Y += 1;
                e = new MouseActionEventArgs(
                    MouseActionButtons.Left, ModifierKeys.None, 
                    0, position.X, position.Y, 0);
                selection.OnMouseMove(e);
                Application.DoEvents();
            }
            
            ticker.Stop();
            var layer = Display.DataLayer as GDIWidgetLayer;
            var iWidgets = 0;
            if (layer != null) {
                iWidgets =layer.sceneRenderer.iWidgets; 
            }
            this.ReportDetail(
                "Selector-Test" + " \t" +
                ticker.ElapsedInSec() + " sec \t" +
                ticker.FramePerSecond() + " fps \t" +
                iWidgets + " widgets \t"
                );
            ticker.Disinstrument(Display);
            Display.ZoomAction.Enabled = zoomEnabled;
            Display.ScrollAction.Enabled = trackerEnabled;
            Application.DoEvents();

        }

        public virtual void RunFrameTest(Frame frame) {
            this.ReportDetail("Test run for " + secToTest + " seconds " + frame.ToString() + "...");

            ticker.Start();

            RectangleI rect = GDIConverter.Convert( Display.ClientRectangle);
            int div = 2;
            if (frame == Frame.Quarter) {
                div = 4;
            }
            SizeI deflate = new SizeI(rect.Width / div, rect.Height / div);
            rect = new RectangleI(
                new PointI(rect.Location.X + deflate.Width, rect.Location.Y + deflate.Height), deflate);

            int time = (secToTest * 1000) + Environment.TickCount;
            while (ticker.Elapsed > Environment.TickCount) {
                if (frame == Frame.Full) {
                    ((IControl)Display).Invalidate();
                } else {
                    ((IControl)Display).Invalidate(rect);
                }
                ((IControl)Display).Update();
                Application.DoEvents();
            }
            ILayer layer = Display.DataLayer;
            ticker.Stop();
            string fpsResult =
                (ticker.FramePerSecond()) +
                " fps (" + frame.ToString() + "),";
            if (layer is ImageLayer)
                fpsResult += ((ImageLayer)Display.DataLayer).drawMode;
            this.ReportDetail(fpsResult);
        }
    }

    class TestForm<TDisplay>:Form 
        where TDisplay:Control,IDisplayBase{
        private TDisplay _display = default(TDisplay);
        public TDisplay Display {
            get { return _display; }
            set { _display = value; }
        } 
        public TestForm(TDisplay display):base() {
            this.Display = display;
            this.Display.Dock = DockStyle.Fill;
            this.Controls.Add (display);
        }
    }

    public enum Frame {
        Full, Half, Quarter
    }

    public class FrameTicker:Ticker {
        public int FrameCount = 0;
        bool stopped = true;
        public void framePerSecond_Paint ( object sender, PaintEventArgs e ) {
            if (! stopped) 
                FrameCount++;
        }
        public override void Start () {
            FrameCount = 0;
            stopped = false;
            base.Start();
        }
        public override int Stop () {
            stopped = true;
            return base.Stop();
            
        }
        public override void Resume () {
            base.Resume();
            stopped = false;
        }
        public virtual void Instrument(Control control) {
            control.Paint += this.framePerSecond_Paint;
        }
        public virtual void Disinstrument ( Control control ) {
            control.Paint -= this.framePerSecond_Paint;
        }
        public virtual string FramePerSecond () {
            return ( (float)FrameCount / Elapsed  * 1000f ).ToString("#.#0");
        }
    }
}
