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
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Limada.App;
using Limada.View;
using Limaki.App;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Model.Streams;
using Limaki.Widgets;
using Limaki.Winform.Controls;
using Limaki.Winform.Displays;


namespace Limaki.Winform.Viewers {
    
    public enum SplitViewMode {
        /// <summary>
        /// shows two WidgetDisplays
        /// </summary>
        GraphGraph,
        /// <summary>
        /// shows one WidgetDisplay and one StreamViewer
        /// </summary>
        GraphStream
    }
    
    public struct HistoryState {
        public bool CanGoBack;
        public bool CanGoForward;
    }

    public class  SplitView : ISplitView, IDisposable {

        public SplitView(Control parent) {
            this.Parent = parent;
            Init ();
        }

        #region Parent-Handling
        public Control Parent = null;
        
        Control _activeControl = null;
        public Control ActiveControl {
            get { return splitContainer.ActiveControl; }
            set { splitContainer.ActiveControl = value; }
        }
        
        #endregion

        WidgetDisplay Display1= null;
        WidgetDisplay Display2= null;
        public SplitContainer splitContainer = null;

        public void InitializeComponent() {
            this.Parent.SuspendLayout();
            if (splitContainer == null) {
                splitContainer = new SplitContainer();
                this.Parent.Controls.Add(splitContainer);
                splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
                splitContainer.Margin = new System.Windows.Forms.Padding(2);
                splitContainer.Name = "splitContainer";
                splitContainer.Panel2MinSize = 0;
                splitContainer.SplitterWidth = 3;
                splitContainer.TabIndex = 1;
            }
            if (Display1 == null) {
                Display1 = new WidgetDisplay();
                Display1.Dock = DockStyle.Fill;
                splitContainer.Panel1.Controls.Add(Display1);
            }
            if (Display2 == null) {
                Display2 = new WidgetDisplay();
                Display2.Dock = DockStyle.Fill;
                splitContainer.Panel2.Controls.Add(Display2);
            }
            this.Parent.ResumeLayout();
            this.Parent.PerformLayout();
            splitContainer.SplitterDistance = (int)(Parent.Width / 2);
        }

        public void Init() {
            InitializeComponent();

            Display1.BackColor = SystemColors.Window;

            Display1.ZoomState = ZoomState.Original;
            Display1.SelectAction.Enabled = true;
            Display1.ScrollAction.Enabled = false;

            initializeDisplay(Display1);
            initializeDisplay(Display2);

            CurrentDisplay = Display1;

            //this.splitContainer.BackColor = Display1.BackColor;
            this.splitContainer.Panel1.BackColor = Display1.BackColor;
            this.splitContainer.Panel2.BackColor = Display1.BackColor;
            // this is for mono on linux:
            this.ActiveControl = Display1;
        }


        void initializeDisplay(WidgetDisplay display) {
            display.Enter -= DisplayFocusHelper;
            display.MouseUp -= DisplayFocusHelper;
            display.Enter += DisplayFocusHelper;
            display.MouseUp += DisplayFocusHelper;

            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            IStyleSheet styleSheet = null;

            if (styleSheets.TryGetValue(display.DataLayout.StyleSheet.Name, out styleSheet)) {
                display.DataLayout.StyleSheet = styleSheet;
            } else {
                styleSheets.Add(display.DataLayout.StyleSheet.Name, display.DataLayout.StyleSheet);
            }

            display.SceneFocusChanged -= SceneFocusChanged;
            display.SceneFocusChanged += SceneFocusChanged;

        }

        public void ChangeData(Scene scene) {
            ClearHistory();

            CurrentDisplay = null;

            this.Display1.Data = scene;
            new FavoriteManager().ShowRoots(this.Display1, true);

            this.Display2.Data = null;

            StreamViewManager.Clear();

            Registry.ApplyProperties<MarkerContextProcessor, Scene>(Display1.Data);

            new WiredDisplays().MakeSideDisplay(this.Display1, this.Display2);

            Registry.ApplyProperties<MarkerContextProcessor, Scene>(Display2.Data);
            GraphStreamView();
            CurrentDisplay = Display1;
        }

        public event EventHandler<EventArgs<ISplitView>> RefreshToolViewer = null;
        public void OnRefreshToolViewer() {
            if (RefreshToolViewer != null) {
                RefreshToolViewer (this, new EventArgs<ISplitView> (this));
            }
        }

        #region Display-Switching
        private object controlLocker = new object();
        public event Action<Control> CurrentControlChanged=null;
        protected void OnCurrentControlChange(Control control) {
            if (CurrentControlChanged != null) {
                CurrentControlChanged(control);
            }
        }

        private WidgetDisplay _currentDisplay = null;
        public WidgetDisplay CurrentDisplay {
            get {
                return _currentDisplay;
            }
            set {
                _currentDisplay = value;
                CurrentControl = value;
            }
        }

        void DisplayFocusHelper(object sender, EventArgs e) {
            CurrentDisplay = sender as WidgetDisplay;
        }

        private object locker = new object ();
        private Control _currentControl = null;
        public Control CurrentControl {
            get { return _currentControl; }
            set {
                lock (locker) {
                    bool isChange = _currentControl != value;
                    if (value is WidgetDisplay) {
                        _currentDisplay = (WidgetDisplay) value;
                    }
                    _currentControl = value;
                    if (isChange) {
                        OnCurrentControlChange (value);
                        OnRefreshToolViewer ();
                    }
                }
            }
        }
        
        void ControlFocusHelper(object sender, EventArgs e) {
            CurrentControl = sender as Control;
        }

        void ApplyFocusHelpers(Control control) {
            if (control != null) {
                control.Enter += ControlFocusHelper;
                control.MouseUp += ControlFocusHelper;
                control.GotFocus += ControlFocusHelper;
            }
        }
        #endregion 


        #region View-Switching
        SplitViewMode _viewMode = SplitViewMode.GraphStream;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphStream)
                        this.GraphStreamView ();
                    else if (value == SplitViewMode.GraphGraph)
                        this.GraphGraphView ();
                    _viewMode = value;
                    OnRefreshToolViewer ();
                }
                
            }
        }

        private StreamViewManager _streamViewManager = null;
        protected StreamViewManager StreamViewManager {
            get {
                if (_streamViewManager == null) {
                    _streamViewManager = new StreamViewManager();
                    _streamViewManager.AfterStreamLoaded += this.AfterStreamLoaded;
                    _streamViewManager.Parent = this.Parent;
                    _streamViewManager.BackColor = Display1.BackColor;
                    _streamViewManager.sheetControl = Display2;
                    _streamViewManager.sheetManager = this.SheetManager;
                    _streamViewManager.Attach += this.ApplyFocusHelpers;
                }
                if (CurrentDisplay == Display2)
                    _streamViewManager.sheetControl = Display1;
                else
                    _streamViewManager.sheetControl = Display2;
                return _streamViewManager;
            }
            set { _streamViewManager = value; }
        }

        
        public void ToggleView() {
            Control[] one = new Control[splitContainer.Panel1.Controls.Count];
            splitContainer.Panel1.Controls.CopyTo(one, 0);

            Control[] two = new Control[splitContainer.Panel2.Controls.Count];
            splitContainer.Panel2.Controls.CopyTo(two, 0);

            bool oneContainsWidgetDisplay = splitContainer.Panel1.Contains(Display1);
            bool oneContainsCurrentControl = splitContainer.Panel1.Contains(CurrentControl);
            splitContainer.SuspendLayout();

            splitContainer.Panel1.Controls.Clear();
            splitContainer.Panel2.Controls.Clear();
            splitContainer.Panel1.Controls.AddRange(two);
            splitContainer.Panel2.Controls.AddRange(one);


            var display = Display1;
            Display1 = Display2;
            Display2 = display;

            splitContainer.ResumeLayout();
        }

        protected void GraphGraphView() {
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
        
        protected  void GraphStreamView() {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null && 
                currentDisplay.Data !=null && 
                currentDisplay.Data.Focused != null) {
                var fce = new SceneEventArgs(currentDisplay.Data, currentDisplay.Data.Focused);
                StreamViewManager.ChangeViewer(currentDisplay, fce);
            }

        }

        private void AfterStreamLoaded(Control control, Action onShowAction) {
            if (control == null)
                return;

            SplitterPanel panel = null;
            if (splitContainer.Panel1.Controls.Contains(CurrentDisplay)) {
                panel = splitContainer.Panel2;
            } else {
                panel = splitContainer.Panel1;
            }
            if (!panel.Controls.Contains(control)) {
                panel.SuspendLayout();
                panel.Controls.Clear();
                panel.Controls.Add(control);
                control.Dock = DockStyle.Fill;
                panel.ResumeLayout();
            }

            if (onShowAction != null) {
                onShowAction();
            }
        }

        void SceneFocusChanged(object sender, SceneEventArgs e) {

            if (ViewMode != SplitViewMode.GraphStream)
                return;

            StreamViewManager.ChangeViewer(sender, e);
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



        public void SaveDocument() {
            if (CurrentControl == CurrentDisplay) {
                if (SheetManager.IsSaveable(CurrentDisplay.Data)) {
                    showTextOkCancelDialog("Sheet:", CurrentDisplay.Text,
                                            new EventHandler<EventArgs<DialogResult>>(SaveSheetDialog_Finish));
                }
            } else {
                StreamViewManager.SaveStream(WidgetThingGraphExtension.GetThingGraph(CurrentDisplay.Data.Graph));
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

        public void NewSheet() {
            var currentDisplay = this.CurrentDisplay;
            SceneHistory.Save(currentDisplay, SheetManager, true);
            SceneTools.CleanScene(currentDisplay.Data);
            currentDisplay.Text = string.Empty;
            currentDisplay.Invalidate();
            OnRefreshToolViewer ();
        }

        #endregion

        #region Search
        public void DoSearch() {
            if (new SearchHandler().IsSearchable(CurrentDisplay.Data)) {
                showTextOkCancelDialog("Search:", CurrentDisplay.Text,
                    new EventHandler<EventArgs<DialogResult>>(SearchDialog_Finish));
            }
        }

        void SearchDialog_Finish(object sender, EventArgs<DialogResult> e) {
            var control = (sender as TextOkCancelBox);
            var currentDisplay = CurrentDisplay;


            if (e.Arg == DialogResult.OK) {
                SceneHistory.Save(currentDisplay, SheetManager, true);
                string name = control.Text;
                var search = new SearchHandler();
                search.LoadSearch(currentDisplay.Data, currentDisplay.DataLayout, name);
                currentDisplay.Text = name;
                currentDisplay.ResetScroll();
                currentDisplay.Invalidate();
                OnRefreshToolViewer();
            }

            control.Finish -= SaveSheetDialog_Finish;
            control.Hide();
            control.Parent = null;
            control.Dispose();
            // hide is changing the CurrentDisplay (whyever)
            CurrentDisplay = currentDisplay;
        }

        private long homeDisplayId = 0x1da3766ef350d2ea;
        public void GoHome() {
            var display = this.CurrentDisplay;
            if (display != null) {
                SceneHistory.Save(display, SheetManager, true);
                SceneTools.CleanScene(display.Data);
                display.Text = string.Empty;
                display.Invalidate();
                new FavoriteManager().ShowRoots(display, false);
                OnRefreshToolViewer();
            }
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
            Display1.SceneId = 0;
            Display1.Text = string.Empty;
            Display2.SceneId = 0;
            Display2.Text = string.Empty;
        }

        public bool CanGoBackOrForward(bool forward) {
            var currentDisplay = this.CurrentDisplay;
            var currentControl = this.CurrentControl;
            if (currentControl == currentDisplay && currentDisplay != null) {
                if (forward)
                    return SceneHistory.CanGoForward();
                else
                    return SceneHistory.CanGoBack();
            } else if (currentControl is INavigateTarget) {
                if (forward)
                    return ((INavigateTarget)currentControl).CanGoForward;
                else
                    return ((INavigateTarget)currentControl).CanGoBack;
            }
            return false;
        }

        public void GoBackOrForward(bool forward) {
            var currentDisplay = this.CurrentDisplay;
            var currentControl = this.CurrentControl;
            if (currentControl == currentDisplay && currentDisplay != null) {
                SceneHistory.Navigate(currentDisplay, SheetManager, forward);
            } else if (currentControl is INavigateTarget) {
                if (forward)
                    ((INavigateTarget)currentControl).GoForward();
                else
                    ((INavigateTarget)currentControl).GoBack();
            
            }
            OnRefreshToolViewer();
        }

        #endregion

        #region Favorites
        public void AddFocusedToFavorites() {
            new FavoriteManager().AddToFavorites(CurrentDisplay.Data);
        }

        public void ViewOnOpen() {
            new FavoriteManager().ViewOnOpen(CurrentDisplay.Data);
        }

        #endregion

        #region Notes
        public void NewNote() {
            showTextOkCancelDialog("Note:", "new note",
                new EventHandler<EventArgs<DialogResult>>(newNoteButton_Finish));
        }

        void createNewNote(string title) {
            var scene = CurrentDisplay.Data;
            if (scene == null)
                return;

            StreamInfo<Stream> streamInfo = new StreamInfo<Stream>(
                new MemoryStream(), CompressionType.bZip2, StreamTypes.RTF);

            streamInfo.Description = title;

            var writer = new StreamWriter(streamInfo.Data);

            writer.Write(@"{\rtf1\ansi\deff0");
            writer.Write(@"{\info{\doccomm limada.note}}");

            writer.Write(@"{\fonttbl{\f0\froman Times New Roman;}}");
            writer.Write(@"\pard\plain ");
            writer.Write(title);
            writer.Write(@"}");
            writer.Flush();
            streamInfo.Data.Position = 0;


            var widget = new WidgetThingStreamHelper().CreateFromStream(scene.Graph, streamInfo);
            var root = scene.Focused;

            var layout = CurrentDisplay.DataLayout;

            if (root == null) {
                PointI pt = new PointI(layout.Distance.Width, scene.Shape.BoundsRect.Bottom);
                SceneTools.AddItem(scene, widget, layout, pt);
            } else {
                SceneTools.PlaceWidget(root, widget, scene, layout);
            }
            scene.Selected.Clear();
            scene.Focused = widget;
            CurrentDisplay.CommandsExecute();
            CurrentDisplay.OnSceneFocusChanged();
        }

        void newNoteButton_Finish(object sender, EventArgs<DialogResult> e) {
            WidgetDisplay currentDisplay = CurrentDisplay;

            TextOkCancelBox control = (sender as TextOkCancelBox);
            if (currentDisplay != null && currentDisplay.Data != null) {
                if (e.Arg == DialogResult.OK) {
                    createNewNote(control.Text);
                }
            }
            control.Finish -= SaveSheetDialog_Finish;
            control.Hide();
            control.Parent = null;
            control.Dispose();
            // hide is changing the CurrentDisplay (whyever)
            CurrentDisplay = currentDisplay;
        }


        #endregion

        private void showTextOkCancelDialog(string title, string text, EventHandler<EventArgs<DialogResult>> finish) {

            TextOkCancelBox NameDialog = new TextOkCancelBox();
            NameDialog.Finish += finish;
            if (splitContainer.Panel1.Contains(CurrentDisplay)) {
                splitContainer.Panel1.Controls.Add(NameDialog);
            } else if (splitContainer.Panel2.Contains(CurrentDisplay)) {
                splitContainer.Panel2.Controls.Add(NameDialog);
            }
            NameDialog.Dock = DockStyle.Top;
            NameDialog.TextBox.Text = text;
            NameDialog.Title = title;
            this.ActiveControl = NameDialog.TextBox;
        }

        public void DoDisplayStyleChanged(object sender, EventArgs<IStyle> arg) {
            if (Display1 != null) {
                Display1.Invalidate ();
            }
            if (Display2 != null) {
                Display2.Invalidate();
            }
        }

        #region IDisposable Member

        public void Dispose() {
            this.StreamViewManager.Dispose();
        }

        #endregion
    }
}
