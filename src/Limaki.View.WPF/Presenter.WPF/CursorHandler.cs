
using Limaki.Drawing;

using System.Windows.Input;
using System.Windows.Controls;

namespace Limaki.View.WPF {
    public class CursorHandler : IBackendCursor {
        public CursorHandler(IWPFControl control) {
            this.control = control;
        }

        IWPFControl control = null;

        protected Cursor savedCursor = Cursors.Arrow;

        public Cursor HitCursor(Anchor hitAnchor) {
            Cursor result = Cursors.Arrow;
#if SILVERLIGHT
            if (hitAnchor != Anchor.None) {
                result = Cursors.SizeNS;
            }
#else
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
#endif
            return result;
        }

  
        public void SetCursor(Anchor anchor, bool hasHit) {
            if (hasHit) {
                if (anchor != Anchor.None) {
                    control.Cursor = HitCursor(anchor);
                } else {
#if ! SILVERLIGHT
                    control.Cursor = Cursors.SizeAll;
#else
                    control.Cursor = Cursors.Hand;
#endif
                }
            } else {
                control.Cursor = Cursors.Arrow;
            }
        }

        public void SetEdgeCursor(Anchor anchor) {
            if (anchor != Anchor.None) {
                control.Cursor = Cursors.Arrow;
            } else {
                control.Cursor = Cursors.Arrow;
            }
        }

        public void SaveCursor() {
            savedCursor = control.Cursor;
        }

        public void  RestoreCursor() {
            control.Cursor = Cursors.Arrow;
        }
    }
}