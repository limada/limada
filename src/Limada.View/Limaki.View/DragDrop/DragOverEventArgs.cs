using System;
using Xwt;

namespace Limaki.View.DragDrop {
    public interface ITransferDataInfo {
        bool HasType (TransferDataType typeId);
    }
	
    public class DragOverEventArgs : EventArgs {
        public DragOverEventArgs (Point position, ITransferData info, DragDropAction action) {
            Position = position;
            Info = info;
            Action = action;
            AllowedAction = DragDropAction.Default;
        }

        public ITransferData Info { get; private set; }

        /// <summary>
        /// Drop coordinates (in widget coordinates)
        /// </summary>
        public Point Position { get; private set; }

        /// <summary>
        /// Proposed drop action, which depends on the control keys being pressed
        /// </summary>
        public DragDropAction Action { get; private set; }

        /// <summary>
        /// Allowed action
        /// </summary>
        /// <remarks>
        /// To be set by the handler of the event. Specifies the action that will be performed if the item is dropped.
        /// If not specified or set to Default, the action will be determined by the handler of DragDropCheck.
        /// </remarks>
        public DragDropAction AllowedAction { get; set; }
    }
}