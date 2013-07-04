using System;
using Xwt;

namespace Limaki.View.Ui.DragDrop1 {

    public class DragEventArgs : EventArgs {
        public DragEventArgs (Point position, TransferDataSource data, DragDropAction action) {
            Data = data;
            Position = position;
            Action = action;
            Success = false;
        }

        public TransferDataSource Data { get; private set; }

        public Point Position { get; private set; }

        public DragDropAction Action { get; private set; }

        public bool Success { get; set; }
    }
}