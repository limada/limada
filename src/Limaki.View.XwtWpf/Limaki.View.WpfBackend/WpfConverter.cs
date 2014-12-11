using System.Windows;
using Limaki.View.Vidgets;
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

        public static string ToWpf (this TextViewerTextType value) {
            //  Rtf, Text, Xaml, and XamlPackage
            if (value == TextViewerTextType.PlainText)
                return DataFormats.OemText;
            if (value == TextViewerTextType.UnicodePlainText)
                return DataFormats.Text;
            if (value == TextViewerTextType.RichText)
                return DataFormats.Rtf;
            return null;
        }
    }
}