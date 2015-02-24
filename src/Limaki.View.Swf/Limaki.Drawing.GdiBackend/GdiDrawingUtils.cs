/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using Limaki.Drawing;
using Xwt.Drawing;
using System.Drawing.Drawing2D;
using Xwt;
using Xwt.GdiBackend;
using Xwt.Backends;

namespace Limaki.View.GdiBackend {

    public class GdiDrawingUtils : IDrawingUtils {

        public virtual Size GetTextDimension(string text, IStyle style) {
            return GdiUtils.GetTextDimension(
                (System.Drawing.Font)style.Font.GetBackend(),
                text,
                style.AutoSize.ToGdi());
        }

        public Size GetObjectDimension (object value, IStyle style) {
            var result = new Size ();
            if (!DrawingExtensions.TryGetObjectDimension (value, style, out result))
                return Size.Zero;
            return result;
        }

        public Size ScreenResolution() {
            return new Size(GdiUtils.DeviceContext.DpiX, GdiUtils.DeviceContext.DpiY);
        }

        public Size Resolution(Context context) {
            var ctx = (Xwt.GdiBackend.GdiContext)context.GetBackend();
            if (ctx.Graphics.PageUnit == System.Drawing.GraphicsUnit.Point)
                return new Size(72, 72);
            return new Size(ctx.Graphics.DpiX, ctx.Graphics.DpiY);
        }

    }
}