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

using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {

    public class XwtDrawingUtils : IDrawingUtils {

        public object GetCustomLineCap (double arrowWidth, double arrowHeigth) {
            return new object ();
        }

        public Pen CreatePen (Color color) {
            return new Pen (color);
        }

        private TextLayout tl = new TextLayout { Trimming = TextTrimming.Word };
        public Size GetTextDimension (string text, IStyle style) {
            var result = default(Size);
            lock (tl) {
                tl.Font = style.Font;
                tl.Text = text;
                if (style.AutoSize != Style.NoSize) {
                    tl.Width = style.AutoSize.Width;
                    tl.Height = style.AutoSize.Height;
                }
                result = tl.GetSize();
            }
            return result;
        }

        public Size ScreenResolution () {
            var f = Desktop.PrimaryScreen.ScaleFactor;
            return  new Size (96, 96);
        }

        public Size Resolution (Context context) {
            return new Size (96, 96);
        }



    }
}
