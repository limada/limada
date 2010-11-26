using System;
using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.UI;
using Limaki.Widgets;
using Limaki.Widgets.UI;

namespace Limaki.Winform.Widgets {
    public class AddEdgeAction : AddEdgeActionBase, IDragDropAction {
        public AddEdgeAction(Func<Scene> sceneHandler, IControl control, ICamera camera, ILayout<Scene, IWidget> layout):
            base(sceneHandler,control,camera,layout) {}

        #region IDragDropAction Member
        public void OnGiveFeedback(GiveFeedbackEventArgs e) { }

        public void OnQueryContinueDrag(QueryContinueDragEventArgs e) {
            if (Resolved) {
                e.Action = DragAction.Cancel;
                Dragging = false;
            }
        }

        public void OnDragOver(DragEventArgs e) {
            //Dragging = !Resolved; 
        }

        public void OnDragDrop(DragEventArgs e) { }
        public void OnDragLeave(EventArgs e) { }

        #endregion
    }
}