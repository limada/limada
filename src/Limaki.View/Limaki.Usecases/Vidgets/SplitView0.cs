/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2017 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Usecases.Vidgets;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Limaki.View.Viz.UI.GraphScene;
using Limaki.View.Viz.Visuals;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.Vidgets {

    [BackendType(typeof(ISplitViewBackend))]
    public class SplitView0 : Vidget, ISplitView, ICheckable {

        public SplitView0 () {
            Compose();
        }

        public new ISplitViewBackend Backend { get { return BackendHost.Backend as ISplitViewBackend; } }

        #region Initialize

        public virtual void Compose () {
            Display1 = new VisualsDisplay();
            Display2 = new VisualsDisplay();

            InitializeDisplay(Display1);
            InitializeDisplay(Display2);

            CurrentDisplay = Display1;
        }

        IGraphSceneDisplayMesh<IVisual, IVisualEdge> _mesh = null;
        public IGraphSceneDisplayMesh<IVisual, IVisualEdge> Mesh { get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneDisplayMesh<IVisual, IVisualEdge>>()); } }

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
            
            AttachVidget (display);

            Mesh.AddDisplay (display);

        }

		public void SetScene (IGraphScene<IVisual, IVisualEdge> scene, string name) {
		    var display = Display1;

			if (name.ToLower().EndsWith (nameof (Display1).ToLower())) {
			    display = Display1;
			}
            else if (name.ToLower ().EndsWith (nameof (Display2).ToLower ())) {
                display = Display2;
            }
            else {
                return;
            }

		    display.Data = scene;
            display.BackendRenderer.Render ();

        }

        #endregion 

        public IGraphSceneDisplay<IVisual, IVisualEdge> Display1 { get; set; }
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display2 { get; set; }

        #region View-Switching
        SplitViewMode _viewMode = SplitViewMode.GraphContent;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphContent)
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
                CurrentVidget = value;
            }
        }

        private object locker = new object();

        IVidget _currentVidget = null;
        public IVidget CurrentVidget  {
            get { return _currentVidget; }
            protected set {
                lock (locker) {
                    bool isChange = _currentVidget != value;
                    var display = value as IGraphSceneDisplay<IVisual, IVisualEdge>;
                    if (display != null || value == null) {
                        //isChange = _currentDisplay != value;
                        _currentDisplay = display;
                    }
                    _currentVidget = value;
                    if (isChange) {
                        OnCurrentVidgetChanged(value);
                        OnViewChanged();
                    }
                }
            }
        }

        protected void AttachVidget (IVidget vidget) {
            vidget.GotFocus -= VidgetGotFocus;
            vidget.ButtonReleased -= VidgetGotFocus;
            vidget.GotFocus += VidgetGotFocus;
            vidget.ButtonReleased += VidgetGotFocus;
        }

        protected void DetachVidget (IVidget vidget) { 
            vidget.GotFocus -= VidgetGotFocus;
            vidget.ButtonReleased -= VidgetGotFocus;
        }

        protected void VidgetGotFocus (object sender, EventArgs e) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null) {
                CurrentDisplay = display;
                return;
            }
            var vidget = sender as IVidget;
            if (vidget != null) {
                CurrentVidget = vidget;
            }
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
            if (CurrentVidget != CurrentDisplay) {
                CurrentDisplay = AdjacentDisplay (CurrentDisplay);
            }
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
        protected void OnViewChanged() {
            ViewChanged?.Invoke(this,new EventArgs());
        }

        public event Action<IVidget> CurrentVidgetChanged = null;
        protected void OnCurrentVidgetChanged (IVidget vidget) {
            CurrentVidgetChanged?.Invoke (vidget);
        }

        #endregion

        public void ShowInNewWindow () {

            var source = CurrentDisplay ?? this.Display1;
            if (source == null)
                return;

            var graph = source.Data.Graph;
            var focused = source.Data.Focused;

            IVidget vidget = null;
            Action onClose = null;

            if (graph != null && focused != null) {
                using (var contentViewManager = Registry.Create<IContentViewManager> ()) {
                    contentViewManager.IsProviderOwner = false;
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
				vidget = display;
            }

            Backend.ViewInWindow (vidget, onClose);
        }

        public void ChangeData() {
            
			IList<IGraphSceneDisplay<IVisual, IVisualEdge>> displays = new IGraphSceneDisplay<IVisual, IVisualEdge>[] { Display2 };

            Clear ();

            CurrentDisplay = null;

            displays
                .Where (d => d != Display1)
                .ForEach (d => {
                    Mesh.CopyDisplayProperties (Display1, d);
                });

            FavoriteManager.GoHome (Display1, true);
            GraphGraphView();
            GraphContentView();

            CurrentDisplay = Display1;
        }

        #region ContentView

        public IVidget ContentVidget { get; protected set; }

        private IContentViewManager _contentViewManager = null;
        public IContentViewManager ContentViewManager {
            get {
                if (_contentViewManager == null) {
                    _contentViewManager = Registry.Create<IContentViewManager>();
                }
                
                _contentViewManager.BackColor = Display1.BackColor;

                _contentViewManager.AttachCurrentViewer = v => {
                    Backend.AttachViewer (v.Frontend, () => v.OnShow ());
                    ContentVidget = v.Frontend;
                    AttachVidget (v.Frontend);
                };
                _contentViewManager.DetachCurrentViewer = v => DetachVidget (v.Frontend);

                _contentViewManager.SceneManager = this.SceneManager;

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

            if (ViewMode != SplitViewMode.GraphContent)
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

        public ISceneManager SceneManager { get; set; }

        public void SaveDocument() {
            if (CurrentVidget == CurrentDisplay) {
                var display = this.CurrentDisplay;
				if (SceneManager.IsSaveable(display.Data)){
					var info = display.Info;
					Backend.ShowTextDialog("Sheet:", info.Name, SaveSheet);
				}
            } else {
                this.ContentViewManager.SaveStream (CurrentDisplay.Data.Graph, ContentViewManager.CurrentViewer as ContentStreamViewer);
            }
        }

        public void SaveSheet(string name) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                var info = currentDisplay.Info;
                if(!string.IsNullOrEmpty(info.Name) && info.Name!=name) {
                    var dialog = Registry.Factory.Create<IMessageBoxShow>();
                    if (dialog.Show("Question",string.Format($"Do you want to copy sheet {info.Name} as {name}"),
                        MessageBoxButtons.YesNo)== DialogResult.Yes) {
                        info = SceneManager.SheetStore.CreateSceneInfo ();
                        info.Name = name;
                    }
                }
                if (string.IsNullOrEmpty(info.Name))
                    info.Name = name;

                SceneManager.SaveInGraph(currentDisplay.Data, currentDisplay.Layout, info);
                currentDisplay.Info = info;
                FavoriteManager.AddToSheets(currentDisplay.Data.Graph, currentDisplay.DataId);
            }
        }

        public void NewSheet() {
            var currentDisplay = this.CurrentDisplay;
            VisualsDisplayHistory.Store(currentDisplay, SceneManager);
            currentDisplay.Data.CleanScene ();
            var info = SceneManager.SheetStore.CreateSceneInfo ();
            currentDisplay.Info = info;
            currentDisplay.BackendRenderer.Render();
            OnViewChanged();
        }

        #endregion

        #region Search
        IVisualGraphSceneSearch _visualGraphSceneSearch = null;
        IVisualGraphSceneSearch VisualGraphSceneSearch { 
            get { return _visualGraphSceneSearch ?? (_visualGraphSceneSearch = Registry.Create<IVisualGraphSceneSearch>()); } 
        }

        public void Search (string name) {
            var currentDisplay = this.CurrentDisplay;
            VisualsDisplayHistory.Store(currentDisplay, SceneManager);
            VisualGraphSceneSearch.LoadSearch(currentDisplay.Data, currentDisplay.Layout, name);
            var info = SceneManager.SheetStore.CreateSceneInfo ();
            info.Name = name;
            currentDisplay.Info = info;
            currentDisplay.Viewport.Reset();
            currentDisplay.Perform();
            currentDisplay.QueueDraw();
            OnViewChanged();
        }

        public void DoSearch() {
            var currentDisplay = this.CurrentDisplay;
            if (VisualGraphSceneSearch.IsSearchable(currentDisplay.Data)) {
                Backend.ShowTextDialog("Search:", "", this.Search);
            }
        }

        #endregion

        #region History
        
        public VisualsDisplayHistory VisualsDisplayHistory { get; set; }
        
        private void Clear() {
            
            VisualsDisplayHistory?.Clear ();

            SceneManager?.Clear();

            ContentViewManager.Clear ();

			Display1.Clear ();
			Display2.Clear ();
            
        }

        #endregion 

        #region Navigating

        public void GoHome() {
            var display = this.CurrentDisplay;
            if (display != null) {
                Trace.WriteLine($"Before {nameof(ISplitView)}.{nameof(GoHome)}\t{display.Info}");
                VisualsDisplayHistory.Store(display, SceneManager);
               
                FavoriteManager.GoHome(display, false);
                OnViewChanged();
                Trace.WriteLine ($"After {nameof (ISplitView)}.{nameof (GoHome)}\t{display.Info}");
            }
        }

        public bool CanGoBackOrForward(bool forward) {
            if (VisualsDisplayHistory == null)
                return false;

            var currentDisplay = this.CurrentDisplay;
            var currentVidget = this.CurrentVidget;
            if (currentVidget == currentDisplay && currentDisplay != null) {
                if (forward)
                    return VisualsDisplayHistory.CanGoForward();
                else
                    return VisualsDisplayHistory.CanGoBack();
            } else if (currentVidget is IHistoryAware) {
                if (forward)
                    return ((IHistoryAware)currentVidget).CanGoForward;
                else
                    return ((IHistoryAware)currentVidget).CanGoBack;
            }
            return false;
        }

        public void LoadSheet(SceneInfo info) {
            if (info == null )
                return;
            info = SceneManager.SheetStore.GetSheetInfo(info.Id);
            var display = this.CurrentDisplay;
            if (info != null) {
                VisualsDisplayHistory.Store(display, SceneManager);
                SceneManager.Load (display, info.Id);
            }
        }

        public void GoBackOrForward(bool forward) {
            var currentDisplay = this.CurrentDisplay;
            var currentVidget = this.CurrentVidget;
            if (currentVidget == currentDisplay && currentDisplay != null) {
                VisualsDisplayHistory.Navigate(currentDisplay, SceneManager, forward);
            } else if (currentVidget is IHistoryAware) {
                if (forward)
                    ((IHistoryAware)currentVidget).GoForward();
                else
                    ((IHistoryAware)currentVidget).GoBack();

            }
            OnViewChanged();
        }

        #endregion

        #region Favorites

        public IFavoriteManager FavoriteManager { get; set; }

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

            var content = new Content<Stream> (new MemoryStream (), CompressionType.bZip2);
            content.Description = title;

            if (Registry.Factory.Contains<IMarkdownEdit>()) {
                content.ContentType = ContentTypes.Markdown;
                var writer = new StreamWriter (content.Data);
                writer.Write (title);
                writer.Flush ();
                content.Data.Position = 0;
            } else {
                content.ContentType = ContentTypes.RTF;
                var writer = new StreamWriter (content.Data);

                writer.Write ("{\\rtf1\\ansi\\deff0");
                writer.Write ("{\\info{\\doccomm limada.note}}");

                writer.Write ($"{{\\fonttbl{{\\f0\\froman {Xwt.Drawing.Font.SystemSerifFont.Family}}}}}");
                writer.Write ("\\pard\\plain ");
                writer.Write (title);
                writer.Write ("}");
                writer.Flush ();
                content.Data.Position = 0;
            }

            var visual = scene.VisualOfContent (content);
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

            var md = ContentVidget as IMarkdownEdit;
            if (md != null)
                md.InEdit = true;
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

            if (_contentViewManager != null) {
                this.ContentViewManager.Dispose ();
            }
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

            if (this.SceneManager == null) {
                throw new CheckFailedException (this.GetType (), typeof (ISceneManager));
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