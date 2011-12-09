using System;
using System.Linq;
using System.Collections.Generic;
using Limaki.Common.Collections;
using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.UseCases.Viewers;
using Limaki.UseCases.Viewers.ToolStripViewers;
using Limaki.Visuals;

namespace Limaki.UseCases.Viewers.ToolStripViewers {
    

    public class SplitViewToolController : ToolController<IGraphSceneDisplay<IVisual, IVisualEdge>, ISplitViewTool> {
        ISplitView _splitView = null;
        public ISplitView SplitView {
            get {
                if (_splitView == null) {
                    _splitView = new SplitViewDumnmy();
                }
                return _splitView;
            }
            set {
                if (_splitView != value) {
                    if (_splitView != null) {
                        _splitView.ViewChanged -= this.Attach;
                    }
                    if (value != null) {
                        value.ViewChanged += this.Attach;
                    }
                }
                _splitView = value;
            }
        }

        public virtual SplitViewMode ViewMode {
            get { return Tool.ViewMode; }
            set { Tool.ViewMode = value; }
        }

        void CheckBackForward(ISplitView splitView) {
            Tool.CheckBackForward(splitView);
        }

        public ISheetManager SheetManager {get;set;}

        public virtual void Attach(object sender, EventArgs e) {
            Attach(sender);
        }

        public override void Detach(object sender) {
            
        }

        public override void Attach(object sender) {
            var display = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
            if (display != null)
                CurrentDisplay = display;

            if (SplitView != null) {
                CheckBackForward(SplitView);
                this.ViewMode = SplitView.ViewMode;
            }
            Tool.AttachSheets();
        }

        public virtual void GraphGraphView() {
            this.ViewMode = SplitViewMode.GraphGraph;
            SplitView.ViewMode = this.ViewMode;
        }

        public virtual void GraphStreamView() {
            this.ViewMode = SplitViewMode.GraphStream;
            SplitView.ViewMode = this.ViewMode;
        }

        public virtual void ToggleView() {
            SplitView.ToggleView();
        }

        public virtual void GoBack() {
            if (SplitView.CanGoBackOrForward(false)) {
                SplitView.GoBackOrForward(false);
                CheckBackForward(SplitView);
            }
        }

        public virtual void GoForward() {
            if (SplitView.CanGoBackOrForward(true)) {
                SplitView.GoBackOrForward(true);
                CheckBackForward(SplitView);
            }
        }

        public virtual void GoHome() {
            SplitView.GoHome();
        }

        public virtual void NewSheet() {
            SplitView.NewSheet();
        }

        public virtual void NewNote() {
            SplitView.NewNote();
        }

        public virtual void SaveDocument() {
            SplitView.SaveDocument();
        }

        public virtual string AttachSheets(IList<SceneInfo> _sheets) {
            string result = null;
            
            var display = CurrentDisplay;
            var dontCare = new Set<long>();

            if (display != null) {
                result = display.Info.Name;
                dontCare.Add(display.DataId);
                var adj = SplitView.AdjacentDisplay(display);
                if (adj != null)
                    dontCare.Add(adj.DataId);
            }
            _sheets.Clear();

            SheetManager.VisitRegisteredSheets(s => {
                if (SheetManager.StoreContains(s.Id) && !dontCare.Contains(s.Id))
                    _sheets.Add(s);
            });

            return result;

        }
        public void SelectSheet(SceneInfo info) {
            SplitView.LoadSheet(info);   
        }
    }

    public class SplitViewDumnmy : ISplitView {
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
    }
}