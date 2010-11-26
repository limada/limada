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
using System.IO;
using Limada.Presenter;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Model.Streams;
using Limaki.Presenter;
using Limaki.Presenter.Widgets;
using Limaki.Presenter.Widgets.UI;
using Limaki.Widgets;

namespace Limaki.UseCases.Viewers {
    public class SplitView : ISplitView, IDisposable, ICheckable {
        #region Initialize
        public void Initialize() {
            Display1.BackColor = KnownColors.FromKnownColor(KnownColor.Window);

            Display1.ZoomState = ZoomState.Original;
            Display1.SelectAction.Enabled = true;
            Display1.ScrollAction.Enabled = false;

            InitializeDisplay(Display1);
            InitializeDisplay(Display2);

            CurrentDisplay = Display1;
        }

        public void InitializeDisplay(WidgetDisplay display) {
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            IStyleSheet styleSheet = null;

            if (styleSheets.TryGetValue(display.StyleSheet.Name, out styleSheet)) {
                display.StyleSheet = styleSheet;
            } else {
                styleSheets.Add(display.StyleSheet.Name, display.StyleSheet);
            }

            display.SceneFocusChanged -= SceneFocusChanged;
            display.SceneFocusChanged += SceneFocusChanged;

            if (DeviceInitializeDisplay !=null) {
                DeviceInitializeDisplay (display);
            }
        }

        #endregion 

        public WidgetDisplay Display1 { get; set; }
        public WidgetDisplay Display2 { get; set; }
        public object Parent { get; set; }

        public event Action<WidgetDisplay> DeviceInitializeDisplay = null;

        public Action<object, Action> AfterStreamLoaded { get; set; }
        public Action<string, string, Action<string>> ShowTextDialog { get; set; }

        #region View-Switching
        SplitViewMode _viewMode = SplitViewMode.GraphStream;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphStream)
                        this.GraphStreamView();
                    else if (value == SplitViewMode.GraphGraph)
                        this.GraphGraphView();
                    _viewMode = value;
                    OnViewChanged();
                }
            }
        }

        WidgetDisplay _currentDisplay = null;
        public WidgetDisplay CurrentDisplay {
            get { return _currentDisplay; }
            protected set {
                _currentDisplay = value;
                CurrentControl = value;
            }
        }

        private object locker = new object();
        object _currentControl = null;
        public object CurrentControl {
            get { return _currentControl; }
            protected set {
                lock (locker) {
                    bool isChange = _currentControl != value;
                    if (value is WidgetDisplay) {
                        _currentDisplay = (WidgetDisplay)value;
                    }
                    _currentControl = value;
                    if (isChange) {
                        OnCurrentControlChanged(value);
                        OnViewChanged();
                    }
                }
            }
        }

        public Action<object> ApplyGotFocus { get; set; }
        
        public void DisplayGotFocus(object sender) {
            CurrentDisplay = sender as WidgetDisplay;
        }

        public void ControlGotFocus(object sender) {
            CurrentControl = sender;
        }

        public event Action DeviceGraphGraphView = null;
        public event Action DeviceGraphStreamView = null;
        public event Action DeviceToggleView = null;

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }

        public void ToggleView() {
            if (DeviceToggleView != null) {
                DeviceToggleView();
            }
            var display = Display1;
            Display1 = Display2;
            Display2 = display;
        }

        public void GraphGraphView() {
            if (DeviceGraphGraphView != null) {
                DeviceGraphGraphView();
            }
        }

        public void GraphStreamView() {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null &&
                currentDisplay.Data != null &&
                currentDisplay.Data.Focused != null) {
                var fce = new SceneEventArgs(currentDisplay.Data, currentDisplay.Data.Focused);
                StreamViewProvider.ChangeViewer(currentDisplay, fce);
            }
            if (DeviceGraphStreamView != null) {
                DeviceGraphStreamView();
            }
        }

        public event EventHandler ViewChanged = null;
        public void OnViewChanged() {
            if (ViewChanged != null) {
                ViewChanged(this,new EventArgs());
            }
        }
        
        public event Action<object> CurrentControlChanged = null;
        public void OnCurrentControlChanged(object control) {
            if (CurrentControlChanged != null) {
                CurrentControlChanged(control);
            }
        }

        #endregion

        public void ChangeData(Scene scene) {
            ClearHistory();

            CurrentDisplay = null;

            Display1.Data = scene;
            new FavoriteManager().GoHome(Display1, true);

            Display2.Data = null;

            StreamViewProvider.Clear();

            Registry.ApplyProperties<MarkerContextProcessor, Scene>(Display1.Data);

            new WiredDisplays().MakeSideDisplay(Display1, Display2);

            Registry.ApplyProperties<MarkerContextProcessor, Scene>(Display2.Data);

            GraphStreamView();

            CurrentDisplay = Display1;
        }

        #region StreamView

        private StreamViewProvider _streamViewProvider = null;
        public StreamViewProvider StreamViewProvider {
            get {
                if (_streamViewProvider == null) {
                    _streamViewProvider = new StreamViewProvider();
                }
                
                _streamViewProvider.Parent = this.Parent;
                _streamViewProvider.BackColor = Display1.BackColor;

                _streamViewProvider.AfterStreamLoaded += this.AfterStreamLoaded;
                _streamViewProvider.Attach += this.ApplyGotFocus;

                _streamViewProvider.sheetManager = this.SheetManager;

                if (CurrentDisplay == Display2)
                    _streamViewProvider.sheetControl = Display1;
                else
                    _streamViewProvider.sheetControl = Display2;

                return _streamViewProvider;
            }
            set { _streamViewProvider = value; }
        }



        void SceneFocusChanged(object sender, SceneEventArgs e) {

            if (ViewMode != SplitViewMode.GraphStream)
                return;
            try {
                StreamViewProvider.ChangeViewer(sender, e);
            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } 
        }

        #endregion

        #region SheetManagement

        public ISheetManager SheetManager {get;set;}

        public void SaveDocument() {
            if (CurrentControl == CurrentDisplay) {
                var currentDisplay = this.CurrentDisplay;
                if (SheetManager.IsSaveable(currentDisplay.Data as Scene)) {
                    ShowTextDialog("Sheet:", 
                                           currentDisplay.Text,
                                           SaveSheet);
                }
            } else {
                this.StreamViewProvider.SaveStream(WidgetThingGraphExtension.GetThingGraph(CurrentDisplay.Data.Graph));
            }
        }

        

        public void SaveSheet(string name) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                var oldInfo = SheetManager.GetSheetInfo (currentDisplay.SceneId);
                oldInfo.Name = name;

                var newInfo = SheetManager.SaveToThing (currentDisplay.Data, currentDisplay.Layout, oldInfo);

                currentDisplay.Text = newInfo.Name;
                currentDisplay.SceneId = newInfo.Id;
            }
        }

        public void NewSheet() {
            var currentDisplay = this.CurrentDisplay;
            SceneHistory.Save(currentDisplay, SheetManager, true);
            SceneTools.CleanScene(currentDisplay.Data);
            currentDisplay.Text = string.Empty;
            currentDisplay.DeviceRenderer.Render();
            OnViewChanged();
        }

        #endregion

        #region Search

        public void Search(string name) {
            var currentDisplay = this.CurrentDisplay;
            SceneHistory.Save (currentDisplay, SheetManager, true);
            var search = new SearchHandler ();
            search.LoadSearch (currentDisplay.Data, currentDisplay.Layout, name);
            currentDisplay.Text = name;
            currentDisplay.Viewport.Reset ();
            currentDisplay.DeviceRenderer.Render ();
            OnViewChanged ();
        }

        public void DoSearch() {
            var currentDisplay = this.CurrentDisplay;
            if (new SearchHandler().IsSearchable(currentDisplay.Data)) {
                ShowTextDialog("Search:", currentDisplay.Text,this.Search);
            }
        }

        #endregion

        #region History


        public SceneHistory SceneHistory { get; set; }
        
        private void ClearHistory() {
            if (SceneHistory != null) {
                SceneHistory.Clear ();
            }

            if (SheetManager != null) {
                SheetManager.Clear();
            }

            Display1.SceneId = 0;
            Display1.Text = string.Empty;
            Display2.SceneId = 0;
            Display2.Text = string.Empty;
        }

        #endregion 

        #region Navigating

        private long homeDisplayId = 0x1da3766ef350d2ea;
        public void GoHome() {
            var display = this.CurrentDisplay;
            if (display != null) {
                SceneHistory.Save(display, SheetManager, true);
                SceneTools.CleanScene(display.Data);
                display.Text = string.Empty;
                display.DeviceRenderer.Render();
                new FavoriteManager().GoHome(display, false);
                OnViewChanged();
            }
        }

        public bool CanGoBackOrForward(bool forward) {
            if (SceneHistory == null)
                return false;

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
            OnViewChanged();
        }

        #endregion

        #region Favorites
        
        public void AddFocusedToFavorites() {
            new FavoriteManager().AddToFavorites(CurrentDisplay.Data as Scene);
        }

        public void ViewOnOpen() {
            new FavoriteManager().ViewOnOpen(CurrentDisplay.Data as Scene);
        }

        #endregion

        #region Notes

        public void NewNote() {
            ShowTextDialog("Note:", "new note", CreateNewNote);
        }

        public void CreateNewNote(string title) {
            var currentDiplay = CurrentDisplay;
            if (currentDiplay == null)
                return;
            var scene = currentDiplay.Data;
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

            var layout = currentDiplay.Layout;

            if (root == null) {
                PointI pt = new PointI(layout.Distance.Width, scene.Shape.BoundsRect.Bottom);
                SceneTools.AddItem(scene, widget, layout, pt);
            } else {
                SceneTools.PlaceWidget(root, widget, scene, layout);
            }
            scene.Selected.Clear();
            scene.Focused = widget;
            currentDiplay.Execute();
            currentDiplay.OnSceneFocusChanged();
        }
        #endregion

        public void DoDisplayStyleChanged(object sender, EventArgs<IStyle> arg) {
            if (Display1 != null) {
                Display1.DeviceRenderer.Render();
            }
            if (Display2 != null) {
                Display2.DeviceRenderer.Render();
            }
        }

        public void Dispose() {
            ClearHistory ();
            this.StreamViewProvider.Dispose();

        }

        public virtual bool Check() {
            if (this.SceneHistory == null) {
                throw new CheckFailedException(this.GetType(), typeof(SceneHistory));
            }
            if (this.SheetManager == null) {
                throw new CheckFailedException(this.GetType(), typeof(SheetManager));
            }
            if (this.Display1 == null) {
                throw new CheckFailedException(this.GetType(), typeof(WidgetDisplay));
            }
            if (this.Display2 == null) {
                throw new CheckFailedException(this.GetType(), typeof(WidgetDisplay));
            }
            if (this.ShowTextDialog == null) {
                throw new CheckFailedException(this.GetType()+"needs a ShowTextDialogAction");
            }
            
            Display1.Check ();
            Display2.Check ();

            return true;
        }
    }
}