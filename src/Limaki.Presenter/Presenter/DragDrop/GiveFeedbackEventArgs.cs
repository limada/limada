using System;

namespace Limaki.Presenter.UI.DragDrop {
    public class GiveFeedbackEventArgs : EventArgs {
        public GiveFeedbackEventArgs(DragDropEffects effect, bool useDefaultCursors) {
            this.Effect = effect;
            this.UseDefaultCursors = useDefaultCursors;
        }

        public DragDropEffects Effect { get; internal set; }
        public bool UseDefaultCursors { get; set; }
        
    }
}