using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Limaki.View.GtkPrototypes {
    
    public static class ImageFormats {
        public static IEnumerable<string> ListImageFormats (){
            
            var fs = Gdk.Pixbuf.Formats;
            foreach (var f in fs) {
                var mime= $"{string.Join (" | ", f.MimeTypes)}";
                Trace.WriteLine (mime);
                yield return mime;
            }
        }
    }
}
