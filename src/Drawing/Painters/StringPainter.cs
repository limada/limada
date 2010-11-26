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

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Limaki.Drawing.Shapes;
using System.Drawing.Drawing2D;

namespace Limaki.Drawing.Painters {
    /// <summary>
    /// A painter that draws text
    /// </summary>
    public class StringPainter : IPainter {

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


        private GraphicsPath linedTextPath = new GraphicsPath();
        private Matrice lineMatrice = new Matrice();
        
        public static double Angle(Vector v) {
            double dx = (v.End.X - v.Start.X);
            double dy = (v.End.Y - v.Start.Y);
            if (dy == 0)
                return 0;
            else if (dx == 0)
                return 90;
            else
                return Math.Atan(dy / dx) / Math.PI * 180d;// +((dy < 0) ? 180 : 0);
        }

        public void Render(Graphics g) {
            IStyle style = this.Style;
            IShape shape = this.Shape;
            Font font = style.Font;

            if (AlignText && shape.ShapeDataType == typeof(Vector)) {
                linedTextPath.Reset();
                lineMatrice.Reset();
                Point c = shape[Anchor.Center];
                lineMatrice.Translate(c.X, c.Y);
                linedTextPath.AddString
                    (Text, font.FontFamily, (int)font.Style, font.Size, Point.Empty, stringFormat);
                lineMatrice.Rotate((float)Angle(((VectorShape)shape).Data));
                linedTextPath.Transform(lineMatrice.Matrix);
                g.DrawPath(GetPen(style.TextColor), linedTextPath);
                g.FillPath(GetSolidBrush(style.FillColor), linedTextPath);
                
            } else {
                RectangleF rect = shape.BoundsRect;
                float PenWidth = style.Pen.Width;
                //rect.Inflate(-PenWidth, -PenWidth);
                float[] elements = g.Transform.Elements;
                SizeF rectSize = rect.Size;
                if (elements[0] > 0.01f && elements[3] > 0.01f)
                    if (rectSize.Width > 1 && rectSize.Height > 1) {
                        g.DrawString(Text, font, GetSolidBrush(style.TextColor), rect, stringFormat);
                    }
            }
        }

        #endregion

        #region helper methods

        protected SolidBrush _brush = null;
        protected virtual SolidBrush GetSolidBrush(Color color) {
            if (_brush != null) {
                if (_brush.Color != color) {
                    _brush.Color = color;
                }
                return _brush;

            }
            if (_brush != null) {
                _brush.Dispose();
                _brush = null;
            }
            _brush = new SolidBrush(color);
            return _brush;
        }
        protected Pen _pen = null;
        protected virtual Pen GetPen(Color color) {
            if (_pen == null) {
                _pen = new Pen(color);
                _pen.Width = 0.1f;
            }
            if (_pen.Color != color) {
                _pen.Color = color;
            }
            return _pen;
        }

        #endregion

        #region TextStyle
        private StringFormat _stringFormat = null;
        protected StringFormat stringFormat {
            get {
                if (_stringFormat == null) {
                    _stringFormat = ShapeUtils.GetDefaultStringFormat();
                    hAlignChanged = true;
                    vAlignChanged = true;
                }
                if (hAlignChanged)
                    _stringFormat.Alignment = HorizontalAlignment;
                if (vAlignChanged)
                    _stringFormat.LineAlignment = VerticalAlignment;
                
                hAlignChanged = false;
                vAlignChanged = false;
                return _stringFormat;
            }
        }

        bool hAlignChanged = true;
        bool vAlignChanged = true;
        private StringAlignment _horizontalTextAlignment = StringAlignment.Center;
        /// <summary>
        /// Horizontal text alignment within the layout
        /// The default is centered text.
        /// </summary>
        virtual public StringAlignment HorizontalAlignment {
            get { return _horizontalTextAlignment; }
            set {
                if (_horizontalTextAlignment != value) {
                    _horizontalTextAlignment = value;
                    hAlignChanged = true;
                }
            }
        }

        private StringAlignment _verticalTextAlignment = StringAlignment.Center;
        /// <summary> 
        /// Vertical text alignment within the layout
        /// The default is centered text
        /// </summary>
        virtual public StringAlignment VerticalAlignment {
            get { return _verticalTextAlignment; }
            set {
                if (_verticalTextAlignment != value) {
                    _verticalTextAlignment = value;
                    vAlignChanged = true;
                }
                _verticalTextAlignment = value;
            }
        }

        private bool _alignText = true;
        /// <summary>
        /// Text is aligned along a vector-shape if Shape is IVectorShape
        /// </summary>
        public bool AlignText {
            get { return _alignText; }
            set { _alignText = value; }
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
