using System;
using Limaki.View.DragDrop;
using Limaki.View.Vidgets;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class DragDropMouseBackendHandler : DragDropBackendHandler, IDragDropMouseBackendHandler {

        public DragDropMouseBackendHandler (IVidgetBackend widget) : base(widget) { }

        public void MouseUp (MouseActionEventArgs e) {
            throw new NotImplementedException();
        }

        public void MouseMove (MouseActionEventArgs e) {
            throw new NotImplementedException();
        }

        public DragDropAction DragDropActionFromKeyState (int keyState, DragDropAction allowedEffect) {
            throw new NotImplementedException();
        }
    }
}