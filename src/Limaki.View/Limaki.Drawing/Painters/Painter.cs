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
 * http://limada.sourceforge.net
 * 
 */

using Xwt;
using System;
using Xwt.Drawing;

namespace Limaki.Drawing.Painters {

    public abstract class Painter<T> : IPainter<T>, IPainter<IShape<T>, T> {

        #region IPainter<T> Member

		protected IShape<T> _shape;
        public virtual IShape<T> Shape {
            get { return _shape; }
            set { _shape = value; }
        }

        #endregion

        #region IPainter Member


		///<directed>True</directed>
        protected IStyle _style;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

        IShape IPainter.Shape {
            get { return Shape; }
            set {
                if (value is IShape<T>)
                    Shape = (IShape<T>)value;
            }
        }

        protected RenderType _renderType = RenderType.DrawAndFill;
        public virtual RenderType RenderType {
            get { return _renderType; }
            set { _renderType = value;  }
        }

        public abstract void Render ( ISurface surface );

        public virtual void Render (Context ctx, Action<Context, T> draw, 
            RenderType renderType, Color fillColor, Color penColor, double thickness) {

            draw (ctx, Shape.Data);
            if (renderType.HasFlag (RenderType.Fill)) {
                ctx.SetColor (fillColor);
                ctx.FillPreserve ();
            }
            if (renderType.HasFlag (RenderType.Draw)) {
                ctx.SetColor (penColor);
                ctx.SetLineWidth (thickness);
                ctx.Stroke ();
            }
        }

        public virtual void Render (Context ctx, Action<Context, T> draw) {
            var style = this.Style;
            var renderType = this.RenderType;
            draw (ctx, Shape.Data);
            if (renderType.HasFlag (RenderType.Fill)) {
                ctx.SetColor (style.FillColor);
                ctx.FillPreserve ();
            }
            if (renderType.HasFlag (RenderType.Draw)) {
                ctx.SetColor (style.Pen.Color);
                ctx.SetLineWidth (style.Pen.Thickness);
                ctx.Stroke ();
            }
        }
        public virtual Point[] Measure(Matrice matrix, int delta, bool extend) {
            return Shape.Hull (matrix, delta, extend);
        }

        public virtual Point[] Measure(int delta, bool extend) {
            return Shape.Hull(delta, extend);
        }

        #endregion

        #region IDisposable Member
        public virtual void Dispose(bool disposing) { }
        public virtual void Dispose() {
            Dispose(true);
        }

        #endregion
    }
}