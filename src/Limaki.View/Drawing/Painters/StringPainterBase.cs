/*
 * Limaki 
 * Version 0.08
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

using System;
using Limaki.Common;
using Limaki.Drawing.Shapes;

namespace Limaki.Drawing.Painters {
    /// <summary>
    /// A painter that draws text
    /// </summary>
    public abstract class StringPainterBase : IDataPainter {

        #region IPainter Member

        public RenderType RenderType {
            get { return RenderType.Fill; }
            set { }
        }

        protected IStyle _style;
        public virtual IStyle Style {
            get { return _style; }
            set { _style = value; }
        }

        protected IShape _shape;
        public virtual IShape Shape {
            get { return _shape; }
            set { _shape = value; }
        }

        protected string _text;
        public virtual string Text {
            get { return _text; }
            set { _text = value; }
        }


        static IDrawingUtils _drawingUtils = null;
        protected static IDrawingUtils drawingUtils {
            get {
                if (_drawingUtils == null) {
                    _drawingUtils = Registry.Factory.One<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }
        
        public static double Angle(Vector v) {
            double dx = (v.End.X - v.Start.X);
            double dy = (v.End.Y - v.Start.Y);
            if (dy == 0)
                return 0;
            else if (dx == 0)
                return 90;
            else
                return Math.Atan(dy / dx) / Math.PI * 180d;
            // +((dy < 0) ? 180 : 0);
        }


        #endregion


        private bool _alignText = true;
        /// <summary>
        /// Text is aligned along a vector-shape if Shape is IVectorShape
        /// </summary>
        public bool AlignText {
            get { return _alignText; }
            set { _alignText = value; }
        }


        #region IDisposable Member

        public virtual void Dispose(bool disposing) { }
        public virtual void Dispose() {
            Dispose(true);
        }

        #endregion

        #region IDataPainter Member

        public object Data {
            get { return Text; }
            set {Text = value.ToString (); }
        }

        #endregion

        
        public virtual PointI[] Measure(Matrice matrix, int delta, bool extend) {
            IShape shape = this.Shape;
            if (this.Text != null && shape != null) {
                IStyle style = this.Style;
                Font font = style.Font;
                if (AlignText && shape.ShapeDataType == typeof(Vector)) {
                    Vector vector = ((VectorShape)shape).Data;
                    float vLen = (float)Vector.Length(vector);
                    float fontSize = (float)font.Size + 2;
                    SizeS size = new SizeS(vLen, fontSize);
                    size = drawingUtils.GetTextDimension(this.Text, style);
                    if (size.Width == 0) {
                        size.Width = vLen;
                        size.Height = fontSize;
                    }
                    PointI[] result = vector.Hull(-(vLen - size.Width) / 2, size.Height / 2);
                    matrix.TransformPoints(result);
                    return result;

                } else {
                    return shape.Hull(matrix, delta, extend);
                }
            } else
                return null;
        }

        public abstract void Render(ISurface surface);
    }
}