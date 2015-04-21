/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Limada.View.ContentViewers;
using Limada.View.VisualThings;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.UI.GraphScene;
using Limaki.View.Viz.Visuals;
using Xwt;
using Xwt.Backends;

namespace Limada.View.Vidgets {

    [BackendType(typeof(ISplitViewBackend))]
    public class SplitView0 : Vidget, ISplitView, IDisposable, ICheckable {

        public SplitView0 () {
            Compose();
        }

        public new ISplitViewBackend Backend { get { return BackendHost.Backend as ISplitViewBackend; } }

        #region Initialize

        public virtual void Compose () {
            Display1 = new VisualsDisplay();
            Display2 = new VisualsDisplay();
            
            Display1.BackColor = SystemColors.Window;

            Display1.ZoomState = ZoomState.Original;
            Display1.SelectAction.Enabled = true;
            Display1.MouseScrollAction.Enabled = false;

            InitializeDisplay(Display1);
            InitializeDisplay(Display2);

            CurrentDisplay = Display1;
        }

        IGraphSceneMesh<IVisual, IVisualEdge> _mesh = null;
        public IGraphSceneMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneMesh<IVisual, IVisualEdge>>()); } }

        public void InitializeDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var styleSheets = Registry.Pooled<StyleSheets>();
            IStyleSheet styleSheet = null;

            if (styleSheets.TryGetValue(display.StyleSheet.Name, out styleSheet)) {
                display.StyleSheet = styleSheet;
            } else {
                styleSheets.Add(display.StyleSheet.Name, display.StyleSheet);
            }

            display.SceneFocusChanged -= SceneFocusChanged;
            display.SceneFocusChanged += SceneFocusChanged;
            
            Backend.SetFocusCatcher(display.Backend);
            Backend.InitializeDisplay(display.Backend);

            Mesh.AddDisplay (display);

        }

        #endregion 

        public IGraphSceneDisplay<IVisual, IVisualEdge> Display1 { get; set; }
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display2 { get; set; }

        #region View-Switching
        SplitViewMode _viewMode = SplitViewMode.GraphStream;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphStream)
                        this.GraphContentView();
                    else if (value == SplitViewMode.GraphGraph)
                        this.GraphGraphView();
                    _viewMode = value;
                    OnViewChanged();
                }
            }
        }

        IGraphSceneDisplay<IVisual, IVisualEdge> _currentDisplay = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> CurrentDisplay {
            get { return _currentDisplay; }
            protected set {
                //_currentDisplay = value;
                CurrentWidget = value;
            }
        }

        private object locker = new object();
        object _currentWidget = null;
        public object CurrentWidget {
            get { return _currentWidget; }
            protected set {
                lock (locker) {
                    bool isChange = _currentWidget != value;
                    var display = value as IGraphSceneDisplay<IVisual, IVisualEdge>;
                    if (display != null || value == null) {
                        //isChange = _currentDisplay != value;
                        _currentDisplay = display;
                    }
                    _currentWidget = value;
                    if (isChange) {
                        OnCurrentWidgetChanged(value);
                        OnViewChanged();
                    }
                }
            }
        }

        public void DisplayGotFocus(object sender) {
            CurrentDisplay = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
        }

        public void WidgetGotFocus(object sender) {
            CurrentWidget = sender;
        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pooled<IExceptionHandler>(); }
        }

        public void ToggleView() {
            if (Backend != null) {
                Backend.ToggleView();
            }
            var display = Display1;
            Display1 = Display2;
            Display2 = display;
        }

        public void GraphGraphView() {
            if (Backend != null) {
                Backend.GraphGraphView();
            }
        }

        public void GraphContentView() {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null &&
                currentDisplay.Data != null &&
                currentDisplay.Data.Focused != null) {

                var eventArgs = new GraphSceneEventArgs<IVisual, IVisualEdge> (currentDisplay.Data, currentDisplay.Data.Focused);

                var contentViewManager = this.ContentViewManager;

                if (contentViewManager.SheetViewer == currentDisplay)
                    contentViewManager.SheetViewer = AdjacentDisplay (currentDisplay);

                contentViewManager.ShowViewer (currentDisplay, eventArgs);
            }
            if (Backend != null) {
                Backend.GraphContentView();
            }
        }

        public event EventHandler ViewChanged = null;
        public void OnViewChanged() {
            if (ViewChanged != null) {
                ViewChanged(this,new EventArgs());
            }
        }
        
        public event Action<object> CurrentWidgetChanged = null;
        public void OnCurrentWidgetChanged(object backend) {
            if (CurrentWidgetChanged != null) {
                CurrentWidgetChanged(backend);
            }
        }

        #endregion

        public void ShowInNewWindow () {

            var source = CurrentDisplay ?? this.Display1;
            if (source == null)
                return;

            var graph = source.Data.Graph;
            var focused = source.Data.Focused;

            IVidgetBackend backend = null;
            Action onClose = null;

            if (graph != null && focused != null) {
                using (var contentViewManager = new ContentViewManager { IsProviderOwner = false }) {

                    if (contentViewManager.IsContent (graph, focused)) {
                        // TODO: get viewer, make a new instance of it, get the backend
                        // if sheetviewer, use display(see down) as Viewer
                    }

                }
            }
            // TODO: see above; for now we take always a VisualsDisplay
            {
                var display = new VisualsDisplay();
                onClose += () => Mesh.RemoveDisplay (display);

                Mesh.CopyDisplayProperties (source, display);
                display.Data = Mesh.CreateSinkScene (graph);

                Mesh.AddDisplay (display);
                backend = display.Backend;
            }

            Backend.ViewInWindow (backend, onClose);
        }

        public void ChangeData(IGraphScene<IVisual, IVisualEdge> scene) {

            var oldScene = Display1.Data;
            IList<IGraphSceneDisplay<IVisual, IVisualEdge>> displays = new IGraphSceneDisplay<IVisual, IVisualEdge>[] { Display2 };

            if (oldScene != null) {
                displays = Mesh.DisplaysOfBackGraph (oldScene.Graph).ToList ();
                if (!displays.Contains (Display2)) {
                    displays.Add (Display2);
                }
                Clear ();

                displays.ForEach (d => Mesh.RemoveScene (d.Data));
                displays.ForEach (d => Mesh.ClearDisplaysOf (d.Data));
            }

            CurrentDisplay = null;
            Display1.Data = scene;

            scene.CreateMarkers();
            Mesh.AddScene (scene);

            displays
                .Where (d => d != Display1)
                .ForEach (d => {
                    Mesh.CopyDisplayProperties (Display1, d);
                    d.Data = Mesh.CreateSinkScene (Display1.Data.Graph);
                    Mesh.AddScene (d.Data);
                });

            FavoriteManager.GoHome (Display1, true);
            GraphGraphView();
            GraphContentView();

            CurrentDisplay = Display1;
        }

        #region ContentView

        private ContentViewManager _contentViewManager = null;
        public ContentViewManager ContentViewManager {
            get {
                if (_contentViewManager == null) {
                    _contentViewManager = new ContentViewManager();
                }
                
                _contentViewManager.BackColor = Display1.BackColor;

                _contentViewManager.AttachViewerBackend = Backend.AttachViewerBackend;
                _contentViewManager.ViewersAttachBackend = Backend.SetFocusCatcher;
               
                _contentViewManager.SheetManager = this.SheetManager;
                
                return _contentViewManager;
            }
            set { _contentViewManager = value; }
        }

        public virtual IGraphSceneDisplay<IVisual,IVisualEdge> AdjacentDisplay(IGraphSceneDisplay<IVisual,IVisualEdge> display) {
            if (display == Display2)
                return Display1;
            else
                return Display2;
        }

        protected virtual void SceneFocusChanged(object sender, GraphSceneEventArgs<IVisual, IVisualEdge> e) {

            if (ViewMode != SplitViewMode.GraphStream)
                return;
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            CurrentDisplay = display;
            lock (locker) {
                var adjacent = AdjacentDisplay(display);
                var contentViewManager = this.ContentViewManager;
                try {
                    display.ActionDispatcher.UserEventsDisabled = true;
                    adjacent.ActionDispatcher.UserEventsDisabled = true;
                    contentViewManager.SheetViewer = adjacent;
                    contentViewManager.ShowViewer(sender, e);
                } catch (Exception ex) {
                    ExceptionHandler.Catch(ex, MessageType.OK);
                } finally {
                    display.ActionDispatcher.UserEventsDisabled = false;
                    adjacent.ActionDispatcher.UserEventsDisabled = false;
                }
            }
        }

        #endregion

        #region SheetManagement

        public ISheetManager SheetManager {get;set;}

        public void SaveDocument() {
            if (CurrentWidget == CurrentDisplay) {
                var display = this.CurrentDisplay;
                if (SheetManager.IsSaveable(display.Data)) {
                    var info = display.Info;
                    Backend.ShowTextDialog("Sheet:", info.Name, SaveSheet);
                }
            } else {
                this.ContentViewManager.SaveStream(CurrentDisplay.Data.Graph.ThingGraph());
            }
        }

        public void SaveSheet(string name) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                var info = currentDisplay.Info;
                if(!string.IsNullOrEmpty(info.Name) && info.Name!=name) {
                    var dialog = Registry.Factory.Create<IMessageBoxShow>();
                    if (dialog.Show("Question",string.Format("Do you want to copy sheet {0} as {1}", info.Name,name),
                        MessageBoxButtons.YesNo)== DialogResult.Yes) {
                            info = SheetManager.RegisterSheet(0, name);
                    }
                }
                if (string.IsNullOrEmpty(info.Name))
                    info.Name = name;

                SheetManager.SaveInGraph(currentDisplay.Data, currentDisplay.Layout, info);
                currentDisplay.Info = info;
                FavoriteManager.AddToSheets(currentDisplay.Data.Graph, currentDisplay.DataId);
            }
        }

        public void NewSheet() {
            var currentDisplay = this.CurrentDisplay;
            VisualsDisplayHistory.Store(currentDisplay, SheetManager);
            var info = SheetManager.CreateSheet(currentDisplay.Data);
            currentDisplay.Info = info;
            currentDisplay.BackendRenderer.Render();
            OnViewChanged();
        }

        #endregion

        #region Search

        public void Search (string name) {
            var currentDisplay = this.CurrentDisplay;
            VisualsDisplayHistory.Store(currentDisplay, SheetManager);
            new VisualThingSearch()
                .LoadSearch(currentDisplay.Data, currentDisplay.Layout, name);
            currentDisplay.DataId = 0;
            new State {Hollow = true}.CopyTo(currentDisplay.State);
            currentDisplay.Text = name;
            currentDisplay.Viewport.Reset();
            currentDisplay.Perform();
            currentDisplay.Backend.Invalidate();
            OnViewChanged();
        }

        public void DoSearch() {
            var currentDisplay = this.CurrentDisplay;
            if (new VisualThingSearch().IsSearchable(currentDisplay.Data)) {
                Backend.ShowTextDialog("Search:", "", this.Search);
            }
        }

        #endregion

        #region History


        public VisualsDisplayHistory VisualsDisplayHistory { get; set; }
        
        private void Clear() {
            if (VisualsDisplayHistory != null) {
                VisualsDisplayHistory.Clear ();
            }

            if (SheetManager != null) {
                SheetManager.Clear();
            }

            ContentViewManager.Clear ();

            Display1.DataId = 0;
            Display1.Text = string.Empty;
            Display2.DataId = 0;
            Display2.Text = string.Empty;
        }

        #endregion 

        #region Navigating

        public void GoHome() {
            var display = this.CurrentDisplay;
            if (display != null) {
                Trace.WriteLine(string.Format("Before Home\tId\t{0}\tName\t{1}",display.Info.Id,display.Info.Name));
                VisualsDisplayHistory.Store(display, SheetManager);
               
                FavoriteManager.GoHome(display, false);
                OnViewChanged();
                Trace.WriteLine(string.Format("After Home\tId\t{0}\tName\t{1}", display.Info.Id, display.Info.Name));
            }
        }

        public bool CanGoBackOrForward(bool forward) {
            if (VisualsDisplayHistory == null)
                return false;

            var currentDisplay = this.CurrentDisplay;
            var currentControl = this.CurrentWidget;
            if (currentControl == currentDisplay && currentDisplay != null) {
                if (forward)
                    return VisualsDisplayHistory.CanGoForward();
                else
                    return VisualsDisplayHistory.CanGoBack();
            } else if (currentControl is IHistoryAware) {
                if (forward)
                    return ((IHistoryAware)currentControl).CanGoForward;
                else
                    return ((IHistoryAware)currentControl).CanGoBack;
            }
            return false;
        }

        public void LoadSheet(SceneInfo info) {
            if (info == null )
                return;
            info = SheetManager.GetSheetInfo(info.Id);
            var display = this.CurrentDisplay;
            if (info != null) {
                VisualsDisplayHistory.Store(display, SheetManager);
                if (SheetManager.Load(display.Data, display.Layout, info.Id)) {
                    info = SheetManager.GetSheetInfo(info.Id);
                    display.Info = info;
                    display.Viewport.Reset();
                    display.BackendRenderer.Render();
                }
            }
        }

        public void GoBackOrForward(bool forward) {
            var currentDisplay = this.CurrentDisplay;
            var currentControl = this.CurrentWidget;
            if (currentControl == currentDisplay && currentDisplay != null) {
                VisualsDisplayHistory.Navigate(currentDisplay, SheetManager, forward);
            } else if (currentControl is IHistoryAware) {
                if (forward)
                    ((IHistoryAware)currentControl).GoForward();
                else
                    ((IHistoryAware)currentControl).GoBack();

            }
            OnViewChanged();
        }

        #endregion

        #region Favorites

        public FavoriteManager FavoriteManager {get;set;}

        public void AddFocusedToFavorites() {
            FavoriteManager.AddToFavorites(CurrentDisplay.Data);
        }

        public void ViewOnOpen() {
            FavoriteManager.SetAutoView(CurrentDisplay.Data);
        }

        #endregion

        #region Notes

        public void NewNote() {
            Backend.ShowTextDialog("Note:", "new note", CreateNewNote);
        }

        public void CreateNewNote(string title) {
            var currentDiplay = CurrentDisplay;
            if (currentDiplay == null)
                return;
            var scene = currentDiplay.Data;
            if (scene == null)
                return;

            var content = new Content<Stream>(
                new MemoryStream(), CompressionType.bZip2, ContentTypes.RTF);

            content.Description = title;

            var writer = new StreamWriter(content.Data);

            writer.Write(@"{\rtf1\ansi\deff0");
            writer.Write(@"{\info{\doccomm limada.note}}");

            writer.Write(@"{\fonttbl{\f0\froman Times New Roman;}}");
            writer.Write(@"\pard\plain ");
            writer.Write(title);
            writer.Write(@"}");
            writer.Flush();
            content.Data.Position = 0;


            var visual = Registry.Pooled<IVisualContentViz>().VisualOfContent(scene.Graph, content);
            var root = scene.Focused;

            var layout = currentDiplay.Layout;

            if (root == null) {
                var pt = new Point(layout.Border.Width, scene.Shape.BoundsRect.Bottom);
                SceneExtensions.AddItem(scene, visual, layout, pt);
            } else {
                SceneExtensions.PlaceVisual(scene, root, visual, layout);
            }
            scene.Selected.Clear();
            scene.Focused = visual;
            currentDiplay.Perform();
            currentDiplay.OnSceneFocusChanged();
        }
        #endregion

        public void DoDisplayStyleChanged(object sender, EventArgs<IStyle> arg) {
            if (Display1 != null) {
                Display1.BackendRenderer.Render();
            }
            if (Display2 != null) {
                Display2.BackendRenderer.Render();
            }
        }

        public override void Dispose () {
            Clear();

            if (_contentViewManager != null)
                this.ContentViewManager.Dispose();

            Mesh.RemoveDisplay (Display1);
            Mesh.RemoveDisplay (Display2);

            Display1.Dispose();
            Display1 = null;
            Display2.Dispose();
            Display2 = null;


        }

        public virtual bool Check() {
            if (this.VisualsDisplayHistory == null) {
                throw new CheckFailedException(this.GetType(), typeof(VisualsDisplayHistory));
            }
            if (this.SheetManager == null) {
                throw new CheckFailedException(this.GetType(), typeof(SheetManager));
            }
            if (this.Display1 == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphSceneDisplay<IVisual, IVisualEdge>));
            }
            if (this.Display2 == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphSceneDisplay<IVisual, IVisualEdge>));
            }
            if (this.Backend == null) {
                throw new CheckFailedException(this.GetType()+"needs a Backend");
            }
            
            Display1.Check ();
            Display2.Check ();

            return true;
        }


    }
}