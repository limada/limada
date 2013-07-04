using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.View.UI;

namespace Limaki.View.Ui.DragDrop1 {

    public class DragDropEventControler : EventControler, IDropAction {

        public List<IDropAction> DragDropActions = new List<IDropAction>();

        public override void Add(IAction action) {
            base.Add(action);

            if (action is IDropAction) {
                DragDropActions.Add((IDropAction) action);
                DragDropActions.Sort(ActionsSort);
            }

        }

        public override void Remove(IAction action) {
            base.Remove(action);

            if (action is IDropAction) {
                DragDropActions.Remove((IDropAction) action);
            }

        }

        public virtual bool Dragging { get; set; }

        public virtual void DragOver (DragOverEventArgs e) {
            Resolved = false;
            foreach (var dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.DragOver(e);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive) {//} || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            Execute();
        }

        public virtual void OnDrop (DragEventArgs e) {
            Resolved = false;
            foreach (var dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.OnDrop(e);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive) {//} || ! dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            Execute();
        }

        public virtual void DragLeave (EventArgs e) {
            Resolved = false;
            foreach (var dragDropAction in DragDropActions) {
                if (dragDropAction.Enabled) {
                    dragDropAction.DragLeave(e);
                    Resolved = dragDropAction.Resolved || Resolved;
                    if (dragDropAction.Exclusive || !dragDropAction.Dragging) {
                        break;
                    }
                }
            }
            Execute();
        }
    }
}