using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.UI;

namespace Limaki.Winform {
    public class CursorHandler:ICursorHander {

        protected Cursor savedCursor = Cursors.Default;

        public Cursor HitCursor(Anchor hitAnchor) {
            Cursor result = Cursor.Current;
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


        public void SetCursor(IControl control, Anchor anchor, bool hasHit) {
            if (hasHit) {
                if (anchor != Anchor.None) {
                    Cursor.Current = HitCursor(anchor);
                } else {
                    Cursor.Current = Cursors.SizeAll;
                }
            } else {
                Cursor.Current = (hasHit ? Cursors.SizeAll : savedCursor);
            }
        }

        public void SetEdgeCursor(IControl control, Anchor anchor) {
            if (anchor != Anchor.None) {
                Cursor.Current = Cursors.HSplit;
            } else {
                Cursor.Current = savedCursor;
            }
        }

        public void SaveCursor(IControl control) {
            savedCursor = Cursor.Current;
        }

        public void  RestoreCursor(IControl control) {
            Cursor.Current = ( (Control) control ).Cursor;
        }
    }

}