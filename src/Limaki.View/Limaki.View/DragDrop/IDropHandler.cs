using System;

namespace Limaki.View.DragDrop {
    public interface IDropHandler {
        void DragOver (DragOverEventArgs args);
        void OnDrop (DragEventArgs args);
        /// <summary>
        /// dragging leaves the widget; opposite of DragOver
        /// </summary>
        /// <param name="e"></param>
        void DragLeave (EventArgs e);

        
    }
}