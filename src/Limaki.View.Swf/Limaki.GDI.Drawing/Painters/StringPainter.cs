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

using System.Drawing;
using System.Drawing.Drawing2D;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Engine;
using Xwt.Gdi;
using Xwt.Gdi.Backend;

namespace Limaki.Drawing.Gdi.Painters {
   
    public class StringPainter : StringPainterBase, IPainter<string> {

        #region TextStyle
        private StringFormat _stringFormat = null;
        protected StringFormat StringFormat {
            get {
                if (_stringFormat == null) {
                    _stringFormat = GdiConverter.GetDefaultStringFormat ();
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
        public override void Render (ISurface surface) {
            RenderGdi(surface);
        }

        private GraphicsPath linedTextPath = new GraphicsPath ();
        private Matrice lineMatrice = new GdiMatrice ();

        public override void RenderGdi (ISurface surface) {
            Graphics g = ((GdiSurface) surface).Graphics;

            float[] elements = g.Transform.Elements;
            bool isVisible = (elements[0] > 0.2f && elements[3] > 0.2f);

            if (isVisible) {
                var style = this.Style;
                var shape = this.Shape;
                var font = (System.Drawing.Font)GdiEngine.Registry.GetBackend(Style.Font);

                if (AlignText && shape is IVectorShape) {
                    var vector = ((IVectorShape) shape).Data;
                    var vlen = (float) Vector.Length (vector);
                    var vheight = font.SizeInPoints + (font.SizeInPoints / 4f);
                    lineMatrice.Reset ();
                    var c = new PointF (
                        (float) (vector.Start.X + (vector.End.X - vector.Start.X) / 2f),
                        (float) (vector.Start.Y + (vector.End.Y - vector.Start.Y) / 2f));

                    lineMatrice.Translate (c.X - 1, c.Y - 1);
                    lineMatrice.Rotate (Vector.Angle (vector));

                    linedTextPath.Reset ();
                    // TODO: something is wrong with emSize, it is too small:
                    var emSize = font.Size;
                    linedTextPath.AddString
                        (Text, font.FontFamily, (int) font.Style, emSize,
                         new RectangleF (new PointF (-vlen / 2f, -vheight / 2f), new SizeF (vlen, vheight)), StringFormat);

                    using (var matrix = ((GdiMatrice) lineMatrice).Matrix) {
                        linedTextPath.Transform (matrix);
                    }

                    g.DrawPath (
                        GetPen (GdiConverter.ToGdi (style.FillColor)),
                        linedTextPath);
                    g.FillPath (
                        GetSolidBrush (GdiConverter.ToGdi (style.TextColor)),
                        linedTextPath);
                } else {
                    RectangleF rect = GDIConverter.Convert (shape.BoundsRect);
                    float PenWidth = (float) style.Pen.Thickness;
                    //rect.Inflate(-PenWidth, -PenWidth);

                    SizeF rectSize = rect.Size;

                    if (rectSize.Width > 1 && rectSize.Height > 1) {
                        g.DrawString (Text, font,
                                      GetSolidBrush (GdiConverter.ToGdi (style.TextColor)),
                                      rect, StringFormat);
                    }
                }
            }
        }

        public override void RenderXwt(ISurface surface) {
            throw new System.NotImplementedException();
        }
        
    }
}