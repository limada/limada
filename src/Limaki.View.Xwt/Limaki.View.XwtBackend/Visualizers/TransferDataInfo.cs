using Xwt;

namespace Limaki.View.XwtBackend {
    public class TransferDataInfo : Limaki.View.DragDrop.ITransferDataInfo {
        public TransferDataInfo (ITransferData dob) {
            this.Dob = dob;
        }
        public bool HasType (TransferDataType typeId) { return Dob.HasType(typeId); }

        public ITransferData Dob { get; set; }
    }
}