using Limaki.View.Swf.Backends;
using Xwt.Gdi.Backend;
using SD = System.Drawing;
using SWF = System.Windows.Forms;

namespace Xwt.WinformBackend {

    public static class DragDropEventConverter{

        public static Limaki.View.DragDrop.DragOverEventArgs ToXwtDragOver (this SWF.DragEventArgs args, SWF.Control control) {
            var pt = control.PointToClient (new SD.Point (args.X, args.Y));

            var result = new Limaki.View.DragDrop.DragOverEventArgs (pt.ToXwt (), new SwfTransferDataStore (args.Data), args.AllowedEffect.ToXwt ()) {
                AllowedAction = args.Effect.ToXwt (),
            };
            return result;
        }

        public static Limaki.View.DragDrop.DragEventArgs ToXwt (this SWF.DragEventArgs args, SWF.Control control) {
            var pt = control.PointToClient (new SD.Point (args.X, args.Y));
            var result = new Limaki.View.DragDrop.DragEventArgs (pt.ToXwt (), new SwfTransferDataStore (args.Data), args.Effect.ToXwt ()) {

            };
            return result;
        }
 
     }
}