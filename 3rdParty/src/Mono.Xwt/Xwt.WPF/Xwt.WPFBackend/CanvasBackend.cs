
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xwt.Backends;
using System.Windows;
using SWC = System.Windows.Controls;
using System.Windows.Media;

namespace Xwt.WPFBackend
{
	class CanvasBackend
		: WidgetBackend, ICanvasBackend
	{
		#region ICanvasBackend Members

		public CanvasBackend ()
		{
			Canvas = new CustomPanel ();
			Canvas.RenderAction = Render;
		    Canvas.CacheMode = new BitmapCache();
		}

		private CustomPanel Canvas
		{
			get { return (CustomPanel)Widget; }
			set { Widget = value; }
		}

		private ICanvasEventSink CanvasEventSink
		{
			get { return (ICanvasEventSink) EventSink; }
		}

		protected override void SetWidgetColor (Drawing.Color value)
		{
			// Do nothing
		}

	    private Rectangle queueRect = Rectangle.Zero; 

		private void Render (System.Windows.Media.DrawingContext dc) {

		    var dirtyRectangle = queueRect;
            queueRect = Rectangle.Zero;
            // disabled! queueRect allways zero!
            if (queueRect == Rectangle.Zero) {
                dirtyRectangle = new Rectangle(0, 0, Widget.ActualWidth, Widget.ActualHeight);
            }
		    var dirtyRect = dirtyRectangle.ToWpfRect();
            if (BackgroundColorSet) {
				SolidColorBrush mySolidColorBrush = new SolidColorBrush ();
				mySolidColorBrush.Color = BackgroundColor.ToWpfColor ();
                dc.DrawRectangle (mySolidColorBrush, null, dirtyRect);
			}
			
			var ctx = new Xwt.WPFBackend.DrawingContext (dc, Widget.GetScaleFactor ());
            ctx.Context.PushClip(new RectangleGeometry(dirtyRect));
		    CanvasEventSink.OnDraw(ctx, dirtyRectangle);
		}

		public void QueueDraw ()
		{
			Canvas.InvalidateVisual ();
            queueRect = Rectangle.Zero; 
		}

		public void QueueDraw (Rectangle rect)
		{
			Canvas.InvalidateVisual ();
            queueRect = rect; 
		}

		public void AddChild (IWidgetBackend widget, Rectangle bounds)
		{
			UIElement element = widget.NativeWidget as UIElement;
			if (element == null)
				throw new ArgumentException ();

			if (!Canvas.Children.Contains (element))
				Canvas.Children.Add (element);

			SetChildBounds (widget, bounds);
		}

		List<IWidgetBackend> children = new List<IWidgetBackend> ();
		List<Rectangle> childrenBounds = new List<Rectangle> ();

		public void SetChildBounds (IWidgetBackend widget, Rectangle bounds)
		{
			int i = children.IndexOf (widget);
			if (i == -1) {
				children.Add (widget);
				childrenBounds.Add (bounds);
			}
			else {
				childrenBounds[i] = bounds;
			}
			Canvas.SetAllocation (children.ToArray (), childrenBounds.ToArray ());
		}

		public void RemoveChild (IWidgetBackend widget)
		{
			UIElement element = widget.NativeWidget as UIElement;
			if (element == null)
				throw new ArgumentException ();

			Canvas.Children.Remove (element);
			int i = children.IndexOf (widget);
			if (i != -1) {
				children.RemoveAt (i);
				childrenBounds.RemoveAt (i);
			}
		}

		#endregion
	}
}
