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

using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Engine;

namespace Limaki.Drawing.GDI.Painters {
    public class StringPainter : StringPainterBase,IPainter<string> {
        #region helper methods

        protected SolidBrush _brush = null;
        protected virtual SolidBrush GetSolidBrush(System.Drawing.Color color) {
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
        protected System.Drawing.Pen _pen = null;
        protected virtual System.Drawing.Pen GetPen(System.Drawing.Color color) {
            if (_pen == null) {
                _pen = new System.Drawing.Pen(color);
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
                    _stringFormat = GDIUtils.GetDefaultStringFormat();
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

        #endregion

        
        private GraphicsPath linedTextPath = new GraphicsPath();
        private Matrice lineMatrice = new GDIMatrice();

        public override void Render(ISurface surface) {
            Graphics g = ( (GDISurface) surface ).Graphics;
            
            float[] elements = g.Transform.Elements;
            bool isVisible = ( elements[0] > 0.2f && elements[3] > 0.2f );
            
            if (isVisible) {
                var style = this.Style;
                var shape = this.Shape;
                var font = (System.Drawing.Font)WidgetRegistry.GetBackend(Style.Font);


                if (AlignText && shape is IVectorShape) {
                    var vector = ( (IVectorShape) shape ).Data;
                    var vlen =  (float)Vector.Length (vector);
                    var vheight = font.SizeInPoints + ( font.SizeInPoints/4f );
                    lineMatrice.Reset ();
                    var c = new PointF (
                        (float)(vector.Start.X + ( vector.End.X - vector.Start.X )/2f),
                        (float)(vector.Start.Y + (vector.End.Y - vector.Start.Y) / 2f));
                    
                    lineMatrice.Translate (c.X - 1, c.Y - 1);
                    lineMatrice.Rotate ( Angle (vector));

                    linedTextPath.Reset ();
                    // TODO: something is wrong with emSize, it is too small:
                    var emSize = font.Size;
                    linedTextPath.AddString
                        (Text, font.FontFamily, (int) font.Style, emSize,
                         new RectangleF (new PointF (-vlen/2f, -vheight/2f), new SizeF (vlen, vheight)), stringFormat);

                    using (var matrix = ( (GDIMatrice) lineMatrice ).Matrix) {
                        linedTextPath.Transform (matrix);
                    }

                    g.DrawPath (
                        GetPen (GDIConverter.Convert (style.FillColor)),
                        linedTextPath);
                    g.FillPath (
                        GetSolidBrush (GDIConverter.Convert (style.TextColor)),
                        linedTextPath);
                } else {
                    RectangleF rect = GDIConverter.Convert (shape.BoundsRect);
                    float PenWidth = (float) style.Pen.Thickness;
                    //rect.Inflate(-PenWidth, -PenWidth);

                    SizeF rectSize = rect.Size;

                    if (rectSize.Width > 1 && rectSize.Height > 1) {
                        g.DrawString (Text, font,
                                      GetSolidBrush (GDIConverter.Convert (style.TextColor)),
                                      rect, stringFormat);
                    }
                }
            }
        }

    }
}