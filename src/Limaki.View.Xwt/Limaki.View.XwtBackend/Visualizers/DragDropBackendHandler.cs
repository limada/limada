using System;
using Limaki.View.DragDrop;
using Xwt;
using Xwt.Backends;
using DragEventArgs = Limaki.View.DragDrop.DragEventArgs;
using DragOverEventArgs = Limaki.View.DragDrop.DragOverEventArgs;

namespace Limaki.View.XwtBackend {
    public class DragDropBackendHandler : DragDropBackendHandlerBase {

        public DragDropBackendHandler (IVidgetBackend widget) {
            this.Widget = widget as Widget;
        }

        public Widget Widget { get; set; }

        protected override DragStartData GetDragStartData () {
            object imageBackend = GetDragImageBackend();
            var hot = Widget.MouseLocation();
            return new DragStartData(DragDataSource(), DragDropAction.All, imageBackend, hot.X,hot.Y);

        }

        protected override object GetDragImageBackend () {
            return null;
        }

        public override void DragStart (DragStartData data) {
            if (data.Data == null)
                throw new ArgumentNullException("data");

            Widget.DragStart(data);
        }

        public override void SetDragTarget (DragDropAction dragAction, params TransferDataType[] types) {
            DragDropInfo.TargetTypes = types == null ? new TransferDataType[0] : types;
            Widget.SetDragDropTarget(dragAction, types);
        }

        
    }
}