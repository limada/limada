using System;
using Limaki.Presenter.Display;
using Limaki.Visuals;
using System.Collections.Generic;
using Limaki.Actions;
using ID = System.Int64;
using Limaki.Presenter.Layout;
using Limaki.Presenter.UI;
using Limaki.Drawing;

namespace Limaki.UseCases.Viewers.ToolStripViewers {

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

        public void Call(IGraphSceneDisplay<IVisual, IVisualEdge> display, Action<Alligner<IVisual, IVisualEdge>, IEnumerable<IVisual>> call) {
            if (display == null)
                return;

            var alligner = new Alligner<IVisual, IVisualEdge>(display.Data, display.Layout);
            var items = display.Data.Selected.Elements;
            call(alligner, items);
            StoreUndo(display, alligner, items);
            alligner.Proxy.Commit(alligner.Data);
            display.Execute();
        }

        private List<ICommand<IVisual>> _undo;
        private ID _undoID = 0;
        protected virtual void StoreUndo(IGraphSceneDisplay<IVisual, IVisualEdge> display, Alligner<IVisual, IVisualEdge> alligner, IEnumerable<IVisual> items) {
            _undo = new List<ICommand<IVisual>>();
            _undoID = display.DataId;
            foreach (var item in items) {
                _undo.Add(new MoveCommand<IVisual>(item, i => i.Shape, item.Location));
            }
            foreach (var edge in alligner.Proxy.AffectedEdges) {
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
       
        public virtual void Columns(AllignerOptions options) {
            Call(CurrentDisplay, (alligner, items) => alligner.Columns(items, options));
        }
        public void OneColumn(AllignerOptions options) {
            Call(CurrentDisplay, (alligner, items) => alligner.OneColumn(items, options));
        }
        public virtual void LogicalLayout(AllignerOptions options) {
            var display = this.CurrentDisplay;
            if (display != null) {
                display.BackColor = display.StyleSheet.BackColor;
                display.Invoke();
                display.DeviceRenderer.Render();
            }
        }


        
    }
}