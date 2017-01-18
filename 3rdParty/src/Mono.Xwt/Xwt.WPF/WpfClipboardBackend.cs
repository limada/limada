using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xwt.Backends;
using WindowsClipboard = System.Windows.Clipboard;

namespace Xwt.WPFBackend {

    public partial class WpfClipboardBackend {
        public override IEnumerable<TransferDataType> GetTypesAvailable () {

            var dataObject = WindowsClipboard.GetDataObject ();
            if (dataObject == null)
                yield break;

            var formats = dataObject.GetFormats (false);
            foreach (var f in formats)
                yield return TransferDataType.FromId (f);
        }
    }
}