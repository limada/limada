/*
 * Limaki 
 * Version 0.063
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System.Drawing;
using System.Windows.Forms;
using Limaki.Drawing;

namespace Limaki.Winform {
    /// <summary>
    /// some helper methods used by selection classes
    /// </summary>
    public class SelectorHelper {
        # region Cursor-Handling

        public static Cursor HitCursor(Anchor hitAnchor) {
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

        public static void SetCursor(Anchor anchor, bool hasHit, Point p, Cursor savedCursor) {
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

        #endregion

        public static Anchor AdjacentAnchor(Anchor hitAnchor) {

            Anchor adjacentAnchor = Anchor.None;

            if (hitAnchor != Anchor.None)
                switch (hitAnchor) {
                    case Anchor.LeftTop:
                        //the origin is the right-bottom of the rectangle
                        adjacentAnchor = Anchor.RightBottom;
                        break;
                    case Anchor.LeftBottom:
                        //the origin is the right-top of the rectangle
                        adjacentAnchor = Anchor.RightTop;
                        break;
                    case Anchor.RightTop:
                        //the origin is the left-bottom of the rectangle
                        adjacentAnchor = Anchor.LeftBottom;
                        break;
                    case Anchor.RightBottom:
                        //the origin is the left-top of the tracker rectangle plus the little tracker offset
                        adjacentAnchor = Anchor.LeftTop;
                        break;
                    case Anchor.MiddleTop:
                        //the origin is the left-bottom of the rectangle
                        adjacentAnchor = Anchor.RightBottom;
                        break;
                    case Anchor.MiddleBottom:
                        adjacentAnchor = Anchor.RightTop;
                        //the origin is the left-top of the rectangle
                        break;
                    case Anchor.LeftMiddle:
                        //the origin is the right-top of the rectangle
                        adjacentAnchor = Anchor.RightBottom;
                        break;
                    case Anchor.RightMiddle:
                        //the origin is the left-top of the rectangle
                        adjacentAnchor = Anchor.LeftTop;
                        break;


                }

            return adjacentAnchor;
        }
    }
}
