using System;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.View.Viz.UI;

namespace Limaki.View.Swf {
    public interface IDragDropAction:IAction, ICopyPasteAction {
        bool Dragging { get;set; }
        void OnGiveFeedback( GiveFeedbackEventArgs e );
        void OnQueryContinueDrag( QueryContinueDragEventArgs e );
        void OnDragOver( DragEventArgs e );
        void OnDragDrop( DragEventArgs e );
        void OnDragLeave( EventArgs e );
    }
}