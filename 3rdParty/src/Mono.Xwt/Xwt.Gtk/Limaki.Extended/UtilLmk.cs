//
// TextEntryMultiLineBackend.cs
//
// Author:
//       Lytico (http://www.limada.org)
//
// Copyright (c) 2014 http://www.limada.org
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


namespace Xwt.GtkBackend
{
	public static class UtilLmk
    {

		#if !XWT_GTK3
		public static void RenderPlaceholderText (Gtk.Widget widget, Gtk.ExposeEventArgs args, string placeHolderText, ref Pango.Layout layout) {

		    if (layout == null) {
		        layout = new Pango.Layout (widget.PangoContext);
		        layout.FontDescription = widget.PangoContext.FontDescription.Copy ();
		    }

		    int wh, ww;
		    args.Event.Window.GetSize (out ww, out wh);

		    int width, height;
		    layout.SetText (placeHolderText);
		    layout.GetPixelSize (out width, out height);
		    using (var gc = new Gdk.GC (args.Event.Window)) {
		        gc.Copy (widget.Style.TextGC (Gtk.StateType.Normal));
		        var color_a = widget.Style.Base (Gtk.StateType.Normal).ToXwtValue ();
		        var color_b = widget.Style.Text (Gtk.StateType.Normal).ToXwtValue ();
		        gc.RgbFgColor = color_b.BlendWith (color_a, 0.5).ToGtkValue ();

		        args.Event.Window.DrawLayout (gc, 2, (wh - height) / 2, layout);
		    }
		}
		#endif
    }
}
