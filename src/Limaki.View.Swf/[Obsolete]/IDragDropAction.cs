using System;
using System.Windows.Forms;
using Limaki.Actions;

namespace Limaki.View.Swf {
    public interface IDragDropAction:IAction, Limaki.View.UI.ICopyPasteAction {
        bool Dragging { get;set; }
        void OnGiveFeedback( GiveFeedbackEventArgs e );
        void OnQueryContinueDrag( QueryContinueDragEventArgs e );
        void OnDragOver( DragEventArgs e );
        void OnDragDrop( DragEventArgs e );
        void OnDragLeave( EventArgs e );
    }
}