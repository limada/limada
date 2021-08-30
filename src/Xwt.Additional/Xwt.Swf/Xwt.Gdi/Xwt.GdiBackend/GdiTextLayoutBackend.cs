// 
// GdiTextLayoutBackend.cs
//  
// Author:
//       Lytico 
// 
// Copyright (c) 2012 Lytico (www.limada.org)
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

using Xwt.Drawing;
using SD = System.Drawing;
using System;
namespace Xwt.GdiBackend {

    public class GdiTextLayoutBackend {

        public GdiContext Context { get; set; }
        private double _width;
        private string _text;
        private Font _font;

        public double Width {
            get { return _width; }
            set {
                if (_width != value)
                    _size = null;
                _width = value;
            }
        }

        public double Heigth { get; set; }
        public SD.StringTrimming Trimming { get; set; }

        public string Text {
            get { return _text; }
            set {
                if (_text != value)
                    _size = null;
                _text = value;
            }
        }

        public Font Font {
            get { return _font ?? Font.FromName ("Default 10"); }
            set {
                if (!Xwt.Drawing.XwtDrawingExtensions.Equals (_font, value))
                    _size = null;
                _font = value;
            }
        }

        Size? _size = null;
        public Size Size {
            get {
                if (_size == null) {
                    var font = Font.ToGdi ();
                    var size = Context.Graphics.MeasureString (Text, font, (int) Width, Format);
                    return new Size(Math.Ceiling(size.Width), Math.Ceiling(size.Height));
                }
                return _size.Value;
            }
        }

        SD.StringFormat _format = null;
        public SD.StringFormat Format {
            get {
                if (_format == null)
                    _format = GdiConverter.GetDefaultStringFormat ();
                if (_format.Trimming != this.Trimming)
                    _format.Trimming = this.Trimming;
                if (!_format.FormatFlags.HasFlag (SD.StringFormatFlags.NoWrap) && WrapMode == Xwt.WrapMode.None)
                    _format.FormatFlags |= SD.StringFormatFlags.NoWrap;
                else
                    _format.FormatFlags &= ~SD.StringFormatFlags.NoWrap;
                _format.Alignment = SD.StringAlignment.Near;
                _format.LineAlignment = SD.StringAlignment.Center;
                return _format;
            }
        }

        public WrapMode WrapMode { get; set; }

        public double Baseline { get {
                throw new NotImplementedException ();
            } 
        }

        public double Meanline {
            get {
                throw new NotImplementedException ();
            }
        }
    }
}