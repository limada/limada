using System.Windows;
using Xwt;
using Xwt.WPFBackend;

namespace Limaki.View.WpfBackend {

    public static class WpfConverter {

        public static ButtonEventArgs ToXwtButtonArgs (FrameworkElement widget, System.Windows.Input.MouseButtonEventArgs e) {
            var pos = e.GetPosition (widget);
            return new ButtonEventArgs () {
                X = pos.X,
                Y = pos.Y,
                MultiplePress = e.ClickCount,
                Button = e.ChangedButton.ToXwtButton ()
            };
        }
    }
}