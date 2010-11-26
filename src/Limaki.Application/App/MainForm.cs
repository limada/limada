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
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Tests.Drawing;
using Limaki.Tests.Display;
using Limaki.Tests.Widget;
using Limaki.Winform;
using Limaki.Winform.Displays;
using Limaki.Widgets;
using Limaki.Tests.Graph.Model;
namespace Limaki.App {
    public partial class MainForm : Form {
        public MainForm() {
            InitializeComponent();

            ISceneFactory factory = new SceneFactory<LimakiShortHelpFactory> ();
            display.Data = factory.Scene;
            
            display.Data.Focused = factory.Node[2];
            //display.WidgetFolding.folder.CollapseToFocused ();
            
          
            display.CommandsInvoke();
            display.Invalidate();

            display.ZoomState = ZoomState.Original;
            display.SelectAction.Enabled = true;
            display.ScrollAction.Enabled = false;
            display.BackColor = SystemColors.Window;

            # region tests

            bool showBounds = false;
            bool runThreadTest = false;
            bool showSandbox = false;
            if (showBounds) {
                WidgetBoundsLayer widgetBoundsLayer = new WidgetBoundsLayer(display, display);
                widgetBoundsLayer.Data = display.Data;
                display.EventControler.Add(widgetBoundsLayer);
            }
            if (showSandbox) {
                RegionSandbox regionSandbox = new RegionSandbox(display, display);
                display.EventControler.Add(regionSandbox);
            }
            if (runThreadTest) {
                SceneThreadTest threadTest = new SceneThreadTest(display.Data);
                threadTest.Run();
            }

            #endregion

            //openFileDialog.InitialDirectory = Environment.GetFolderPath (Environment.SpecialFolder.MyPictures);
            this.selectButton.Checked = display.SelectAction.Enabled;
            this.moveButton.Checked = display.ScrollAction.Enabled;

        }
        bool showPosition = false;
        private void display_MouseMove(object sender, MouseEventArgs e) {
            if (showPosition)
                this.mousePosLabel.Text = new Point(e.X, e.Y).ToString();
        }

        private void display_Paint(object sender, PaintEventArgs e) {
            if (showPosition)
                this.scrollPosLabel.Text = display.AutoScrollPosition.ToString();
        }

        List<ToolStripButton> _toolsGroup = null;
        List<ToolStripButton> toolsGroup {
            get {
                if (_toolsGroup == null) {
                    _toolsGroup = new List<ToolStripButton>();
                    _toolsGroup.Add(selectButton);
                    _toolsGroup.Add(moveButton);
                    _toolsGroup.Add(connectorButton);
                    _toolsGroup.Add(AddWidgetButton);
                }
                return _toolsGroup;
            }
        }
        void activateToolInGroup(object sender) {
            bool activated = false;
            foreach (ToolStripButton button in toolsGroup) {
                if (sender == button) {
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
            activateToolInGroup(sender);

            display.SelectAction.Enabled = this.selectButton.Checked;
            display.ScrollAction.Enabled = this.moveButton.Checked;

            display.AddEdgeAction.Enabled = this.connectorButton.Checked;
            display.AddWidgetAction.Enabled = this.AddWidgetButton.Checked;

            display.WidgetChanger.Enabled = !display.AddWidgetAction.Enabled;
            display.EdgeWidgetChanger.Enabled = !display.AddEdgeAction.Enabled;


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
                display.UpdateZoom();
                foreach (ToolStripMenuItem item in zoomButton.DropDownItems) {
                    if (menuItem != item) {
                        item.Checked = false;
                    }
                }
            }
        }

        private void zoomButton_ButtonClick(object sender, EventArgs e) {
            if (display.ActiveControl == null) {
                display.ZoomAction.ZoomIn();
                display.UpdateZoom();
            }
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Application.Exit();
        }


        private void fileOpenMenuItem_Click(object sender, EventArgs e) {

            OpenExampleData dialog = new OpenExampleData();
            if (dialog.ShowDialog(this) == DialogResult.OK) {
                OpenExampleData.ITypeChoose testData = dialog.examples[dialog.comboBox1.SelectedIndex];
                testData.Data.Count = (int)dialog.numericUpDown1.Value;
                display.Data = testData.Data.Scene;
            }
            dialog.Dispose();

        }

        private void testMessage(object sender, string message) {
            this.scrollPosLabel.Text = message;
            Application.DoEvents();
        }


        Form about = null;
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (about == null) {
                about = new About();
            }
            about.ShowDialog();
        }

        private void selectorTestToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                display.Paint -= display_Paint;
                display.MouseMove -= display_MouseMove;

                WidgetDisplayTest test = new BenchmarkOneTests(this.display);

                test.WriteDetail += this.testMessage;
                test.Setup();
                test.RunSelectorTest();
                test.TearDown();
            } finally {
                display.Paint += display_Paint;
                display.MouseMove += display_MouseMove;
            }
        }

        private void otherTestToolStripMenuItem_Click(object sender, EventArgs e) {

            try {
                display.Paint -= display_Paint;
                display.MouseMove -= display_MouseMove;
                BenchmarkOneTests test = new BenchmarkOneTests(this.display);
                test.Scene = null;
                test.Setup();
                test.WriteDetail += this.testMessage;
                test.MoveAlongSceneBoundsTest();
                test.TearDown();
            } finally {
                display.Paint += display_Paint;
                display.MouseMove += display_MouseMove;
            }

        }

        private void LayoutButton_Click(object sender, EventArgs e) {
            display.CommandsInvoke();
            display.Invalidate();
        }

        Rectangle ControlSize(Control control) {
            Rectangle result = Rectangle.Empty;
            foreach (Control c in control.Controls) {
                result = Rectangle.Union(result, c.Bounds);
            }
            return result;
        }

        Options options = null;
        private void layoutToolStripMenuItem_ClickOld(object sender, EventArgs e) {

            options = new Options();
            LayoutOptions layoutOptions = new LayoutOptions();
            layoutOptions.SceneLayout = ((WidgetKit)display.displayKit).Layout;
            options.applyButton.Click += new EventHandler(LayoutButton_Click);
            options.closeButton.Click += new EventHandler(LayoutButton_Click);
            options.SuspendLayout();
            options.OptionChanger = layoutOptions;
            options.Controls.Add(layoutOptions);
            layoutOptions.Dock = DockStyle.Fill;
            Rectangle optionsBounds = ControlSize(options);
            optionsBounds = Rectangle.Union(optionsBounds, ControlSize(layoutOptions));
            options.ClientSize = optionsBounds.Size;
            options.ResumeLayout(true);
            options.Show();

        }

        private void layoutToolStripMenuItem_Click(object sender, EventArgs e) {
            LayoutProperties layoutOptions = new LayoutProperties();
            layoutOptions.SceneLayout = ((WidgetKit)display.displayKit).Layout;
            showPropertyWindow(layoutOptions);
        }

        private void showPropertyWindow(object optionObject) {

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

        }
        void grid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
            display.CommandsInvoke();
            display.Invalidate();
        }

        private void styleToolStripMenuItem_Click(object sender, EventArgs e) {
            Limaki.Drawing.IStyle[] styles = new Limaki.Drawing.IStyle[2];
            styles[0] = ((WidgetKit)display.displayKit).StyleSheet[Limaki.Drawing.StyleNames.DefaultStyle];
            styles[1] = ((WidgetKit)display.displayKit).StyleSheet[Limaki.Drawing.StyleNames.EdgeStyle];
            showPropertyWindow(styles);
        }

        private void showQuadTreeToolStripMenuItem_Click(object sender, EventArgs e) {
            QuadTreeVisualizer vis = new QuadTreeVisualizer();
            if (this.display.Data.SpatialIndex is QuadTreeIndex)
                vis.Data = ((QuadTreeIndex)this.display.Data.SpatialIndex).GeoIndex;
            vis.Show();
        }



        private void currentProblemToolStripMenuItem_Click(object sender, EventArgs e) {

            try {
                display.Paint -= display_Paint;
                display.MouseMove -= display_MouseMove;


            } finally {
                display.Paint += display_Paint;
                display.MouseMove += display_MouseMove;

            }


        }
    }
}
