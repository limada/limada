using System.Linq;
using Limaki.View.DragDrop;
using Xwt;

namespace Limaki.View.SwfBackend.DragDrop {
    public class SwfTransferDataInfo : ITransferDataInfo {
        public SwfTransferDataInfo (System.Windows.Forms.IDataObject dob) {
            this.Dob = dob;
        }
        public bool HasType (TransferDataType typeId) { return Dob.GetFormats().Any(t => t.ToLower() == typeId.Id.ToLower()); }

        public System.Windows.Forms.IDataObject Dob { get; set; }
    }
}