using System;
using System.Collections.Generic;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Drawing;
using Limaki.View;
using Limaki.View.ContentViewers;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Mesh;
using Xwt;

namespace Limada.UseCases {
    public class UsecasePersistor {

        private Dictionary<string, DisplayMemento> SavedDisps = new Dictionary<string, DisplayMemento> ();

        private IGraphSceneMesh<IVisual, IVisualEdge> _mesh = null;

        public IGraphSceneMesh<IVisual, IVisualEdge> Mesh {
            get { return _mesh ?? (_mesh = Registry.Pooled<IGraphSceneMesh<IVisual, IVisualEdge>> ()); }
        }

        private Int64 HomeId = 0;

        public virtual void Save (string tag, IGraphSceneDisplay<IVisual, IVisualEdge> display) {

            SavedDisps[tag] = new DisplayMemento (display);
            display.Data = null;
            Mesh.RemoveDisplay (display);
        }

        public virtual void Restore (string tag, IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            SavedDisps[tag].Restore (display);
            Mesh.AddDisplay (display);
        }

        public void Save (IConceptUsecase usecase) {

            if (usecase.SplitView == null || usecase.GraphSceneUiManager == null)
                return;

            HomeId = usecase.FavoriteManager.HomeId;
            var displays = Mesh.Displays;
            usecase.VisualsDisplayHistory.SaveChanges (displays, usecase.SheetManager, false);
            usecase.FavoriteManager.SaveChanges (displays);
            usecase.GraphSceneUiManager.Save ();

            Save ("SplitView.Display1", usecase.SplitView.Display1);
            Save ("SplitView.Display2", usecase.SplitView.Display2);

            ClearContentViewers ();

        }

        IList<Type> contentViewers = new List<Type> ();
        public void ClearContentViewers () {
            contentViewers.Clear ();
            var provider = Registry.Pooled<ContentViewerProvider> ();
            provider.Viewers.ForEach (viewer => contentViewers.Add (viewer.GetType ()));
            provider.Clear ();
        }

        public void Resume (IConceptUsecase usecase) {

            if (SavedDisps == null || usecase.FavoriteManager == null)
                return;

            // there could be a resume without compose (only called in start)
            if (Registry.Pooled<ContentViewerProvider> ().Viewers.Count () == 0)
                ComposeContentViewers ();

            if (SavedDisps.Count > 0) {
                Restore ("SplitView.Display1", usecase.SplitView.Display1);
                Restore ("SplitView.Display2", usecase.SplitView.Display2);
                if (usecase.SplitView.CurrentDisplay != null && usecase.SplitView.CurrentDisplay.Data.Focused != null) {
                    usecase.SplitView.CurrentDisplay.OnSceneFocusChanged ();
                }
            }
            usecase.FavoriteManager.HomeId = HomeId;
        }

        public void ComposeContentViewers () {
            var provider = Registry.Pooled<ContentViewerProvider> ();
            contentViewers.ForEach (type => provider.Add (Activator.CreateInstance (type) as ContentViewer));
        }
    }

    public class DisplayMemento {

        public IGraphScene<IVisual, IVisualEdge> Scene { get; set; }

        public SceneInfo Info { get; set; }

        public Point Offset { get; set; }

        public ZoomState ZoomState { get; set; }

        public double Zoom { get; set; }

        public State State { get; set; }

        public Int64 DataId { get; set; }

        public string Text { get; set; }

        public DisplayMemento (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            Save (display);
        }

        public void Save (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            Scene = display.Data;
            Info = display.Info;
            Offset = display.Viewport.ClipOrigin;
            Zoom = display.Viewport.ZoomFactor;
            ZoomState = display.Viewport.ZoomState;
            State = display.State;
            DataId = display.DataId;
            Text = display.Text;
        }

        public void Restore (IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            display.Data = Scene;
            display.DataId = DataId;
            display.Text = Text;

            display.Info = Info;
            this.State.CopyTo (display.State);

            display.Viewport.ClipOrigin = Offset;
            display.Viewport.ZoomState = ZoomState;
            if (ZoomState == Limaki.Drawing.ZoomState.Custom)
                display.Viewport.ZoomFactor = Zoom;
        }
    }
}