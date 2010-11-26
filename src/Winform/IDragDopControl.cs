using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
namespace Limaki.Winform {
    public interface IDragDopControl:IWinControl {
        bool AllowDrop { get; set; }
        DragDropEffects DoDragDrop(Object data,DragDropEffects allowedEffects);
    }
    public interface IDragDropAction:IAction {
        bool Dragging { get;set; }
        void OnGiveFeedback( GiveFeedbackEventArgs e );
        void OnQueryContinueDrag( QueryContinueDragEventArgs e );
        void OnDragOver( DragEventArgs e );
        void OnDragDrop( DragEventArgs e );
        void OnDragLeave( EventArgs e );
    }
}
