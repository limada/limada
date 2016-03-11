using System.Windows.Controls;

namespace Xwt.WPFBackend
{
	public class ExScrollViewer
		: ScrollViewer, IWpfWidget
	{
		public WidgetBackend Backend
		{
			get;
			set;
		}

		protected override System.Windows.Size MeasureOverride (System.Windows.Size constraint)
		{
			var s = base.MeasureOverride (constraint);
			return Backend.MeasureOverride (constraint, s);
		}

		System.Windows.FrameworkElement NativeWidget () {
		    var port = this.Content as System.Windows.Controls.Panel;
		    if (port != null && port.Children.Count == 1)
		        return port.Children[0] as System.Windows.FrameworkElement;
		    return null;
		}

		protected virtual void RouteKeyEvent (System.Windows.Input.KeyEventArgs e) {
		    var w = NativeWidget();
		    if (w != null && !w.Equals(e.Source) && w.Focusable)
		        w.RaiseEvent(e);
		}


		protected override void OnKeyDown (System.Windows.Input.KeyEventArgs e) {
		    base.OnKeyDown(e);
		    RouteKeyEvent(e);
		}

		protected override void OnKeyUp (System.Windows.Input.KeyEventArgs e) {
		    base.OnKeyUp(e);
		    RouteKeyEvent(e);
		}
	}
}
