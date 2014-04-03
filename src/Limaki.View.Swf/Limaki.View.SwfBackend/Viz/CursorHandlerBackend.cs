/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.View.Viz;

namespace Limaki.View.SwfBackend.Viz {

    public class CursorHandlerBackend:ICursorHandler {

        Control control = null;
        public CursorHandlerBackend(Control control) {
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