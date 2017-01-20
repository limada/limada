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
using System;

namespace Limaki.View.XwtBackend {

    public class XwtDrawingUtils : IDrawingUtils {

        private TextLayout tl = new TextLayout { Trimming = TextTrimming.Word };
        SystemFonts SystemFonts = new SystemFonts ();

        public Size GetTextDimension (string text, IStyle style) {
            var result = default (Size);
            if (style == null) {
                style = new Style ("") { Font = SystemFonts.DefaultFont.WithSize(10) };
            }
            lock (tl) {
                tl.Font = style.Font;
                tl.Text = text;
                if (style.AutoSize != Style.NoSize) {
                    tl.Width = style.AutoSize.Width;
                    tl.Height = style.AutoSize.Height;
                }
				var size = tl.GetSize();
				result = new Size (size.Width, Math.Min (style.AutoSize.Height, size.Height));
            }
            return result;
        }

        public Size GetObjectDimension (object value, IStyle style) {
            var result = new Size ();
            if (!DrawingExtensions.TryGetObjectDimension (value, style, out result))
                return Size.Zero;
            return result;
        }

        public Size ScreenResolution () {
            var f = Desktop.PrimaryScreen.ScaleFactor;
            return  new Size (96*f, 96*f);
        }

        public Size Resolution (Context context) {
            return new Size (96, 96);
        }

    }
}
