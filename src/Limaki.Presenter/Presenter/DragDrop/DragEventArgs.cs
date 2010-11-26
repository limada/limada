using System;

namespace Limaki.Presenter.UI.DragDrop {
    public class DragEventArgs : EventArgs {
        internal int x;
        internal int y;

        public DragEventArgs(IDataObject data, DragDropKeyStates keyState, int x, int y, DragDropEffects allowedEffect, DragDropEffects effect) {
            this.x = x;
            this.y = y;
            this.KeyState = keyState;
            this.AllowedEffect = allowedEffect;
            this.Effect = effect;
            this.Data = data;
        }
        
        public DragDropEffects AllowedEffect {get; internal set; }
        public IDataObject Data { get; internal set; }
        public DragDropEffects Effect { get; set; }
        public DragDropKeyStates KeyState { get; internal set; }
        public int X { get { return this.x; } }
        public int Y { get { return this.y; } }
    }
}