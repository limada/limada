using Xwt.GdiBackend;
using Xwt.SwfBackend;

namespace Limaki.View.SwfBackend.DragDrop {

    public static class DragDropEventConverter{

        public static Limaki.View.DragDrop.DragOverEventArgs ToXwtDragOver (this System.Windows.Forms.DragEventArgs args, System.Windows.Forms.Control control) {
            var pt = control.PointToClient (new System.Drawing.Point (args.X, args.Y));

            var result = new Limaki.View.DragDrop.DragOverEventArgs (pt.ToXwt (), new SwfTransferDataStore (args.Data), args.AllowedEffect.ToXwt ()) {
                AllowedAction = args.Effect.ToXwt (),
            };
            return result;
        }

        public static Limaki.View.DragDrop.DragEventArgs ToXwt (this System.Windows.Forms.DragEventArgs args, System.Windows.Forms.Control control) {
            var pt = control.PointToClient (new System.Drawing.Point (args.X, args.Y));
            var result = new Limaki.View.DragDrop.DragEventArgs (pt.ToXwt (), new SwfTransferDataStore (args.Data), args.Effect.ToXwt ()) {

            };
            return result;
        }
 
     }
}