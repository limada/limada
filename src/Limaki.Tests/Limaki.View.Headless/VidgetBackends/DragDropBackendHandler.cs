using Limaki.View.DragDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Limaki.View.Headless.VidgetBackends {
    
    public class DragDropBackendHandler : DragDropBackendHandlerBase, IDragDropBackendHandler {

        public IVidgetBackend Backend { get; set; }
        public DragDropBackendHandler (IVidgetBackend backend) {
            this.Backend = backend;
        }



        protected override Xwt.Backends.DragStartData GetDragStartData () {
            throw new NotImplementedException ();
        }

        public override void DragStart (Xwt.Backends.DragStartData data) {
            
        }

        public override void SetDragTarget (Xwt.DragDropAction dragAction, params Xwt.TransferDataType[] types) {
            
        }
    }
}
