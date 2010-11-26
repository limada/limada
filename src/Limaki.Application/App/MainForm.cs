/*
 * Limada 
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
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Limada.View;
using Limaki.Common;
using Limaki.Data;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Widgets;
using Limaki.Widgets.Layout;
using Limaki.Winform.Controls.ToolStrips;
using Limaki.Winform.Displays;
using Limaki.Winform.Viewers;

namespace Limaki.App {
    public partial class MainForm : Form {
        private string FormText = "limada::concept";

        public MainForm() {
            InitializeComponent();
            InitializePostProcess();
            saveFileDialog.InitialDirectory = Environment.SpecialFolder.MyPictures.ToString();
            openFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            
            
            //this.openFileDialog.InitialDirectory = "E:\\testdata\\txbProjekt\\Limaki";
            if (!FileManager.OpenCommandLine()) {
                //Registry.ApplyProperties<TestFormContextProcessor, MainForm>(this);
                FileManager.ShowEmptyThingGraph();
            }
        }



        private Func<Control> _getCurrentControl = null;
        public Func<Control> GetCurrentControl {
            get {
                if (_getCurrentControl == null) {
                    _getCurrentControl = () => { return SplitView.CurrentControl; };
                }
                return _getCurrentControl;
            }
            set { _getCurrentControl = value; }
        }

        private Func<WidgetDisplay> _getCurrentDisplay = null;
        public Func<WidgetDisplay> GetCurrentDisplay {
            get {
                if (_getCurrentDisplay == null) {
                    _getCurrentDisplay = () => { return SplitView.CurrentDisplay; };
                }
                return _getCurrentDisplay;
            }
            set { _getCurrentDisplay = value; }
        }

        public WidgetDisplay CurrentDisplay {
            get { return GetCurrentDisplay(); }
        }

        SceneToolStrip _sceneToolStrip = null;
        SceneToolStrip SceneToolStrip {
            get {
                if (_sceneToolStrip == null) {
                    _sceneToolStrip = new SceneToolStrip();
                    _sceneToolStrip.Controller.DisplayHandler = this.GetCurrentDisplay;
                    _sceneToolStrip.Controller.ControlHandler = this.GetCurrentControl;
                    this.toolStripContainer1.TopToolStripPanel.Controls.Add(_sceneToolStrip);
                }
                return _sceneToolStrip;
            }
        }

        ViewToolStrip _viewToolStrip = null;
        ViewToolStrip ViewToolStrip {
            get {
                if (_viewToolStrip == null) {
                    _viewToolStrip = new ViewToolStrip();
                    _viewToolStrip.Controller = this.SplitView;
                    this.toolStripContainer1.TopToolStripPanel.Controls.Add(_viewToolStrip);
                }
                return _viewToolStrip;
            }
        }

        SplitView _splitView = null;
        public SplitView SplitView {
            get {
                if (_splitView == null) {
                    _splitView = new SplitView(this.toolStripContainer1.ContentPanel);
                    this.graphGraphViewMenuItem.Click += (s, e) => {
                        _splitView.ViewMode = SplitViewMode.GraphGraph;
                    };
                    this.graphStreamViewMenuItem.Click += (s, e) => {
                        _splitView.ViewMode = SplitViewMode.GraphStream;
                    };
                    this.toggleViewsMenuItem.Click += (s, e) => {
                        _splitView.ToggleView();
                    };
                    _splitView.CurrentControlChanged += ControlChanged;
                    this.DisplayStyleChanged += _splitView.DoDisplayStyleChanged;
                }
                return _splitView;
            }
        }

        public void ControlChanged(Control control) {
            var display = control as WidgetDisplay;
            if (display != null) {
                MarkerTools_ChangeData(display.Data);
                SceneToolStrip.Controller.Attach();
                SetStyleToolBar(display);
                SetLayoutTools(display);
            }
        }

        void InitializePostProcess() {
            InitDefaultStyles();
            InitLayoutTools();
            InitializeToolstripPositions();
        }


        void InitializeToolstripPositions() {
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();

            Point location = this.menuStrip.Location +
                new Size(0, this.menuStrip.Size.Height);

            this.toolStripContainer1.TopToolStripPanel.Controls.Clear();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(true);
            Application.DoEvents();

            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();

            this.toolStripContainer1.TopToolStripPanel.Controls.Add(menuStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(SceneToolStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(layoutTools);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(markerStrip);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(ViewToolStrip);

            //this.SceneToolStrip.Location = location;


            location = new Point(this.SceneToolStrip.Bounds.Right, this.SceneToolStrip.Bounds.Top);
            this.layoutTools.Location = location;


            location = new Point(this.layoutTools.Bounds.Right, this.layoutTools.Bounds.Top);
            this.markerStrip.Location = location;


            location = new Point(this.markerStrip.Bounds.Right, this.markerStrip.Bounds.Top);
            this.ViewToolStrip.Location = location;


            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();


        }


        #region Marker-Handling
        // see also: DataPostProcess


        void MarkerTools_ChangeData(Scene scene) {
            markerCombo.Items.Clear();
            bool makeVisible = scene != null && scene.Markers != null;
            if (makeVisible) {
                markerCombo.Items.AddRange(scene.Markers.MarkersAsStrings());
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


        #region File-Handling
        private void ApplicationExit_Click(object sender, EventArgs e) {
            FileManager.Close ();
            Application.Exit();
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e) {
            FileManager.Close();
            base.OnClosing(e);
        }



        FileManager _fileManager = null;
        FileManager FileManager {
            get {
                if (_fileManager == null) {
                    _fileManager = new FileManager ();
                    _fileManager.DataBound = this.DataBound;
                    _fileManager.DataPostProcess = this.DataPostProcess;
                }
                return _fileManager;
            }
        }

        public void DataPostProcess(string dataName) {
            this.Text = dataName + " - " + this.FormText;
        }

        public void DataBound(Scene scene) {
            SplitView.ChangeData(scene);
        }



        private void OpenFile_Click(object sender, EventArgs e) {
            if (FileManager.HasUnsavedData()) {
                if (MessageBox.Show(this, "You have an unsaved document. Do you want to save it?", "", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes) {
                    SaveAsFile_Click(sender, e);
                }
            }
            openFileDialog.Filter = FileManager.FileProviderFilter + "All Files|*.*";
            openFileDialog.DefaultExt = "limo";
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK) {
                Application.DoEvents();
                FileManager.OpenFile(DataBaseInfo.FromFileName(openFileDialog.FileName));
            }
        }

        private void SaveAsFile_Click(object sender, EventArgs e) {
            openFileDialog.Filter = FileManager.FileProviderFilter + "All Files|*.*";
            openFileDialog.DefaultExt = "limo";
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK) {
                Application.DoEvents();
                FileManager.SaveAs(DataBaseInfo.FromFileName(openFileDialog.FileName));
            }
        }

        private void SaveFile_Click(object sender, EventArgs e) {
            FileManager.Save ();  
        }

        #endregion
        #region Export

        private void ExportAsMenuItem_Click(object sender, EventArgs e) {
            if (CurrentDisplay != null && 
                FileManager.IsSceneExportable(CurrentDisplay.Data)) {
                
                saveFileDialog.Filter = FileManager.FileProviderFilter+"All Files|*.*";
                saveFileDialog.DefaultExt = "limo";
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    FileManager.ExportAs(
                        DataBaseInfo.FromFileName(saveFileDialog.FileName),
                        CurrentDisplay.Data);
                }
            }
        }
        

        private void imageToolStripMenuItem_Click(object sender, EventArgs e) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null && currentDisplay.Data != null) {
                saveFileDialog.Filter = "TIF-Image|*.tif|All Files|*.*";
                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    Image image =
                        new GDIImageExporter(currentDisplay.Data, currentDisplay.DataLayout)
                            .ExportImage();
                    if (image != null) {
                        image.Save(saveFileDialog.FileName, ImageFormat.Tiff);
                        image.Dispose();
                    }
                }
            }
        }

        #endregion

        #region Menu - About
        Form about = null;
        private void aboutMenuItem_Click(object sender, EventArgs e) {
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
        private void layoutOptionsMenuItem_Click(object sender, EventArgs e) {
            options = new Options();
            options.applyButton.Click += (s1, e1) => {
                this.SceneToolStrip.Controller.Layout();
            };

            var editor = new LayoutEditor();
            editor.Dock = DockStyle.Fill;
            editor.SelectedObject = (WidgetLayout<Scene, IWidget>)CurrentDisplay.DataLayout;
            editor.PropertyValueChanged += (s1, e1) => {
                //CurrentDisplay.CommandsInvoke();
                //CurrentDisplay.Invalidate();
                OnDisplayStyleChanged(s1, new EventArgs<IStyle>(null));
            };

            options.SuspendLayout();
            options.Controls.Remove(options.OptionList);

            options.OptionChanger = editor;
            options.ContentPanel.Size = editor.Size;
            options.ContentPanel.Controls.Add(editor);

            options.ClientSize = ControlSize(options).Size;
            options.ResumeLayout(true);
            Application.DoEvents();
            options.ClientSize = ControlSize(options).Size;
            Application.DoEvents();

            options.Show();
        }
        
        public event EventHandler<EventArgs<IStyle>> DisplayStyleChanged=null;

        public void OnDisplayStyleChanged(object sender, EventArgs<IStyle> arg) {
            if (DisplayStyleChanged != null) {
                DisplayStyleChanged (sender, arg);
            }
        }

        private void showStyleEditor_Click(object sender, EventArgs e) {
            showStyleEditor(CurrentDisplay.DataLayout.StyleSheet.Styles);
        }

        private void showStyleEditor(ICollection<IStyle> optionObject) {
            options = new Options();
            options.applyButton.Click += (s1, e1) => {
                this.SceneToolStrip.Controller.Layout();
            };

            var editor = new StyleEditor();
            editor.Dock = DockStyle.Fill;

            editor.PropertyValueChanged += (s, e) => {
                //CurrentDisplay.CommandsInvoke();
                //CurrentDisplay.Invalidate();
                OnDisplayStyleChanged (s, e);
            };

            options.OptionChanger = editor;

            options.SuspendLayout();
            options.ContentPanel.Size = editor.Size;
            options.ContentPanel.Controls.Add(editor);

            options.OptionList.SelectedItem = null;

            options.OptionList.Items.Clear();
            editor.Top = options.OptionList.Bottom + 1;

            IList<IStyle> optionList = new List<IStyle>(optionObject);
            foreach (object o in optionObject) {
                options.OptionList.Items.Add(o.ToString());
            }
            options.OptionList.SelectedIndexChanged += (s, e) => {
                if (options.OptionList.SelectedIndex != -1)
                    editor.SelectedObject = optionList[options.OptionList.SelectedIndex];
            };
            options.OptionList.SelectedIndex = 0;

            options.ClientSize = ControlSize(options).Size;

            options.ResumeLayout(true);
            Application.DoEvents();
            options.ClientSize = ControlSize(options).Size;
            Application.DoEvents();
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

        private void ExampleOpen_Click(object sender, EventArgs e) {
            TestFormContextProcessor processor = Registry.Pool.TryGetCreate<TestFormContextProcessor>();
            processor.ExampleOpen(this);
        }

        private void SelectorTest_Click(object sender, EventArgs e) {
            TestFormContextProcessor processor = Registry.Pool.TryGetCreate<TestFormContextProcessor>();
            processor.SelectorTest(this.CurrentDisplay);
        }

        private void BenchmarkOneTest_Click(object sender, EventArgs e) {
            TestFormContextProcessor processor = Registry.Pool.TryGetCreate<TestFormContextProcessor>();
            processor.BenchmarkOneTest(this.CurrentDisplay);

        }

        private void ShowQuadTree_Click(object sender, EventArgs e) {
            TestFormContextProcessor processor = Registry.Pool.TryGetCreate<TestFormContextProcessor>();
            processor.ShowQuadTree(this.CurrentDisplay.Data);
        }


        private void currentProblemToolStripMenuItem_Click(object sender, EventArgs e) {
            TestFormContextProcessor processor = Registry.Pool.TryGetCreate<TestFormContextProcessor>();
            processor.currentProblemToolStripMenuItem_Click(this);
        }

        private void wCFTestToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        #endregion

        #region StyleToolBar
        void SetStyleToolBar(WidgetDisplay display) {
            styleComboBox.Items.Clear();
            IStyleSheet styleSheet = display.DataLayout.StyleSheet;
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
            var font = ((GDIFont)selectedStyle.Font).Native;
            fontComboBox.SelectedItem = font.Name;
            fontSizeComboBox.SelectedItem = ((int)font.SizeInPoints).ToString();
        }

        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            IStyleSheet styleSheet = CurrentDisplay.DataLayout.StyleSheet;
            IStyle selectedStyle = styleSheet[styleComboBox.SelectedItem.ToString()];
            if (selectedStyle != null) {
                SetStyleToolBar(selectedStyle);
            }
        }

        private void fontComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            IStyleSheet styleSheet = CurrentDisplay.DataLayout.StyleSheet;
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

        void InitDefaultStyles() {
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            shapeCombo.ShapeComboBoxControl.ShapeLayout.StyleSheet =
                styleSheets[styleSheets.StyleSheetNames[1]];

        }

        void InitLayoutTools() {
            styleSheetCombo.Items.Clear();
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            foreach (IStyleSheet styleSheet in styleSheets.Values) {
                styleSheetCombo.Items.Add(styleSheet.Name);
            }
            Application.DoEvents();
        }

        void SetLayoutTools(WidgetDisplay display) {
            styleSheetCombo.SelectedIndexChanged -= styleSheetCombo_SelectedIndexChanged;
            styleSheetCombo.SelectedItem = display.DataLayout.StyleSheet.Name;
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
                CurrentDisplay.DataLayout.StyleSheet = styleSheet;
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



        #region Views



        private void schemaFilterMenuItem_Click(object sender, EventArgs e) {
            (sender as ToolStripMenuItem).Checked = WidgetThingGraphExtension.ToggleFilterOnTwo(CurrentDisplay.Data.Graph);
        }

        #endregion


        
        private void exportPagesToolStripMenuItem_Click(object sender, EventArgs e) {
            if (CurrentDisplay != null) {
                Scene scene = CurrentDisplay.Data;
                if (FileManager.DocumentHasPages(scene) &&
                    folderBrowserDialog1.ShowDialog() == DialogResult.OK) {
                    FileManager.ExportPages (folderBrowserDialog1.SelectedPath, scene);
                }
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e) {
            SplitView.DoSearch();
        }

        #region Favorites

        private void addToFavoritesMenuItem_Click(object sender, EventArgs e) {
            SplitView.AddFocusedToFavorites();
        }

        private void viewOnOpenMenuItem_Click(object sender, EventArgs e) {
            SplitView.ViewOnOpen();
        }

        #endregion

        #region Printing
        private void printToolStripMenuItem_Click(object sender, EventArgs e) {
            PrintManager man = new PrintManager();
            using (var doc = man.CreatePrintDocument(this.CurrentDisplay)) {
                printDialog.Document = doc;
                if (printDialog.ShowDialog() == DialogResult.OK) {
                    doc.Print();

                }
                printDialog.Document = null;
            }
        }

        private void printPreviewToolStripMenuItem_Click(object sender, EventArgs e) {
            PrintManager man = new PrintManager();
            using (var doc = man.CreatePrintDocument(this.CurrentDisplay)) {
                printPreviewDialog.Document = doc;
                if (printPreviewDialog.ShowDialog() == DialogResult.OK) {
                    doc.Print();
                }
                printPreviewDialog.Document = null;
            }
        }
        #endregion

    }
}