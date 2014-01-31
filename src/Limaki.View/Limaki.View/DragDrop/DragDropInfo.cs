using Xwt;

namespace Limaki.View.DragDrop {
    public class DragDropInfo {
        // Source
        public bool AutodetectDrag { get; set; }
        public Rectangle DragRect { get; set; }
        // Target
        public TransferDataType[] TargetTypes = new TransferDataType[0];

        public static TransferDataType[] SourceTypes { get; set; }
    }
}