/*
 * Limaki 
 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2012 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.Common;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing.Painters {
    /// <summary>
    /// A painter that draws text
    /// </summary>
    public abstract class StringPainterBase : Painter<string>,IDataPainter<string>,IDataPainter {

        protected IShape _shape;
        public virtual IShape Shape {
            get { return _shape; }
            set { _shape = value; }
        }

        public override RenderType RenderType {
            get { return RenderType.Fill; }
            set { }
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
                    _drawingUtils = Registry.Factory.Create<IDrawingUtils>();
                }
                return _drawingUtils;
            }
        }

        public override Point[] Measure(Matrix matrix, int delta, bool extend) {
            var shape = this.Shape;
            if (this.Text != null && shape != null) {
                var style = this.Style;
                var font = style.Font;
                if (AlignText && shape is IVectorShape) {
                    var vector = ((IVectorShape)shape).Data;
                    var vLen = Vector.Length(vector);
                    var fontSize = font.Size + 2;
                    var size = new Size(vLen, fontSize);
                    size = drawingUtils.GetTextDimension(this.Text, style);
                    if (size.Width == 0) {
                        size.Width = vLen;
                        size.Height = fontSize;
                    }
                    var result = vector.Hull(-(vLen - size.Width) / 2, size.Height / 2);
                    if (matrix != null)
                        matrix.Transform(result);
                    return result;

                } else {
                    if (matrix != null)
                        return shape.Hull(matrix, delta, extend);
                    else
                        return shape.Hull(delta, extend);
                }
            } else
                return null;
        }
 
        private bool _alignText = true;
        /// <summary>
        /// Text is aligned along a vector-shape if Shape is IVectorShape
        /// </summary>
        public bool AlignText {
            get { return _alignText; }
            set { _alignText = value; }
        }

        public string Data {
            get { return Text; }
            set { Text = value; }
        }


        object IDataPainter.Data {
            get { return this.Data; }
            set {
                if (value is string)
                    this.Data = (string)value;
                else
                    this.Data = value.ToString();
            }
        }
       
    }
}