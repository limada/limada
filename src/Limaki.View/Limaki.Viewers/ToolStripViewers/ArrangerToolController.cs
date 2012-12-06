using System;
using System.Collections.Generic;
using Limaki.Actions;
using System.Linq;
using Limaki.Graphs.Extensions;
using Limaki.View.Visualizers;
using Limaki.View.Layout;
using Limaki.View.UI.GraphScene;
using Limaki.Visuals;
using System.Diagnostics;

namespace Limaki.Viewers.ToolStripViewers {

    public interface IArrangerTool {}

    public class ArrangerToolController : ToolController<IGraphSceneDisplay<IVisual, IVisualEdge>, IArrangerTool> {

        public virtual void Attach(object sender, EventArgs e) {
            Attach(sender);
        }

        public override void Detach(object sender) {
            
        }

        public override void Attach(object sender) {
            base.Attach(sender);
        }

        //public void Call(IGraphSceneDisplay<IVisual, IVisualEdge> display, Action<Aligner<IVisual, IVisualEdge>> call) {
        //    if (display == null)
        //        return;

        //    var aligner = new Aligner<IVisual, IVisualEdge>(display.Data, display.Layout);
        //    var items = display.Data.Selected.Elements;
        //    call(aligner);
        //    StoreUndo(display, aligner, items);
        //    aligner.Proxy.Commit(aligner.Data);
        //    display.Execute();
        //}

        public void Call(IGraphSceneDisplay<IVisual, IVisualEdge> display, Action<Aligner<IVisual, IVisualEdge>, IEnumerable<IVisual>> call) {
            if (display == null)
                return;

            Call(display, call, display.Data.Selected.Elements);
        }

        public void Call(IGraphSceneDisplay<IVisual, IVisualEdge> display, Action<Aligner<IVisual, IVisualEdge>, IEnumerable<IVisual>> call, IEnumerable<IVisual> items) {
            if (display == null)
                return;

            var aligner = new Aligner<IVisual, IVisualEdge>(display.Data, display.Layout);

            call(aligner, items);
            
            aligner.Locator.Commit (aligner.GraphScene.Requests);

            StoreUndo (display, aligner, items);

            display.Execute();
        }

        private List<ICommand<IVisual>> _undo;
        private Int64 _undoID = 0;
        protected virtual void StoreUndo(IGraphSceneDisplay<IVisual, IVisualEdge> display, Aligner<IVisual, IVisualEdge> aligner, IEnumerable<IVisual> items) {
            _undo = new List<ICommand<IVisual>>();
            _undoID = display.DataId;
            foreach (var item in aligner.GraphScene.Requests.Select(c=>c.Subject)) {
                _undo.Add(new MoveCommand<IVisual>(item, i => i.Shape, item.Location));
            }
            foreach (var edge in aligner.Locator.AffectedEdges) {
                _undo.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
            }
        }

        public virtual void Undo(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            if (display == null)
                return;
            if (_undo != null && _undoID == display.DataId) {
                foreach (var comm in _undo)
                    display.Data.Requests.Add(comm);
                display.Execute();
                _undo = null;
                _undoID = 0;
            }
        }

        public virtual void Undo() {
            Undo(CurrentDisplay);
        }
       
        public virtual void Columns(AlignerOptions options) {
            options.Distance = CurrentDisplay.Layout.Distance;
            Call(CurrentDisplay, (aligner, items) => aligner.Columns(items, options));
        }
        public void OneColumn(AlignerOptions options) {
            options.Distance = CurrentDisplay.Layout.Distance;
            Call(CurrentDisplay, (aligner, items) => aligner.OneColumn(items, options));
        }
        public virtual void FullLayout(AlignerOptions options) {
            options.Distance = CurrentDisplay.Layout.Distance;
            var display = this.CurrentDisplay;
            if (display != null) {
                display.BackColor = display.StyleSheet.BackColor;
                display.Invoke();
                display.BackendRenderer.Render();
            }
        }

        public virtual void LogicalLayout(AlignerOptions options) {
            options.Distance = CurrentDisplay.Layout.Distance;
            var display = this.CurrentDisplay;
            if (display != null) {
                var selected = display.Data.Selected.Elements;
                var root = display.Data.Focused;
                if (selected.Count() == 1) {
                    selected = new Walker<IVisual, IVisualEdge>(display.Data.Graph).DeepWalk(root, 0).Select(l => l.Node);
                }
                Call(CurrentDisplay, (aligner, items) => aligner.Columns(root, items, options), selected);
            }
        }
        
    }
}