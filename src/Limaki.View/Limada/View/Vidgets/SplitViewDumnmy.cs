using System;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Usecases.Vidgets;
using Limaki.View;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Visualizers;
using Limada.View.ContentViewers;

namespace Limada.View.Vidgets {

	public class SplitViewDumnmy : Vidget, ISplitView {

		public void ChangeData () { }

		public IGraphSceneDisplay<IVisual, IVisualEdge> CurrentDisplay { get { return null; } }
	    public IVidget CurrentVidget { get { return null; } }
	    public IGraphSceneDisplay<IVisual, IVisualEdge> Display1 { get { return null; } }
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display2 { get { return null; } }

        public event EventHandler ViewChanged;
        public event Action<IVidget> CurrentVidgetChanged;
        public ContentViewManager ContentViewManager { get; set; }

        public virtual void ToggleView() { }

        SplitViewMode _viewMode = SplitViewMode.GraphContent;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphContent)
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

        public void DoDisplayStyleChanged (object sender, EventArgs<IStyle> arg) {
          
        }

        public bool Check () {
            return true;
        }

        public override void Dispose () {
           
        }

		public void SetScene (IGraphScene<IVisual, IVisualEdge> display, string name) {
		}

		public IVidget ContentVidget {
            get { return null; }
        }

        public VisualsDisplayHistory VisualsDisplayHistory { get; set; }
        public ISheetManager SheetManager { get; set; }
        public FavoriteManager FavoriteManager { get; set; }

    }
}