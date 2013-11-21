using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#if GTK
using Gdk;

namespace Gecko.Utils
{

	public static class GtkImageCreator
	{
		public static byte[] CapturePng( this GeckoWebBrowser browser )
		{
			// TODO req. mono.cairo
			//var src_context = Gdk.CairoHelper.Create(win);
			//var src_surface = src_context.Target;
			//var dst_surface = new Cairo.ImageSurface(Cairo.Format.ARGB32, win.Allocation.Width, win.Allocation.Height);
			//var dst_context = new Cairo.Context(dst_surface);
			//dst_context.SetSourceSurface(src_surface, 0, 0);
			//dst_context.Paint();
			//dst_surface.WriteToPng("screenshot.png");
			return null;
		}
	}
}
#endif
