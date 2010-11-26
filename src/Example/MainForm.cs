/*
 * Limaki 
 * Version 0.063
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Tests.Display;
using Limaki.Tests.Widget;
using Limaki.Winform;
using Limaki.Displays;
using Limaki.Widgets;

namespace Limaki.Examples {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();
#if Widget


#endif
            string testFile = "..\\..\\..\\..\\TestData\\TestDeleteBeforeRelease.tif";
            if (System.IO.File.Exists(testFile)) {
#if Widget
             
#else
                Image image = Image.FromFile (testFile);
                display.Data = image;
#endif
                display.SelectAction.Enabled=true;
                display.ScrollAction.Enabled = false;
            } else {
                display.SelectAction.Enabled = false;
                display.ScrollAction.Enabled = true;
            }
#if Widget

            display.Data = new ProgammingLanguageTestData().Scene;

            display.ZoomState = ZoomState.Original;
            display.SelectAction.Enabled = false;
            display.ScrollAction.Enabled = false;
            display.BackColor = SystemColors.Window;

            # region tests
            
            bool showBounds = false;
            bool runThreadTest = false;
            bool showSandbox = false;
            if (showBounds) {
                WidgetBoundsLayer widgetBoundsLayer = new WidgetBoundsLayer (display, display);
                widgetBoundsLayer.Data = display.Data;
                display.ActionDispatcher.Add (widgetBoundsLayer);
            }

            if (runThreadTest) {
                SceneThreadTest threadTest = new SceneThreadTest (display.Data);
                threadTest.Run ();
            }

            #endregion 
#endif
            openFileDialog.InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyPictures);
            this.drawModeComboBox.Items.AddRange(Enum.GetNames(typeof(ImageLayer.DrawMode)));
            this.drawModeComboBox.SelectedIndex = 0;
            this.selectButton.Checked = display.SelectAction.Enabled;
            this.moveButton.Checked = display.ScrollAction.Enabled;

        }
        bool showPosition = false;
        private void display_MouseMove(object sender, MouseEventArgs e) {
            if (showPosition)
                this.mousePosLabel.Text = new Point (e.X, e.Y).ToString ();
        }

        private void display_Paint(object sender, PaintEventArgs e) {
            if(showPosition)
                this.scrollPosLabel.Text = display.AutoScrollPosition.ToString();
        }

        List<ToolStripButton> _toolsGroup = null;
        List<ToolStripButton> toolsGroup {
            get {
                if (_toolsGroup==null) {
                    _toolsGroup = new List<ToolStripButton> ();
                    _toolsGroup.Add (selectButton);
                    _toolsGroup.Add (moveButton);
                    _toolsGroup.Add (connectorButton);
                    _toolsGroup.Add (AddWidgetButton);
                }
                return _toolsGroup;
            }
        }
        void activateToolInGroup(object sender) {
            bool activated = false;
            foreach(ToolStripButton button in toolsGroup ) {
                if (sender==button) {
                    button.Checked = !button.Checked;
                    activated = button.Checked;
                } else {
                    button.Checked = false;
                }
            }
            if (!activated) {
                //moveButton.Checked = true;
            }
        }
        private void selectOrMoveButton_Click(object sender, EventArgs e) {
            // checkOnClick is false cause of mono bug so we have to toggle ourself
            activateToolInGroup (sender);

            display.SelectAction.Enabled = this.selectButton.Checked;
            display.ScrollAction.Enabled = this.moveButton.Checked;
#if Widget
            display.AddLinkAction.Enabled = this.connectorButton.Checked;
            display.AddWidgetAction.Enabled = this.AddWidgetButton.Checked;

            display.WidgetChanger.Enabled = !display.AddWidgetAction.Enabled;
            display.LinkWidgetChanger.Enabled = !display.AddLinkAction.Enabled;
#endif            
            
        }

        private void zoomButton_DropDownItemClicked(object sender, EventArgs e) {
            // is some editor in action?? then do nothing
            if (display.ActiveControl == null) {
                ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
                if (menuItem == this.zoomMenuFittoHeigth) {
                    display.ZoomState = ZoomState.FitToHeigth;
                } else if (menuItem == this.zoomMenuFitToScreen) {
                    display.ZoomState = ZoomState.FitToScreen;
                } else if (menuItem == this.zoomMenuFittoWidth) {
                    display.ZoomState = ZoomState.FitToWidth;
                } else if (menuItem == this.zoomMenuOriginalSize) {
                    display.ZoomState = ZoomState.Original;
                }
                display.UpdateZoom ();
                foreach (ToolStripMenuItem item in zoomButton.DropDownItems) {
                    if (menuItem != item) {
                        item.Checked = false;
                    }
                }
            }
        }

        private void zoomButton_ButtonClick(object sender, EventArgs e) {
            if (display.ActiveControl == null) {
                display.ZoomAction.ZoomIn ();
                display.UpdateZoom ();
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit ();
        }

        
        private void fileOpenMenuItem_Click(object sender, EventArgs e) {
#if Widget
            OpenExampleData dialog = new OpenExampleData ();
            if (dialog.ShowDialog(this) == DialogResult.OK) {
                OpenExampleData.ITypeChoose testData = dialog.algos[dialog.comboBox1.SelectedIndex];
                testData.Data.Count = (int)dialog.numericUpDown1.Value;
                display.Data = testData.Data.Scene;
            }
            dialog.Dispose ();
#else       
            if (this.openFileDialog.ShowDialog(this)==DialogResult.OK) {
                try {

                    

                    display.Data = Image.FromFile(openFileDialog.FileName);
                    display.ZoomState = ZoomState.FitToScreen;
                    display.UpdateZoom ();
                } catch {
                    MessageBox.Show("Image load failed");

                }   
            }
#endif
        }

        private void testMessage(object sender, string message) {
            this.scrollPosLabel.Text = message;
            Application.DoEvents();
        }


        private void framePerSecondToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem control = sender as ToolStripMenuItem;
#if Widget
#else
            display.GetAction<ImageLayer> ().drawMode = 
                (ImageLayer.DrawMode) Enum.Parse (typeof(ImageLayer.DrawMode), 
                this.drawModeComboBox.SelectedItem.ToString ());
#endif
            
            if (control != null) {
                Frame frame = Frame.Full;
                if (control.Text.Contains("Half")) {
                    frame = Frame.Half;
                } else if (control.Text.Contains("Quarter")) {
                    frame = Frame.Quarter;
                }
                try {
                    display.Paint -= display_Paint;
                    display.MouseMove -= display_MouseMove;
#if Widget
                    WidgetDisplayTest test = new WidgetDisplayTest(this.display);
#else
                    ImageDisplayTest test = new ImageDisplayTest(this.display);
#endif
                    test.WriteMessage += this.testMessage;
                    
                    test.Setup();
                    test.RunFrameTest(frame);
                    test.TearDown();
                } finally {
                    display.Paint += display_Paint;
                    display.MouseMove += display_MouseMove;
                }
            }
        }

        Form about = null;
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (about == null) {
                about = new About ();
            }
            about.ShowDialog();
        }

        private void selectorTestToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                display.Paint -= display_Paint;
                display.MouseMove -= display_MouseMove;
#if Widget
                WidgetDisplayTest test = new WidgetDisplayTest(this.display);
#else
                ImageDisplayTest test = new ImageDisplayTest(this.display);
#endif
                test.WriteMessage += this.testMessage;
                test.Setup();
                test.RunSelectorTest();
                test.TearDown();
            } finally {
                display.Paint += display_Paint;
                display.MouseMove += display_MouseMove;
            }
        }

        private void otherTestToolStripMenuItem_Click ( object sender, EventArgs e ) {
#if Widget
            try {
                display.Paint -= display_Paint;
                display.MouseMove -= display_MouseMove;
                LongTermPerformanceTests test = new LongTermPerformanceTests(this.display);
                test.Scene = null;
                test.Setup();
                test.WriteMessage += this.testMessage;
                test.MoveAlongSceneBoundsTest();
                test.TearDown();
            } finally {
                display.Paint += display_Paint;
                display.MouseMove += display_MouseMove;
            }
#endif
        }

        private void LayoutButton_Click(object sender, EventArgs e) {
            display.CommandsInvoke ();
            display.Invalidate ();
        }

        Rectangle ControlSize(Control control) {
            Rectangle result = Rectangle.Empty;
            foreach(Control c in control.Controls) {
                result = Rectangle.Union (result, c.Bounds);
            }
            return result;
        }

        Options options = null;
        private void layoutToolStripMenuItem_ClickOld(object sender, EventArgs e) {
#if Widget
            options = new Options ();
            LayoutOptions layoutOptions = new LayoutOptions ();
            layoutOptions.SceneLayout = ((WidgetKit)display.displayKit).Layout;
            options.applyButton.Click += new EventHandler(LayoutButton_Click);
            options.closeButton.Click += new EventHandler(LayoutButton_Click);
            options.SuspendLayout();
            options.OptionChanger = layoutOptions;
            options.Controls.Add (layoutOptions);
            layoutOptions.Dock = DockStyle.Fill;
            Rectangle optionsBounds = ControlSize (options);
            optionsBounds = Rectangle.Union(optionsBounds,ControlSize(layoutOptions));
            options.ClientSize = optionsBounds.Size;
            options.ResumeLayout (true);
            options.Show ();
#endif
        }

        private void layoutToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutProperties layoutOptions = new LayoutProperties();
            layoutOptions.SceneLayout = ((WidgetKit)display.displayKit).Layout;
            showPropertyWindow (layoutOptions);
        }

        private void showPropertyWindow(object optionObject) {
#if Widget
            options = new Options();
            options.applyButton.Click += new EventHandler(LayoutButton_Click);
            options.closeButton.Click += new EventHandler(LayoutButton_Click);
            options.SuspendLayout();
            PropertyGrid grid = new PropertyGrid();
            grid.PropertyValueChanged += new PropertyValueChangedEventHandler(grid_PropertyValueChanged);
            options.OptionChanger = grid;
            options.Controls.Add(grid);
            if (optionObject.GetType().IsArray) {
                grid.SelectedObjects = (object[])optionObject;
            } else {
                grid.SelectedObject = optionObject;
            }
            grid.Dock = DockStyle.Fill;
            Rectangle optionsBounds = ControlSize(options);
            options.ClientSize = optionsBounds.Size;
            options.ResumeLayout(true);
            options.Show();
#endif            
        }
        void grid_PropertyValueChanged ( object s, PropertyValueChangedEventArgs e ) {
            display.CommandsInvoke();
            display.Invalidate();
        }

        private void styleToolStripMenuItem_Click(object sender, EventArgs e) {
            Limaki.Drawing.IStyle[] styles = new Limaki.Drawing.IStyle[2];
            styles[0] = ( (WidgetKit) display.displayKit ).StyleSheet[Limaki.Drawing.StyleNames.DefaultStyle];
            styles[1] = ((WidgetKit)display.displayKit).StyleSheet[Limaki.Drawing.StyleNames.LinkStyle];
            showPropertyWindow(styles);
        }
    }
}
