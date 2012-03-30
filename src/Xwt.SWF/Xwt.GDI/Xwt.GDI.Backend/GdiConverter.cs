// 
// GdiConverter.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (http://limada.sourceforge.net)
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


using Xwt.Engine;
using Color = Xwt.Drawing.Color;
using Font = Xwt.Drawing.Font;
using FontStyle = Xwt.Drawing.FontStyle;
using  System.Drawing.Drawing2D;
using SD = System.Drawing;

namespace Xwt.Gdi {

    public static class GdiConverter {

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

        public static SD.StringTrimming ToGdi (this Xwt.Drawing.TextTrimming value) {
            if (value == Xwt.Drawing.TextTrimming.Word) return SD.StringTrimming.Word;
            if (value == Xwt.Drawing.TextTrimming.WordElipsis) return SD.StringTrimming.EllipsisWord;

            return SD.StringTrimming.Word;
        }

        public static SD.FontStyle ToGdi(this FontStyle value) {
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

        public static FontStyle ToXwt(this SD.FontStyle value) {
            var result = FontStyle.Normal;
            if (value == null)
                return result;
            if ((value & SD.FontStyle.Italic) != 0) {
                result |= FontStyle.Italic;
            }
            //if ((native & SD.FontStyle.Underline) != 0) {
            //    result |= FontStyle.Underline;
            //}
            if ((value & SD.FontStyle.Bold) != 0) {
                result |= FontStyle.Oblique;
            }
            return result;
        }

        public static SD.Font ToGdi (this Font value) {
            return (System.Drawing.Font) WidgetRegistry.GetBackend (value);
        }

        public static Size ToXwt (this SD.SizeF value) {
            return new Size (value.Width, value.Height);
        }
        public static Size ToXwt(this SD.Size value) {
            return new Size(value.Width, value.Height);
        }
        public static SD.StringFormat GetDefaultStringFormat() {
            var stringFormat =
                SD.StringFormat.GenericTypographic;
            stringFormat.Trimming = SD.StringTrimming.EllipsisWord;
            //stringFormat.FormatFlags = SD.StringFormatFlags.FitBlackBox;
            stringFormat.FormatFlags = stringFormat.FormatFlags
                                       & ~SD.StringFormatFlags.NoClip
                                       & ~SD.StringFormatFlags.FitBlackBox
                                       & SD.StringFormatFlags.LineLimit
                ;
            return stringFormat;
        }

        public class GraphicsQuality {
            public InterpolationMode InterpolationMode { get; set; }
            public CompositingMode CompositingMode { get; set; }
            public CompositingQuality CompositingQuality { get; set; }

            public SmoothingMode SmoothingMode { get; set; }

            public SD.Text.TextRenderingHint TextRenderingHint { get; set; }

            public PixelOffsetMode PixelOffsetMode { get; set; }
        }

        public static GraphicsQuality DrawHighQuality {
            get {
                return new GraphicsQuality {
                    InterpolationMode = InterpolationMode.HighQualityBilinear,
                    CompositingMode = CompositingMode.SourceOver,
                    CompositingQuality = CompositingQuality.HighQuality,
                    PixelOffsetMode = SD.Drawing2D.PixelOffsetMode.HighQuality
                };
            }
        }
        public static GraphicsQuality DrawTextHighQuality {
            get {
                return new GraphicsQuality {
                    InterpolationMode = InterpolationMode.NearestNeighbor,
                    CompositingMode = CompositingMode.SourceOver,
                    CompositingQuality = CompositingQuality.HighQuality,
                    PixelOffsetMode = SD.Drawing2D.PixelOffsetMode.HighQuality,
                    SmoothingMode = SmoothingMode.HighQuality,
                    TextRenderingHint = SD.Text.TextRenderingHint.AntiAliasGridFit,
                  
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
                    TextRenderingHint = SD.Text.TextRenderingHint.AntiAliasGridFit,
                    PixelOffsetMode=System.Drawing.Drawing2D.PixelOffsetMode.HighQuality
                };
            }
        }
        public static GraphicsQuality CopyHighQuality {
            get {
                return new GraphicsQuality {
                    InterpolationMode = InterpolationMode.HighQualityBilinear,
                    CompositingMode = CompositingMode.SourceCopy,
                    CompositingQuality = CompositingQuality.HighQuality,
                    PixelOffsetMode = SD.Drawing2D.PixelOffsetMode.HighQuality
                };
            }
        }
        public static GraphicsQuality SetQuality (this SD.Graphics g, GraphicsQuality quality) {
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
    }
}