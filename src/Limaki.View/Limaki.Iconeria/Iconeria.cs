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
using Xwt.Engine;

namespace Limaki.Iconerias {

    public partial class Iconeria {

        public virtual Color FillColor { get; set; }
        public virtual Color StrokeColor { get; set; }
        public virtual bool Fill { get; set; }
        public virtual bool Stroke { get; set; }
        public virtual bool StrokeFirst { get; set; }

        public virtual void PaintIcon (Context c, double size, double x, double y, Action<Context> icon) {
            c.Save();

            // settings needs to be adjusted from the SVG path values, those works well for FontAwesome
            var border = size / 10;
            c.Translate(border + x, size * .8 + y);
            // c.Translate(border + x, size - border + y);
            var scale = 2500;
            c.Scale(size / scale, -size / scale);

            icon(c);

            c.SetLineWidth(size / 50);

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

        public virtual Image AsImage (WidgetRegistry registry, Action<Context> icon, int size) {
            var ib = new ImageBuilder(registry, size, size, ImageFormat.ARGB32);
            PaintIcon(ib.Context, size, 0, 0, icon);
            var img = ib.ToImage();
            // remark: get backend like this:
            // var imgBackend = WidgetRegistry.MainRegistry.GetBackend(img) as e.g. System.Drawing.Image;
            return img;
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
}