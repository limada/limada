/*
 * Limada 
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
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using Limada.App;
using Limada.View;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.GDI.UI;
using Limaki.Model.Streams;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using Limaki.Widgets.Paint;
using Limaki.Winform.Controls;
using Limaki.Winform.Displays;

namespace Limaki.App {
    public partial class MainForm : Form {
        private string FormText = "limada::concept";

        public MainForm() {
            InitializeComponent();

            saveFileDialog.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();

            Display1.BackColor = SystemColors.Window;
			Display2.BackColor = Display1.BackColor;
			
            InitializePostProcess();

            Display1.ZoomState = ZoomState.Original;
            Display1.SelectAction.Enabled = true;
            Display1.ScrollAction.Enabled = false;


            this.selectButton.Checked = Display1.SelectAction.Enabled;
            this.moveButton.Checked = Display1.ScrollAction.Enabled;

            initializeDisplay(Display1);
            initializeDisplay(Display2);

            CurrentDisplay = Display1;

            // this is for mono on linux:
            splitContainer.ActiveControl = Display1;

        }

        void InitializePostProcess() {
            InitDefaultStyles();
            SetLayoutTools();
            InitializeToolstripPositions();
            ShowTiledGraphDocumentTool = true;
        }

        void initializeDisplay(WidgetDisplay display) {
            display.Enter -= setCurrentDisplay;
            display.MouseUp -= setCurrentDisplay;
            display.Enter += setCurrentDisplay;
            display.MouseUp += setCurrentDisplay;

            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            IStyleSheet styleSheet = null;

            if (styleSheets.TryGetValue(display.DisplayKit.StyleSheet.Name, out styleSheet)) {
                display.DisplayKit.StyleSheet = styleSheet;
            } else {
                styleSheets.Add(display.DisplayKit.StyleSheet.Name, display.DisplayKit.StyleSheet);
            }

            display.SceneFocusChanged -= display_FocusChanged;
            display.SceneFocusChanged += display_FocusChanged;

        }


        private Control _currentControl = null;
        public Control CurrentControl {
            get { return _currentControl; }
            set { _currentControl = value; }
        }

        private WidgetDisplay _currentDisplay = null;
        public WidgetDisplay CurrentDisplay {
            get {
                return _currentDisplay;
            }
            set {
                if (_currentDisplay != value && value != null) {
                    SetMarkerTools(value);
                    setWidgetTools(value);
                    SetStyleToolBar(value);
                    SetLayoutTools(value);
                }
                _currentDisplay = value;
                CurrentControl = value;
            }
        }

        void setCurrentDisplay(object sender, EventArgs e) {
            CurrentDisplay = sender as WidgetDisplay;
        }

        void setCurrentControl(object sender, EventArgs e) {
            CurrentControl = sender as Control;
        }

        #region Marker-Handling
        // see also: DataPostProcess


        void SetMarkerTools(WidgetDisplay display) {
            markerCombo.Items.Clear();
            bool makeVisible = display.Data != null && display.Data.Markers != null;
            if (makeVisible) {
                markerCombo.Items.AddRange(display.Data.Markers.MarkersAsStrings());
            }
            markerStrip.Visible = makeVisible;
        }

        private void markerCombo_SelectedIndexChanged(object sender, EventArgs e) {
            string marker = markerCombo.SelectedItem.ToString();
            Scene scene = CurrentDisplay.Data;
            if (scene.Markers != null) {
                SceneTools.ChangeMarkers(scene, scene.Selected.Elements, marker);
            }
            CurrentDisplay.CommandsExecute();

        }
        #endregion

        #region WidgetToolBar
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

        private void setWidgetTools(WidgetDisplay display) {
            this.selectButton.Checked = display.SelectAction.Enabled;
            this.moveButton.Checked = display.ScrollAction.Enabled;

            this.connectorButton.Checked = display.AddEdgeAction.Enabled;
            this.AddWidgetButton.Checked = display.AddWidgetAction.Enabled;
        }

        private void selectOrMoveButton_Click(object sender, EventArgs e) {
            // checkOnClick is false cause of mono bug so we have to toggle ourself
            activateToolInGroup(sender);

            CurrentDisplay.SelectAction.Enabled = this.selectButton.Checked;
            CurrentDisplay.ScrollAction.Enabled = this.moveButton.Checked;

            CurrentDisplay.AddEdgeAction.Enabled = this.connectorButton.Checked;
            CurrentDisplay.AddWidgetAction.Enabled = this.AddWidgetButton.Checked;

            CurrentDisplay.WidgetChanger.Enabled = !CurrentDisplay.AddWidgetAction.Enabled;
            CurrentDisplay.EdgeWidgetChanger.Enabled = !CurrentDisplay.AddEdgeAction.Enabled;


        }

        private void zoomButton_DropDownItemClicked(object sender, EventArgs e) {
            // is some editor in action?? then do nothing
            if (CurrentDisplay.ActiveControl == null) {
                ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
                if (menuItem == this.zoomMenuFittoHeigth) {
                    CurrentDisplay.ZoomState = ZoomState.FitToHeigth;
                } else if (menuItem == this.zoomMenuFitToScreen) {
                    CurrentDisplay.ZoomState = ZoomState.FitToScreen;
                } else if (menuItem == this.zoomMenuFittoWidth) {
                    CurrentDisplay.ZoomState = ZoomState.FitToWidth;
                } else if (menuItem == this.zoomMenuOriginalSize) {
                    CurrentDisplay.ZoomState = ZoomState.Original;
                }
                CurrentDisplay.UpdateZoom();
                foreach (ToolStripMenuItem item in zoomButton.DropDownItems) {
                    if (menuItem != item) {
                        item.Checked = false;
                    }
                }
            }
        }

        private void zoomButton_MouseDown(object sender, MouseEventArgs e) {
            if (!zoomButton.DropDownButtonPressed) {
                if (CurrentControl == CurrentDisplay && CurrentDisplay != null) {
                    if (CurrentDisplay.ActiveControl == null) {
                        if (e.Button == MouseButtons.Left)
                            CurrentDisplay.ZoomAction.ZoomIn();
                        else if (e.Button == MouseButtons.Right)
                            CurrentDisplay.ZoomAction.ZoomOut();
                        CurrentDisplay.UpdateZoom();
                    }
                } else if (CurrentControl is IZoomTarget) {
                    IZoomTarget zoomTarget = CurrentControl as IZoomTarget;
                    if (e.Button == MouseButtons.Left)
                        zoomTarget.ZoomFactor = zoomTarget.ZoomFactor * 1.1f;
                    else if (e.Button == MouseButtons.Right)
                        zoomTarget.ZoomFactor = zoomTarget.ZoomFactor / 1.1f;
                }
            }
        }

        private void LayoutButton_Click(object sender, EventArgs e) {
            CurrentDisplay.CommandsInvoke();
            CurrentDisplay.Invalidate();
        }



        #endregion

        #region Menu - File
        private void ApplicationExit_Click(object sender, EventArgs e) {
            dataBaseHandler.Close();
            Application.Exit();
        }

        IDataBaseHandler _dataBaseHandler = null;
        public IDataBaseHandler dataBaseHandler {
            get {
                if (_dataBaseHandler == null) {
                    _dataBaseHandler = Registry.Factory.One<IDataBaseHandler>();
                    _dataBaseHandler.Display = this.Display1;
                }
                return _dataBaseHandler;
            }
        }



        public void DataPostProcess(string dataName) {

            ClearHistory();

            Registry.ApplyProperties<MarkerContextProcessor, Scene>(Display1.Data);

            new WiredDisplays().MakeSideDisplay(this.Display1, this.Display2);

            Registry.ApplyProperties<MarkerContextProcessor, Scene>(Display2.Data);

            this.Text = dataName + " - " + this.FormText;

            CurrentDisplay = Display1;


        }

        private void OpenFile_Click(object sender, EventArgs e) {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK) {
                CurrentDisplay = null;
                this.Display2.Data = null;

                dataBaseHandler.Open(openFileDialog.FileName);
                DataPostProcess(DataBaseInfo.FromFileName(openFileDialog.FileName).Name);
            }
        }

        private void SaveFile_Click(object sender, EventArgs e) {
            dataBaseHandler.Save();
        }

        #endregion

        #region Menu - About
        Form about = null;
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (about == null) {
                about = new About();
            }
            about.ShowDialog();
        }

        #endregion

        #region Menu - Format

        Rectangle ControlSize(Control control) {
            Rectangle result = Rectangle.Empty;
            foreach (Control c in control.Controls) {
                result.Location = new Point(
                    Math.Min(c.Left, result.Location.X),
                    Math.Min(c.Top, result.Location.Y));
                result.Size = new Size(
                    Math.Max(c.Left + c.Width, result.Width),
                    c.Height + result.Height
                    );

            }
            return result;
        }

        Options options = null;
        private void layoutToolStripMenuItem_Click(object sender, EventArgs e) {
            options = new Options();
            options.applyButton.Click += new EventHandler(LayoutButton_Click);

            var grid = new LayoutEditor();
            grid.Dock = DockStyle.Fill;
            grid.SelectedObject = (WidgetLayout<Scene, IWidget>)CurrentDisplay.DataLayout;

            
            options.SuspendLayout();
            options.Controls.Remove (options.OptionList);

            options.OptionChanger = grid;
            options.ContentPanel.Size = grid.Size;
            options.ContentPanel.Controls.Add(grid);
            
            options.ClientSize = ControlSize(options).Size;
            options.ResumeLayout(true);
            Application.DoEvents ();
            options.ClientSize = ControlSize(options).Size;
            Application.DoEvents();

            options.Show();
        }



        

        private void styleToolStripMenuItem_Click(object sender, EventArgs e) {
            showStylesPropertyWindow(CurrentDisplay.DisplayKit.StyleSheet.Styles);
        }

        private void showStylesPropertyWindow(ICollection<IStyle> optionObject) {
            options = new Options();
            options.applyButton.Click += new EventHandler(LayoutButton_Click);
            //options.closeButton.Click += new EventHandler(LayoutButton_Click);
            
            var grid = new StyleEditor();
            grid.Dock = DockStyle.Fill;

            grid.PropertyValueChanged +=
                delegate(object sender, EventArgs e) {
                    //CurrentDisplay.CommandsInvoke();
                    CurrentDisplay.Invalidate();
                };

            options.OptionChanger = grid;

            options.SuspendLayout();
            options.ContentPanel.Size = grid.Size;
            options.ContentPanel.Controls.Add(grid);
            
            options.OptionList.SelectedItem = null;

            options.OptionList.Items.Clear();
            grid.Top = options.OptionList.Bottom + 1;

            IList<IStyle> optionList = new List<IStyle> (optionObject);
            foreach (object o in optionObject) {
                options.OptionList.Items.Add(o.ToString());
            }
            options.OptionList.SelectedIndexChanged +=
                delegate(object sender, EventArgs e) {
                    if (options.OptionList.SelectedIndex != -1)
                        grid.SelectedObject = optionList[options.OptionList.SelectedIndex];
                };
            options.OptionList.SelectedIndex = 0;

            options.ClientSize = ControlSize(options).Size;
            
            options.ResumeLayout(true);
            Application.DoEvents();
            options.ClientSize = ControlSize(options).Size;
            Application.DoEvents();
            options.Show();

        }

        private void showPropertyWindow(object optionObject) {
            options = new Options();
            options.applyButton.Click += new EventHandler(LayoutButton_Click);
            //options.closeButton.Click += new EventHandler(LayoutButton_Click);
            options.SuspendLayout();
            PropertyGrid grid = new PropertyGrid();
            grid.PropertyValueChanged +=
                delegate(object s, PropertyValueChangedEventArgs e) {
                    //CurrentDisplay.CommandsInvoke();
                    CurrentDisplay.Invalidate();
                };

            options.OptionChanger = grid;
            options.Controls.Add(grid);
            if (optionObject is ICollection) {
                options.OptionList.SelectedItem = null;
                options.OptionList.Items.Clear();
                IList optionArray = new ArrayList((ICollection)optionObject);
                foreach (object o in optionArray) {
                    options.OptionList.Items.Add(o.ToString());
                }
                options.OptionList.SelectedIndexChanged +=
                    delegate(object sender, EventArgs e) {
                        if (options.OptionList.SelectedIndex != -1)
                            grid.SelectedObject = optionArray[options.OptionList.SelectedIndex];
                    };
                options.OptionList.SelectedIndex = 0;

            } else {
                options.OptionList.Hide();
                grid.SelectedObject = optionObject;
            }
            grid.Dock = DockStyle.Fill;
            Rectangle optionsBounds = ControlSize(options);
            options.ClientSize = optionsBounds.Size;
            options.ResumeLayout(true);
            options.Show();

        }
        #endregion

        #region Menu - Edit


        private void Copy_Click(object sender, EventArgs e) {
            this.CurrentDisplay.WidgetDragDrop.Copy();

        }

        private void Paste_Click(object sender, EventArgs e) {
            this.CurrentDisplay.WidgetDragDrop.Paste();
            this.CurrentDisplay.CommandsExecute();
        }

        #endregion

        #region Menu - Tests

        #region ShowPosition - DebugHelper
        bool showPosition = false;
        public void display_MouseMove(object sender, MouseEventArgs e) {
            if (showPosition)
                this.scrollPosLabel.Text = new Point(e.X, e.Y).ToString();
        }

        public void display_Paint(object sender, PaintEventArgs e) {
            if (showPosition)
                this.scrollPosLabel.Text = CurrentDisplay.AutoScrollPosition.ToString();
        }

        public void testMessage(object sender, string message) {
            this.scrollPosLabel.Text = message;
            Application.DoEvents();
        }


        #endregion


        #endregion

        #region StyleToolBar
        void SetStyleToolBar(WidgetDisplay display) {
            styleComboBox.Items.Clear();
            IStyleSheet styleSheet = display.DisplayKit.StyleSheet;
            foreach (IStyle style in styleSheet.Styles) {
                styleComboBox.Items.Add(style.Name);
            }

            fontComboBox.Items.Clear();
            fontComboBox.SelectedItem = null;
            fontSizeComboBox.SelectedIndex = -1;
            fontComboBox.SelectedText = string.Empty;
            foreach (FontFamily oneFontFamily in FontFamily.Families) {
                fontComboBox.Items.Add(oneFontFamily.Name);
            }

            fontSizeComboBox.SelectedItem = null;
            fontSizeComboBox.SelectedIndex = -1;
            fontSizeComboBox.SelectedText = string.Empty;
            Application.DoEvents();
        }

        void SetStyleToolBar(IStyle selectedStyle) {
            var font = ((GDIFont) selectedStyle.Font).Native;
            fontComboBox.SelectedItem = font.Name;
            fontSizeComboBox.SelectedItem = ((int)font.SizeInPoints).ToString();
        }

        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            IStyleSheet styleSheet = CurrentDisplay.DisplayKit.StyleSheet;
            IStyle selectedStyle = styleSheet[styleComboBox.SelectedItem.ToString()];
            if (selectedStyle != null) {
                SetStyleToolBar(selectedStyle);
            }
        }

        private void fontComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            IStyleSheet styleSheet = CurrentDisplay.DisplayKit.StyleSheet;
            IStyle selectedStyle = styleSheet[styleComboBox.SelectedItem.ToString()];
            if (selectedStyle != null) {
                //int size = (int)selectedStyle.Font.SizeInPoints;
                //if (fontSizeComboBox.SelectedItem != null) {
                //    int.TryParse(fontSizeComboBox.SelectedItem.ToString(), out size);
                //}
                //string fontName = fontComboBox.SelectedItem.ToString();
                //if (selectedStyle.Font.Name != fontName) {
                //    Font oldFont = selectedStyle.Font;
                //    Font newFont = null;
                //    try {
                //        newFont = new Font(fontName, size);
                //        selectedStyle.Font = new Font(fontName, size);
                //        bool fontInUse = false;
                //        foreach (IStyle style in styleSheet.Styles) {
                //            if (style.Font == oldFont) {
                //                fontInUse = true;
                //            }
                //        }
                //        if (!fontInUse) {
                //            oldFont.Dispose();
                //        }
                //        CurrentDisplay.Invalidate();
                //    } catch { }

                //}
            }
        }
        #endregion

        #region Layout toolbar

        void InitializeToolstripPositions() {
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            Point location = this.menuStrip.Location +
                new Size(0, this.menuStrip.Size.Height);
            this.sceneTools.Location = location;
            location = new Point(this.sceneTools.Bounds.Right, this.sceneTools.Bounds.Top);
            this.layoutTools.Location = location;
            location = new Point(this.layoutTools.Bounds.Right, this.layoutTools.Bounds.Top);
            this.markerStrip.Location = location;

            location = new Point(this.markerStrip.Bounds.Right, this.markerStrip.Bounds.Top);
            this.viewToolStrip.Location = location;

            this.toolStripContainer1.TopToolStripPanel.ResumeLayout();

            //this.splitContainer.BackColor = Display1.BackColor;
            this.splitContainer.Panel1.BackColor = Display1.BackColor;
            this.splitContainer.Panel2.BackColor = Display1.BackColor;
        }

        void InitDefaultStyles() {
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            shapeCombo.ShapeComboBoxControl.ShapeLayout.StyleSheet =
                styleSheets[styleSheets.StyleSheetNames[1]];

        }

        void SetLayoutTools() {
            styleSheetCombo.Items.Clear();
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            foreach (IStyleSheet styleSheet in styleSheets.Values) {
                styleSheetCombo.Items.Add(styleSheet.Name);
            }
            Application.DoEvents();
        }
        void SetLayoutTools(WidgetDisplay display) {
            styleSheetCombo.SelectedIndexChanged -= styleSheetCombo_SelectedIndexChanged;
            styleSheetCombo.SelectedItem = display.DisplayKit.StyleSheet.Name;
            styleSheetCombo.SelectedIndexChanged += styleSheetCombo_SelectedIndexChanged;
        }

        private void styleSheetCombo_SelectedIndexChanged(object sender, EventArgs e) {
            IStyleSheet styleSheet = null;
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            if (!styleSheets.TryGetValue(styleSheetCombo.SelectedItem.ToString(), out styleSheet)) {
                IStyle style = StyleSheet.CreateStyleWithSystemSettings();
                style.Name = StyleNames.DefaultStyle;
                styleSheet = new StyleSheet(styleSheetCombo.SelectedItem.ToString(), style);
                styleSheets.Add(styleSheet.Name, styleSheet);
            }
            if (CurrentDisplay != null) {
                CurrentDisplay.DisplayKit.StyleSheet = styleSheet;
                CurrentDisplay.Invalidate();
            }
        }

        private void styleSheetCombo_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                int i = styleSheetCombo.Items.Add(styleSheetCombo.Text);
                styleSheetCombo.SelectedIndex = i;
            }
        }


        private void shapeCombo_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (IWidget widget in CurrentDisplay.Data.Selected.Elements) {
                SceneTools.ChangeShape(CurrentDisplay.Data, widget, shapeCombo.ShapeComboBoxControl.SelectedItem as IShape);
            }
            CurrentDisplay.CommandsExecute();
        }

        #endregion

        #region ImageExport
        private void exportToolStripMenuItem_Click(object sender, EventArgs e) {
            saveFileDialog.Filter = "TIF-Image|*.tif|All Files|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                Image image = ExportImage(CurrentDisplay);
                image.Save(saveFileDialog.FileName, ImageFormat.Tiff);
                image.Dispose();
            }
        }


        public virtual Image ExportImage(WidgetDisplay display) {
            Rectangle 
                clipRect =
                 Rectangle.Ceiling(GDIExtensions.Native(display.Data.Shape.BoundsRect));
            Size size =
                clipRect.Size + GDIExtensions.Native(display.DataLayout.Distance);
            clipRect.Size = size;
            // Create image
            Image result = new Bitmap((int)size.Width, (int)size.Height, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(result);

            IPaintActionEventArgs e = new GDIPaintActionEventArgs(g, clipRect);

            SolidBrush b = new SolidBrush(display.BackColor);
            g.FillRectangle(b, clipRect);

            GDIWidgetLayer layer = new GDIWidgetLayer(new Camera(new GDIMatrice()));
            layer.Data = display.Data;
            layer.Layout = display.DataLayout;
            layer.OnPaint(e);

            return result;
        }
        #endregion

        # region Streams

        void display_FocusChanged(object sender, SceneEventArgs e) {

            if (!ShowTiledGraphDocumentTool)
                return;

            if (splitContainer.Panel2.Controls.Contains(sender as Control))
                return;

            streamViewControler.FocusChanged(sender, e);
        }

        private StreamViewControler _streamViewControler = null;
        protected StreamViewControler streamViewControler {
            get {
                if (_streamViewControler == null) {
                    _streamViewControler = new StreamViewControler();
                    _streamViewControler.ShowStreamControl += this.ShowStreamControl;
                    _streamViewControler.Parent = this;
                    _streamViewControler.BackColor = Display1.BackColor;
                    _streamViewControler.sheetControl = Display2;
                    _streamViewControler.sheetManager = this.SheetManager;
                }
                return _streamViewControler;
            }
            set { _streamViewControler = value; }
        }




        private void ShowStreamControl(Control control) {
            if (control == null)
                return;

            if (!splitContainer.Panel2.Controls.Contains(control)) {
                splitContainer.Panel2.SuspendLayout();
                splitContainer.Panel2.Controls.Clear();
                splitContainer.Panel2.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                splitContainer.Panel2.ResumeLayout();
                if (CurrentControl != null && CurrentControl != control) {
                    CurrentControl.Enter -= setCurrentControl;
                    CurrentControl.MouseUp -= setCurrentControl;
                    CurrentControl.GotFocus -= setCurrentControl;
                }
                CurrentControl = control;
                control.Enter += setCurrentControl;
                control.MouseUp += setCurrentControl;
                control.GotFocus += setCurrentControl;
            }
        }

        #endregion

        #region Views

        private void toggleViewsToolStripMenuItem_Click(object sender, EventArgs e) {
            Control[] one = new Control[splitContainer.Panel1.Controls.Count];
            splitContainer.Panel1.Controls.CopyTo(one, 0);

            Control[] two = new Control[splitContainer.Panel2.Controls.Count];
            splitContainer.Panel2.Controls.CopyTo(two, 0);

            bool oneContainsWidgetDisplay = splitContainer.Panel1.Contains(Display1);

            splitContainer.SuspendLayout();

            splitContainer.Panel1.Controls.Clear();
            if (splitContainer.Panel2.Contains(Display2)) {
                splitContainer.Panel1.Controls.AddRange(two);
            } else {
                splitContainer.Panel1.Controls.Add(Display2);
            }

            if (oneContainsWidgetDisplay) {
                splitContainer.Panel2.Controls.Clear();
                splitContainer.Panel2.Controls.AddRange(one);
            }

            WidgetDisplay display = Display1;
            Display1 = Display2;
            Display2 = display;
            this.dataBaseHandler.Display = Display1;
            ShowTiledGraphDocumentTool = false;
            splitContainer.ResumeLayout();
        }

        private bool _showTiledGraphDocumentTool = false;
        public bool ShowTiledGraphDocumentTool {
            get {
                return _showTiledGraphDocumentTool;
            }
            set {
                graphDocViewButton.Checked = value;
                graphGraphViewButton.Checked = !value;
                _showTiledGraphDocumentTool = value;
            }
        }

        private void tileGraphGraphToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowTiledGraphDocumentTool = false;
            splitContainer.SuspendLayout();
            splitContainer.Panel1.SuspendLayout();

            if (!splitContainer.Panel1.Contains(Display1)) {
                splitContainer.Panel1.Controls.Clear();
                splitContainer.Panel1.Controls.Add(Display1);
            }

            splitContainer.Panel2.SuspendLayout();
            if (!splitContainer.Panel2.Contains(Display2)) {
                splitContainer.Panel2.Controls.Clear();
                splitContainer.Panel2.Controls.Add(Display2);
            }

            splitContainer.Panel1.ResumeLayout();
            splitContainer.Panel2.ResumeLayout();
            splitContainer.ResumeLayout();

        }

        private void tiledGraphDocumentToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowTiledGraphDocumentTool = true;

            if (splitContainer.Panel2.Controls.Contains(CurrentDisplay))
                return;


            if (CurrentDisplay.Data.Focused != null) {
                var fce = new SceneEventArgs(CurrentDisplay.Data, CurrentDisplay.Data.Focused);
                streamViewControler.FocusChanged(CurrentDisplay, fce);
            }
            //if (streamViewControler.StreamControl != null) {
            //    ShowStreamControl(streamViewControler.StreamControl);
            //}
        }


        private void schemaFilterToolStripMenuItem_Click(object sender, EventArgs e) {
            (sender as ToolStripMenuItem).Checked = WidgetThingGraphExtension.ToggleFilterOnTwo(CurrentDisplay.Data.Graph);
        }

        #endregion

        #region Sheet management

        ISheetManager _sheetManager = null;
        private ISheetManager SheetManager {
            get {
                if (_sheetManager == null) {
                    _sheetManager = Registry.Factory.One<ISheetManager>();
                }
                return _sheetManager;
            }
        }

        private void showTextOkCancelDialog(string title, EventHandler<EventArgs<DialogResult>> finish) {

            TextOkCancelBox NameDialog = new TextOkCancelBox();
            NameDialog.Finish += finish;
            if (splitContainer.Panel1.Contains(CurrentDisplay)) {
                splitContainer.Panel1.Controls.Add(NameDialog);
            } else if (splitContainer.Panel2.Contains(CurrentDisplay)) {
                splitContainer.Panel2.Controls.Add(NameDialog);
            }
            NameDialog.Dock = DockStyle.Top;
            NameDialog.TextBox.Text = CurrentDisplay.Text;
            NameDialog.Title = title;
            this.ActiveControl = NameDialog.TextBox;
        }

        private void saveSheetToolStripMenuItem_Click(object sender, EventArgs e) {
            if (SheetManager.IsSaveable(CurrentDisplay.Data)) {
                showTextOkCancelDialog("Sheet:", new EventHandler<EventArgs<DialogResult>>(SaveSheetDialog_Finish));
            }
        }

        void SaveSheetDialog_Finish(object sender, EventArgs<DialogResult> e) {
            WidgetDisplay currentDisplay = CurrentDisplay;

            TextOkCancelBox control = (sender as TextOkCancelBox);

            if (e.Arg == DialogResult.OK) {
                var oldInfo = SheetManager.GetSheetInfo(currentDisplay.SceneId);
                oldInfo.Name = control.Text;

                var newInfo = SheetManager.SaveToThing(currentDisplay.Data, currentDisplay.DataLayout, oldInfo);

                currentDisplay.Text = newInfo.Name;
                currentDisplay.SceneId = newInfo.Id;

            }
            control.Finish -= SaveSheetDialog_Finish;
            control.Hide();
            control.Parent = null;
            control.Dispose();
            // hide is changing the CurrentDisplay (whyever)
            CurrentDisplay = currentDisplay;

        }

        private void newSheetButton_Click(object sender, EventArgs e) {
            var currentDisplay = this.CurrentDisplay;
            SceneHistory.Save(currentDisplay, SheetManager, true);
            SceneTools.CleanScene(currentDisplay.Data);
            currentDisplay.Text = string.Empty;
            currentDisplay.Invalidate();
        }

        #endregion

        #region Search
        private void searchToolStripMenuItem_Click(object sender, EventArgs e) {
            if (new SearchHandler().IsSearchable(CurrentDisplay.Data)) {
                showTextOkCancelDialog("Search:", new EventHandler<EventArgs<DialogResult>>(SearchDialog_Finish));
            }
        }

        void SearchDialog_Finish(object sender, EventArgs<DialogResult> e) {
            TextOkCancelBox control = (sender as TextOkCancelBox);
            WidgetDisplay currentDisplay = CurrentDisplay;


            if (e.Arg == DialogResult.OK) {
                SceneHistory.Save(currentDisplay, SheetManager, true);
                string name = control.Text;
                SearchHandler search = new SearchHandler();
                search.LoadSearch(currentDisplay.Data, currentDisplay.DataLayout, name);
                currentDisplay.Text = name;

            }

            control.Finish -= SaveSheetDialog_Finish;
            control.Hide();
            control.Parent = null;
            control.Dispose();
            // hide is changing the CurrentDisplay (whyever)
            CurrentDisplay = currentDisplay;
        }

        private long homeDisplayId = 0x1da3766ef350d2ea;
        private void goHomeButton_Click(object sender, EventArgs e) {
            var display = this.Display1;
            SceneHistory.Save(display, SheetManager, true);
            SceneTools.CleanScene(display.Data);
            display.Text = string.Empty;
            display.Invalidate();
            dataBaseHandler.ShowRoots(display);
        }

        #endregion

        #region History

        SceneHistory _sceneHistory = null;
        SceneHistory SceneHistory {
            get {
                if (_sceneHistory == null) {
                    _sceneHistory = new SceneHistory();
                }
                return _sceneHistory;
            }
        }

        private void ClearHistory() {
            SceneHistory.Clear();
            SheetManager.Clear();
            goForwardButton.Enabled = true;
            goBackButton.Enabled = true;
            Display1.SceneId = 0;
            Display1.Text = string.Empty;
            Display2.SceneId = 0;
            Display2.Text = string.Empty;
        }

        private void goBackButton_Click(object sender, EventArgs e) {
            if (CurrentControl == CurrentDisplay && CurrentDisplay != null) {
                SceneHistory.Navigate(this.CurrentDisplay, SheetManager, true);
                goForwardButton.Enabled = true;
                goBackButton.Enabled = SceneHistory.CanGoBack();
            } else if (CurrentControl is INavigateTarget) {
                ((INavigateTarget)CurrentControl).GoBack();
                goForwardButton.Enabled = true;
                goBackButton.Enabled = ((INavigateTarget)CurrentControl).CanGoBack;
            }
        }

        private void goForwardButton_Click(object sender, EventArgs e) {
            if (CurrentControl == CurrentDisplay && CurrentDisplay != null) {
                SceneHistory.Navigate(this.CurrentDisplay, SheetManager, false);
                goBackButton.Enabled = true;
                goForwardButton.Enabled = SceneHistory.HasForward();
            } else if (CurrentControl is INavigateTarget) {
                ((INavigateTarget)CurrentControl).GoForward();
                goBackButton.Enabled = true;
                goForwardButton.Enabled = ((INavigateTarget)CurrentControl).CanGoForward;
            }
        }

        #endregion

        private void exportPagesToolStripMenuItem_Click(object sender, EventArgs e) {
            
            var currentDisplay = this.CurrentDisplay;
            var graph = currentDisplay.Data.Graph;
            var document = currentDisplay.Data.Focused;
            var documentSchemaManager = new DocumentSchemaManager();

            if (documentSchemaManager.HasPages(graph, document) &&
                folderBrowserDialog1.ShowDialog() == DialogResult.OK) {

                string dir = folderBrowserDialog1.SelectedPath;

                int i = 0;
                foreach (var streamThing in documentSchemaManager.PageStreams(graph, document)) {
                    string pageName = i.ToString().PadLeft(5, '0');
                    if (streamThing.Description != null)
                        pageName = streamThing.Description.ToString().PadLeft(5, '0');

                    string name = dir + Path.DirectorySeparatorChar +
                        currentDisplay.Data.Focused.Data.ToString() + " " +
                        pageName +
                        StreamTypes.Extension(streamThing.StreamType);

                    streamThing.Data.Position = 0;
                    using (FileStream fileStream = new FileStream(name, FileMode.Create)) {
                        var buff = new byte[streamThing.Data.Length];
                        streamThing.Data.Read(buff, 0, (int)streamThing.Data.Length);
                        fileStream.Write(buff, 0, (int)streamThing.Data.Length);
                        fileStream.Flush();
                        fileStream.Close();
                    }
                    streamThing.Data.Dispose ();
                    streamThing.Data = null;
                }
            }
        }
    }
}