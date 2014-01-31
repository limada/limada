using System;
using Limaki.View.DragDrop;
using Xwt;
using Xwt.Backends;
using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.XwtBackend {
    public class DragDropBackendHandler : DragDropBackendHandlerBase {
        protected override DragStartData GetDragStartData () {
            throw new NotImplementedException();
        }

        public override void DragStart (DragStartData data) {
            throw new NotImplementedException();
        }

        public override void SetDragTarget (DragDropAction dragAction, params TransferDataType[] types) {
            throw new NotImplementedException();
        }
    }
}