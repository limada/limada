using System;

namespace Limaki.Presenter.UI.DragDrop {
    public class QueryContinueDragEventArgs : EventArgs {

        public QueryContinueDragEventArgs(DragDropKeyStates keyState, bool escapePressed, DragAction action) {
            this.KeyState = keyState;
            this.EscapePressed = escapePressed;
            this.Action = action;
        }
        
        public DragAction Action {get;set;}
        public bool EscapePressed { get; internal set; }
        public DragDropKeyStates KeyState { get; internal set; }
    }
}