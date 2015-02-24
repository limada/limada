using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Drawing;
using Xwt;
using Xwt.Drawing;
using Xwt.Html5.Backend;
using Xwt.Backends;

namespace Limaki.View.Html5 {

    public class Html5DrawingUtils : IDrawingUtils {

        private static HtmlTextLayoutBackend tl = new HtmlTextLayoutBackend();
        public Size GetTextDimension (string text, IStyle style) {
            //return new Size (text.Length * 10, 10);
            var f = style.Font.GetBackend() as FontData;
            return tl.MeasureString(text, f, new Size(0,f.Size));
        }

        public Size GetObjectDimension (object value, IStyle style) {
            var result = new Size ();
            if (!DrawingExtensions.TryGetObjectDimension (value, style, out result))
                return Size.Zero;
            return result;
        }

        public Size ScreenResolution () {
            return new Size (96, 96);
        }

        public Size Resolution (Context context) {
            return new Size (96, 96);
        }



    }
}
