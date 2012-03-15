using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.GDI;

namespace Limaki.View.Winform {
    public class CursorHandler:IDeviceCursor {

        Control control = null;
        public CursorHandler(Control control) {
            this.control = control;
        }

        protected Cursor savedCursor = Cursors.Default;

        public Cursor HitCursor(Anchor hitAnchor) {
            Cursor result = control.Cursor;
            if (hitAnchor != Anchor.None)
                switch (hitAnchor) {
                    case Anchor.Center:
                        result = Cursors.SizeAll;
                        break;
                    case Anchor.LeftTop:
                        result = Cursors.SizeNWSE;
                        break;
                    case Anchor.LeftBottom:
                        result = Cursors.SizeNESW;
                        break;
                    case Anchor.RightTop:
                        result = Cursors.SizeNESW;
                        break;
                    case Anchor.RightBottom:
                        result = Cursors.SizeNWSE;
                        break;
                    case Anchor.MiddleTop:
                        result = Cursors.SizeNS;
                        break;
                    case Anchor.MiddleBottom:
                        result = Cursors.SizeNS;
                        break;
                    case Anchor.LeftMiddle:
                        result = Cursors.SizeWE;
                        break;
                    case Anchor.RightMiddle:
                        result = Cursors.SizeWE;
                        break;
                }
            return result;
        }


        public void SetCursor(Anchor anchor, bool hasHit) {
            if (hasHit) {
                if (anchor != Anchor.None) {
                    control.Cursor = HitCursor(anchor);
                } else {
                    control.Cursor = Cursors.SizeAll;
                }
            } else {
                control.Cursor = (hasHit ? Cursors.SizeAll : savedCursor);
            }
        }

        public void SetEdgeCursor(Anchor anchor) {
            if (anchor != Anchor.None) {
                control.Cursor = Cursors.HSplit;
            } else {
                control.Cursor = savedCursor;
            }
        }

        public void SaveCursor() {
            savedCursor = control.Cursor;
        }

        public void  RestoreCursor() {
            control.Cursor = Cursors.Default;
        }

    }

}