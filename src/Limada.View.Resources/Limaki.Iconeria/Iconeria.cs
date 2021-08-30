/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using Xwt.Drawing;
using System;
using System.Linq;
using System.Linq.Expressions;
using Xwt;

namespace Limaki.Iconerias {

    public partial class Iconeria {

        public virtual Color FillColor { get; set; }
        public virtual Color StrokeColor { get; set; }
        public virtual bool Fill { get; set; }
        public virtual bool Stroke { get; set; }
        public virtual bool StrokeFirst { get; set; }
        public virtual Size DefaultSize { get; set; }
        public virtual double LineWidth { get; set; }

        public virtual void PaintIcon (Context c, double size, double x, double y, Action<Context> icon) {
            c.Save();

            // settings needs to be adjusted from the SVG path values, those works well for FontAwesome
            var border = size / 10;
            c.Translate(border + x, size * .8 + y);
            // c.Translate(border + x, size - border + y);
            var scale = 680;
            c.Scale(size / scale, -size / scale);

           
            var lw = LineWidth == 0 ? (2 / size * scale) : (LineWidth / size * scale);
            c.SetLineWidth (lw);
            icon (c);

            if (Stroke && StrokeFirst && Fill) {
                c.SetColor(StrokeColor);
                c.StrokePreserve();
            }

            if (Fill) {
                c.SetColor(FillColor);
                if (Stroke && ! StrokeFirst)
                    c.FillPreserve();
                else
                    c.Fill();
            }

            if (Stroke && ! StrokeFirst) {
                c.SetColor(StrokeColor);
                c.Stroke();
            }

            c.Restore();

        }


        public virtual void ForEach (Action<Action<Context>, string, string> visit) {
            foreach (var iconMethod in this.GetType().GetMethods()
                .Where(m => m.IsDefined(typeof(IconAttribute), true))
                .OrderBy(m => m.Name)) {
                var att = iconMethod.GetCustomAttributes(typeof(IconAttribute), true).First()
                          as IconAttribute;

                var icon = Delegate.CreateDelegate(typeof(Action<Context>), this, iconMethod) as Action<Context>;
                visit(icon, att.Name, att.Id);
            }
        }
    }

    public static class IconeriaExtensions {
        
        public static Image AsImage (this Iconeria self, Action<Context> icon, double size) {
            var ib = new ImageBuilder (size, size);
            self.PaintIcon (ib.Context, size, 0, 0, icon);
            var img = ib.ToBitmap (ImageFormat.ARGB32);
            return img;
        }

        public static Image AsImage<T> (this T self, Func<T, Action<Context>> icon, double size = default) where T : Iconeria {
            if (size == default)
                size = self.DefaultSize.Width;
            var ib = new ImageBuilder (size, size);
            var action = icon (self);
            self.PaintIcon (ib.Context, size, 0, 0, action);
            var img = ib.ToBitmap (ImageFormat.ARGB32);
            return img;
        }

        public static Image AsVectorImage (this Iconeria self, Action<Context> icon, double size) {
            var ib = new ImageBuilder (size, size);
            self.PaintIcon (ib.Context, size, 0, 0, icon);
            var img = ib.ToVectorImage();
            return img;
        }
    }
}