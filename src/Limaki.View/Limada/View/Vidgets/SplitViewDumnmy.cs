using System;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;

namespace Limada.View.Vidgets {

	public class SplitViewDumnmy : ISplitView {

		public void ChangeData (IGraphScene<IVisual, IVisualEdge> scene) { }

		public IGraphSceneDisplay<IVisual, IVisualEdge> CurrentDisplay { get { return null; } }
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display1 { get { return null; } }
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display2 { get { return null; } }

        public event EventHandler ViewChanged;
        public virtual void ToggleView() { }

        SplitViewMode _viewMode = SplitViewMode.GraphStream;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphStream)
                        this.SetGraphStreamView();
                    else if (value == SplitViewMode.GraphGraph)
                        this.SetGraphGraphView();
                }
                _viewMode = value;
            }
        }

        public virtual void SetGraphGraphView() { }
        public virtual void SetGraphStreamView() { }

        public virtual bool CanGoBackOrForward(bool forward) {
            return false;
        }

        public virtual void GoHome() { }
        public virtual void GoBackOrForward(bool forward) { }
        public virtual void NewSheet() { }
        public virtual void NewNote() { }
        public virtual void SaveDocument() { }
        public virtual void LoadSheet(SceneInfo info) { }
        public virtual IGraphSceneDisplay<IVisual, IVisualEdge> AdjacentDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            return null;
        }

        public void ShowInNewWindow () {}


        public void Search (string name) {
           
        }

        public void DoSearch () {
           
        }

    }
}