// 
// GdiContext.cs
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


using System;
using System.Collections.Generic;
using Xwt.Drawing;

namespace Xwt.Html5.Backend {
    public class Html5Context {

        public Html5Context () {
            Color = Colors.Black;
            LineWidth = 1;
        }

        IHtmlChunk _context = null;
        public IHtmlChunk Context {
            get { return _context ?? (_context = new HtmlChunk ("context")); }
            set { _context = value; }
        }

        public Color Color { get; set; }
        public double LineWidth { get; set; }
        public double[] LineDash { get; set; }
        public object Pattern { get; set; }
        public Font Font { get; set; }

        Matrix _ctm = null;
        public Matrix CTM { get { return _ctm ?? (_ctm = new Matrix()); } set { _ctm = value; } }

        public double Angle { set { CTM.Rotate(value); } }
        public double ScaleX { get { return CTM.M11; } set { CTM.Scale(value,1); } }
        public double ScaleY { get { return CTM.M22; } set { CTM.Scale(1, value); } }
        public double TranslateX { get { return CTM.OffsetX; } set { CTM.Translate(value,0); } }
        public double TranslateY { get { return CTM.OffsetY; } set { CTM.Translate(0,value); } }
        

        public Html5Context (Html5Context c): this () {
            c.SaveTo (this, false);
        }

        private Point _current;
        public Point Current {
            get { return _current; }
            set { _current = value; }
        }

        protected Stack<Html5Context> contexts;

        public void SaveTo (Html5Context c, bool p) {
            c.Font = this.Font;
            c.LineDash = this.LineDash;
            c.Current = this.Current;
            c.Context = this.Context;
            c.Pattern = this.Pattern;
            c.CTM = new Matrix(this.CTM);

        }

        public void Save () {
            if (this.contexts == null)
                this.contexts = new Stack<Html5Context> ();

            this.contexts.Push (new Html5Context (this) { });
        }


        public void Restore () {
            if (this.contexts == null || this.contexts.Count == 0)
                throw new InvalidOperationException ();

            var c = this.contexts.Pop ();

            c.SaveTo (this, true);

        }


        
    }
}