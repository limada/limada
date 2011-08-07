using Limaki.Drawing;
using Limaki.Presenter.Display;
using Limaki.Presenter.Layout;
using Limaki.Visuals;
using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Presenter.UI;

namespace Limaki.UseCases {
    public class AlignTools {
        

        public void Call(IGraphSceneDisplay<IVisual, IVisualEdge> display,  Action<Alligner<IVisual, IVisualEdge>,IEnumerable<IVisual>> call){
            var alligner = new Alligner<IVisual, IVisualEdge>(display.Data, display.Layout);
            var items = display.Data.Selected.Elements;
            call(alligner,items);
            StoreUndo(display, alligner, items);
            alligner.Proxy.Commit(alligner.Data);
            display.Execute();
        }
        
        private List<ICommand<IVisual>> _undo;
        private Int64 _undoID = 0;
        private void StoreUndo(IGraphSceneDisplay<IVisual, IVisualEdge> display, Alligner<IVisual, IVisualEdge> alligner, IEnumerable<IVisual> items) {
            _undo = new List<ICommand<IVisual>>();
            _undoID = display.DataId;
            foreach(var item in items) {
                _undo.Add(new MoveCommand<IVisual>(item, i => i.Shape, item.Location));
            }
            foreach (var edge in alligner.Proxy.AffectedEdges) {
                _undo.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
            }
        }

        public void Undo(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            if(_undo!=null&&_undoID==display.DataId) {
                foreach (var comm in _undo)
                    display.Data.Requests.Add(comm);
                display.Execute();
                _undo = null;
                _undoID = 0;
            }
        }

        public void AlignHorizontal(IGraphSceneDisplay<IVisual, IVisualEdge> display, HorizontalAlignment alignment) {
            Call(display, (alligner,items) => alligner.Allign(items, alignment));
        }

        public void Distribute(IGraphSceneDisplay<IVisual, IVisualEdge> display, VerticalAlignment alignment) {
            Call(display, (alligner,items) => alligner.Distribute(items, alignment));
        }
    }
}