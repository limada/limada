using System;
using System.Collections.Generic;
using System.Linq;
using Xwt;
using Xwt.SwfBackend;

namespace Limaki.View.SwfBackend.DragDrop {

    public class SwfTransferDataStore : ITransferData {
        public SwfTransferDataStore (System.Windows.Forms.IDataObject data) {
            this.Data = data;
        }
        public bool HasType (TransferDataType typeId) { return Data.GetFormats().Any(t => t.ToLower() == typeId.Id.ToLower()); }

        protected System.Windows.Forms.IDataObject Data { get; set; }

        public string Text {
            get { return ((System.Windows.Forms.DataObject) Data).GetText(); }
        }

        public Uri[] Uris {
            get { return ((System.Windows.Forms.DataObject) Data).GetFileDropList().OfType<string>().Select(s => new Uri(s)).ToArray(); }
        }

        public Xwt.Drawing.Image Image {
            get { return new Xwt.Drawing.Image(((System.Windows.Forms.DataObject) Data).GetImage()); }
        }

        public object GetValue (TransferDataType type) {
            return Data.GetData(type.ToSwf());
        }

        public T GetValue<T> () where T : class {
            var t = typeof (T);
            if (Data.GetDataPresent(t))
                return Data.GetData(t) as T;
            return null;
        }

        public IEnumerable<TransferDataType> DataTypes {
            get { return Data.GetFormats().Select(f => DragDropConverter.ToXwtTransferType(f)); }
        }
    }
}