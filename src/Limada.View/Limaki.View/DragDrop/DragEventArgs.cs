using System;
using Xwt;

namespace Limaki.View.DragDrop {

    public class DragEventArgs : EventArgs {
        public DragEventArgs (Point position, ITransferData data, DragDropAction action) {
            Data = data;
            Position = position;
            Action = action;
            Success = false;
        }

        public ITransferData Data { get; private set; }

        public Point Position { get; private set; }

        public DragDropAction Action { get; private set; }

        public bool Success { get; set; }
    }
}