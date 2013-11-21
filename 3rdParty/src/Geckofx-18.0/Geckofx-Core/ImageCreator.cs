using System;
using Gecko.DOM;

namespace Gecko
{
	/// <summary>
	/// Creates Png images of webbrowser
	/// </summary>
	public static class ImageCreator
	{
		/// <summary>
		/// Get byte array with png image of the current browsers Window.
		/// Wpf methods on windows platform don't use a Bitmap :-/
		/// Not captures plugin (Flash,etc...) windows
		/// </summary>
		/// <param name="xOffset"></param>
		/// <param name="yOffset"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns></returns>
		public static byte[] CanvasGetPngImage(this IGeckoWebBrowser browser, uint xOffset, uint yOffset, uint width, uint height)
		{
			if (width == 0)
				throw new ArgumentException("width");

			if (height == 0)
				throw new ArgumentException("height");

			// Use of the canvas technique was inspired by: the abduction! firefox plugin by Rowan Lewis
			// https://addons.mozilla.org/en-US/firefox/addon/abduction/

			// Some opertations fail without a proper JSContext.
			using (AutoJSContext jsContext = new AutoJSContext(GlobalJSContextHolder.BackstageJSContext))
			{
				GeckoCanvasElement canvas = (GeckoCanvasElement)browser.Document.CreateElement("canvas");
				canvas.Width = width;
				canvas.Height = height;

				nsIDOMHTMLCanvasElement canvasPtr = (nsIDOMHTMLCanvasElement)canvas.DomObject;
				nsIDOMCanvasRenderingContext2D context;
				using (nsAString str = new nsAString("2d"))
				{
					context = (nsIDOMCanvasRenderingContext2D)canvasPtr.MozGetIPCContext(str);
				}

				// the bitmap image needs to conform to the (Full)Zoom being applied, otherwise it will render wrongly
				var zoom = browser.GetMarkupDocumentViewer().GetFullZoomAttribute();
				context.Scale(zoom, zoom);

				using (nsAString color = new nsAString("rgb(255,255,255)"))
				{
					context.DrawWindow((nsIDOMWindow)browser.Window.DomWindow, xOffset, yOffset, width, height, color,
										(uint)(nsIDOMCanvasRenderingContext2DConsts.DRAWWINDOW_DO_NOT_FLUSH |
												   nsIDOMCanvasRenderingContext2DConsts.DRAWWINDOW_DRAW_VIEW |
												   nsIDOMCanvasRenderingContext2DConsts.DRAWWINDOW_ASYNC_DECODE_IMAGES |
												   nsIDOMCanvasRenderingContext2DConsts.DRAWWINDOW_USE_WIDGET_LAYERS));
					;
				}

				string data = canvas.toDataURL("image/png");
				byte[] bytes = Convert.FromBase64String(data.Substring("data:image/png;base64,".Length));
				return bytes;
			}
		}

		public static byte[] CanvasGetPngImage(this IGeckoWebBrowser browser,uint width, uint height)
		{
			return CanvasGetPngImage(browser,0, 0, width, height);
		}
	}
}
