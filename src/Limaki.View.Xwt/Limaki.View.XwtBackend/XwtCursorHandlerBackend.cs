/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class XwtCursorHandlerBackend : ICursorHandler {

        protected Widget Widget { get; set; }

        public XwtCursorHandlerBackend(Widget widget) {
            this.Widget = widget;
        }

        protected CursorType SavedCursor = CursorType.Arrow;

        public CursorType HitCursor (Anchor hitAnchor) {
            var result = Widget.Cursor;
            if (hitAnchor != Anchor.None)
                switch (hitAnchor) {
                    case Anchor.Center:
                        result = MoveCursor;
                        break;
                    case Anchor.LeftTop:
                        result = CursorType.ResizeLeftRight;
                        break;
                    case Anchor.LeftBottom:
                        result = CursorType.ResizeLeftRight;
                        break;
                    case Anchor.RightTop:
                        result = CursorType.ResizeLeftRight;
                        break;
                    case Anchor.RightBottom:
                        result = CursorType.ResizeLeftRight;
                        break;
                    case Anchor.MiddleTop:
                        result = CursorType.ResizeUpDown;
                        break;
                    case Anchor.MiddleBottom:
                        result = CursorType.ResizeUpDown;
                        break;
                    case Anchor.LeftMiddle:
                        result = CursorType.ResizeLeftRight;
                        break;
                    case Anchor.RightMiddle:
                        result = CursorType.ResizeLeftRight;
                        break;
                }
            return result;
        }

        public void SetCursor (Anchor anchor, bool hasHit) {
            if (hasHit) {
                if (anchor != Anchor.None) {
                    Widget.Cursor = HitCursor(anchor);
                } else {
                    Widget.Cursor = MoveCursor;
                }
            } else {
                Widget.Cursor = (hasHit ? MoveCursor : SavedCursor);
            }
        }

        public static readonly CursorType MoveCursor = CursorType.Hand;
        public static readonly CursorType EdgeCursor = CursorType.IBeam;

        public void SetEdgeCursor (Anchor anchor) {
            if (anchor != Anchor.None) {
                Widget.Cursor = EdgeCursor;
            } else {
                Widget.Cursor = SavedCursor;
            }
        }

        public void SaveCursor () {
            SavedCursor = Widget.Cursor;
        }

        public void RestoreCursor () {
            Widget.Cursor = CursorType.Arrow; //.SavedCursor;
        }
    }
}
