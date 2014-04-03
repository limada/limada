// 
// GdiConverter.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (http://www.limada.org)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.


using System.Drawing;
using SD = System.Drawing;
using SDI = System.Drawing.Imaging;
using System.Drawing.Text;
using Xwt.Drawing;
using Color = Xwt.Drawing.Color;
using Font = Xwt.Drawing.Font;
using FontStyle = Xwt.Drawing.FontStyle;
using Matrix = Xwt.Drawing.Matrix;
using System.Drawing.Drawing2D;
using Xwt.Backends;
using System;


namespace Xwt.GdiBackend {

    public static class GdiConverter {

        #region Color

        public static SD.Color ToGdi(this Color color) {
            return SD.Color.FromArgb((int)ToArgb(color));
        }

        public static Color ToXwt(this SD.Color color) {
            return FromArgb((uint)color.ToArgb());
        }

        public static uint ToArgb(Color color) {
            return
                (uint)(color.Alpha * 255) << 24
                | (uint)(color.Red * 255) << 16
                | (uint)(color.Green * 255) << 8
                | (uint)(color.Blue * 255);

        }

        public static Color FromArgb(uint argb) {
            var a = (argb >> 24) / 255d;
            var r = ((argb >> 16) & 0xFF) / 255d;
            var g = ((argb >> 8) & 0xFF) / 255d;
            var b = (argb & 0xFF) / 255d;
            return new Color(r, g, b, a);

        }

        public static Color FromArgb(byte a, Color color) {
            return new Color(color.Red, color.Green, color.Blue, a);
        }

        public static Color FromArgb(byte a, byte r, byte g, byte b) {
            return Color.FromBytes(r, g, b, a);
        }

        public static Color FromArgb(byte r, byte g, byte b) {
            return Color.FromBytes(r, g, b);
        }

        #endregion

        #region Size, Point, Rectangle

        public static Size ToXwt (this SD.SizeF value) {
            return new Size (value.Width, value.Height);
        }

        public static Size ToXwt(this SD.Size value) {
            return new Size(value.Width, value.Height);
        }

        public static SD.Size ToGdi (this Size value) {
            return new SD.Size ((int) value.Width, (int) value.Height);
        }

        public static SD.SizeF ToGdiF (this Size value) {
            return new SD.SizeF ((float) value.Width, (float) value.Height);
        }

        public static SD.Point ToGdi (this Point value) {
            return new SD.Point ((int) value.X, (int) value.Y);
        }

        public static SD.PointF ToGdiF (this Point value) {
            return new SD.PointF ((float) value.X, (float) value.Y);
        }

        public static Point ToXwt (this SD.Point value) {
            return new Point (value.X, value.Y);
        }

        public static Point ToXwt (this SD.PointF value) {
            return new Point (value.X, value.Y);
        }

        public static Rectangle ToXwt (this SD.Rectangle value) {
            return new Rectangle (value.X, value.Y, value.Width, value.Height);
        }

        public static Rectangle ToXwt (this RectangleF value) {
            return new Rectangle (value.X, value.Y, value.Width, value.Height);
        }

        public static SD.Rectangle ToGdi (this Rectangle value) {
            return new SD.Rectangle ((int) value.X, (int) value.Y, (int) value.Width, (int) value.Height);
        }

        public static SD.RectangleF ToGdiF (this Rectangle value) {
            return new SD.RectangleF ((float) value.X, (float) value.Y, (float) value.Width, (float) value.Height);
        }

        #endregion

        #region Graphics

        public static StringFormat GetDefaultStringFormat() {
            var stringFormat =
                StringFormat.GenericTypographic;
            stringFormat.Trimming = StringTrimming.EllipsisWord;
            //stringFormat.FormatFlags = SD.StringFormatFlags.FitBlackBox;
            stringFormat.FormatFlags = stringFormat.FormatFlags
                                       & ~StringFormatFlags.NoClip
                                       & ~StringFormatFlags.FitBlackBox
                                       & StringFormatFlags.LineLimit
                ;
            return stringFormat;
        }

        public class GraphicsQuality {
            public InterpolationMode InterpolationMode { get; set; }
            public CompositingMode CompositingMode { get; set; }
            public CompositingQuality CompositingQuality { get; set; }

            public SmoothingMode SmoothingMode { get; set; }

            public TextRenderingHint TextRenderingHint { get; set; }

            public PixelOffsetMode PixelOffsetMode { get; set; }
        }

        public static GraphicsQuality DrawHighQuality {
            get {
                return new GraphicsQuality {
                    InterpolationMode = InterpolationMode.HighQualityBilinear,
                    CompositingMode = CompositingMode.SourceOver,
                    CompositingQuality = CompositingQuality.HighQuality,
                    PixelOffsetMode = PixelOffsetMode.HighQuality
                };
            }
        }
        
        public static GraphicsQuality DrawTextHighQuality {
            get {
                return new GraphicsQuality {
                    InterpolationMode = InterpolationMode.NearestNeighbor,
                    CompositingMode = CompositingMode.SourceOver,
                    CompositingQuality = CompositingQuality.HighQuality,
                    PixelOffsetMode = PixelOffsetMode.HighQuality,
                    SmoothingMode = SmoothingMode.HighQuality,
                    TextRenderingHint = TextRenderingHint.AntiAliasGridFit,
                  
                };
            }
        }
        public static GraphicsQuality PaintHighQuality {
            get {
                return new GraphicsQuality {
                    InterpolationMode = InterpolationMode.HighQualityBilinear,
                    CompositingMode = CompositingMode.SourceOver,
                    CompositingQuality = CompositingQuality.AssumeLinear,
                    SmoothingMode = SmoothingMode.AntiAlias,
                    TextRenderingHint = TextRenderingHint.AntiAliasGridFit,
                    PixelOffsetMode=PixelOffsetMode.HighQuality
                };
            }
        }
        public static GraphicsQuality CopyHighQuality {
            get {
                return new GraphicsQuality {
                    InterpolationMode = InterpolationMode.HighQualityBilinear,
                    CompositingMode = CompositingMode.SourceCopy,
                    CompositingQuality = CompositingQuality.HighQuality,
                    PixelOffsetMode = PixelOffsetMode.HighQuality
                };
            }
        }

        public static GraphicsQuality SetQuality (this Graphics g, GraphicsQuality quality) {
            var result = new GraphicsQuality {
                InterpolationMode = g.InterpolationMode,
                CompositingMode = g.CompositingMode,
                CompositingQuality = g.CompositingQuality,
                SmoothingMode = g.SmoothingMode,
                TextRenderingHint = g.TextRenderingHint,
                PixelOffsetMode = g.PixelOffsetMode
            };
            if (true) {
                g.InterpolationMode = quality.InterpolationMode;
                g.CompositingMode = quality.CompositingMode;
                g.CompositingQuality = quality.CompositingQuality;
                g.SmoothingMode = quality.SmoothingMode;
                g.TextRenderingHint = quality.TextRenderingHint;
                g.PixelOffsetMode = quality.PixelOffsetMode;
            }
            return result;
        }

        public static StringTrimming ToGdi (this TextTrimming value) {
            if (value == TextTrimming.Word)
                return StringTrimming.Word;
            if (value == TextTrimming.WordElipsis)
                return StringTrimming.EllipsisWord;

            return StringTrimming.Word;
        }

        public static Xwt.Drawing.Image ToXwt (this SD.Image value) {
            if (value == null)
                return null;
            return new Xwt.Drawing.Image(value);
        }

        public static SD.Image ToGdi (this  Xwt.Drawing.Image value) {
            if (value == null)
                return null;
            var image = value.GetBackend() as GdiImage;
            if (image == null)
                return null;
            return image.Image;
        }

        public static SDI.PixelFormat ToGdi (this  ImageFormat value) {
            if (value == ImageFormat.ARGB32)
                return SDI.PixelFormat.Format32bppPArgb;
            else if (value == ImageFormat.RGB24)
                return SDI.PixelFormat.Format24bppRgb;
            return SDI.PixelFormat.DontCare;
        }

        public static Xwt.Drawing.ImageFormat ToXwt (this SD.Imaging.PixelFormat value) {
            if (value == SDI.PixelFormat.Format32bppPArgb)
                return ImageFormat.ARGB32;
            else if (value == SDI.PixelFormat.Format24bppRgb)
                return ImageFormat.RGB24;
            throw new ArgumentException();
        }

        public static SDI.ImageFormat ToGdi(this ImageFileType value) {
            if (value == ImageFileType.Bmp)
                return SDI.ImageFormat.Bmp;
            else if (value == ImageFileType.Jpeg)
                return SDI.ImageFormat.Jpeg;
            else if (value == ImageFileType.Png)
                return SDI.ImageFormat.Png;
            throw new ArgumentException();
        }
        #endregion

        #region Matrix

        public static System.Drawing.Drawing2D.Matrix ToGdi (this Matrix matrix) {
            return new System.Drawing.Drawing2D.Matrix(
                (float)matrix.M11,
                (float)matrix.M12,
                (float)matrix.M21,
                (float)matrix.M22,
                (float)matrix.OffsetX,
                (float)matrix.OffsetY);
        }

        public static Matrix ToXwt (this System.Drawing.Drawing2D.Matrix matrice) {
            return new Matrix(
               matrice.Elements[0],
               matrice.Elements[1],
               matrice.Elements[2],
               matrice.Elements[3],
               matrice.Elements[4],
               matrice.Elements[5]);
        }

        #endregion

        #region Font

        public static SD.FontStyle ToGdi (this FontStyle value) {
            var result = SD.FontStyle.Regular;
            if (value == null)
                return result;
            if ((value & FontStyle.Italic) != 0) {
                result |= SD.FontStyle.Italic;
            }
            //if ((value & FontStyle.Underline) != 0) {
            //    result |= SD.FontStyle.Underline;
            //}
            if ((value & FontStyle.Oblique) != 0) {
                result |= SD.FontStyle.Bold;
            }
            return result;
        }
        
        public static FontStyle ToXwt (this SD.FontStyle value) {
            var result = FontStyle.Normal;
            if (value == null)
                return result;
            if ((value & SD.FontStyle.Italic) != 0) {
                result |= FontStyle.Italic;
            }
            //if ((value & SD.FontStyle.Underline) != 0) {
            //    result |= FontStyle.Underline;
            //}
            if ((value & SD.FontStyle.Bold) != 0) {
                result |= FontStyle.Oblique;
            }
            return result;
        }

        public static FontWeight ToXwtWeight (SD.FontStyle style) {
            if (style == SD.FontStyle.Bold)
                return FontWeight.Bold;
            return FontWeight.Normal;

        }

        public static SD.Font ToGdi (this Font value) {
            return (SD.Font) value.GetBackend();
        }

        public static SD.FontStyle ToGdi (this FontStyle style, FontWeight weight) {
            var result = SD.FontStyle.Regular;
            if (FontStyle.Italic == style || FontStyle.Oblique == style)
                result |= SD.FontStyle.Italic;
            if (FontWeight.Heavy == weight || FontWeight.Bold == weight || FontWeight.Ultrabold == weight)
                result |= SD.FontStyle.Bold;
            return result;
        }

        public static Font ToXwt (this System.Drawing.Font backend) {
            return new Font(backend);
        }

        
        #endregion

    }
}