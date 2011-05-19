using System.Windows;
using System.Windows.Input;
using System;
using Limaki.Presenter.UI;

namespace Limaki.Presenter.WPF {
    public class Converter {

#if ! SILVERLIGHT
        public static MouseActionButtons Convert(MouseButtonState left, MouseButtonState right) {
            var result = MouseActionButtons.None;
            if (left == MouseButtonState.Pressed) {
                result |= MouseActionButtons.Left;
            }
            if (right == MouseButtonState.Pressed) {
                result |= MouseActionButtons.Right;
            }
            return result;
        }
#endif

        public static MouseActionEventArgs Convert(MouseButtonEventArgs e,UIElement relativeTo) {
            Point point = e.GetPosition(relativeTo);
            return new MouseActionEventArgs(
#if ! SILVERLIGHT
                Convert(e.LeftButton,e.RightButton),
                Convert(Keyboard.Modifiers),
                e.ClickCount, 
#else
                MouseActionButtons.Left,
                Convert(Keyboard.Modifiers),
                1,
#endif
                (int)point.X, (int)point.Y, 1);

        }

        public static MouseActionEventArgs Convert(MouseEventArgs e,UIElement relativeTo) {
            var point = e.GetPosition(relativeTo);
            return new MouseActionEventArgs(
#if ! SILVERLIGHT
                Convert(e.LeftButton,e.RightButton),
#else
                MouseActionButtons.Left,
#endif
                Convert(Keyboard.Modifiers), 
                0, (int)point.X, (int)point.Y, 1);

        }

        public static Limaki.Presenter.UI.Key Convert(System.Windows.Input.Key native) {
            // Attention! silverlight and wpf-keys are different!
            var key = Enum.GetName(typeof(System.Windows.Input.Key), native);
            var lkey = Limaki.Presenter.UI.Key.None;

            if (!string.IsNullOrEmpty(key)) {
                try {
                    lkey = (Limaki.Presenter.UI.Key)
#if ! SILVERLIGHT
Enum.Parse(typeof(Limaki.Presenter.UI.Key), key);
#else
                    Enum.Parse(typeof(Limaki.Presenter.UI.Key), key,true);
#endif

                } catch { lkey = Limaki.Presenter.UI.Key.None; }
            }
            return lkey;
        }

        public static Limaki.Presenter.UI.ModifierKeys Convert(System.Windows.Input.ModifierKeys native) {
            var result = Limaki.Presenter.UI.ModifierKeys.None;
            if ((native & System.Windows.Input.ModifierKeys.Alt) !=0) {
                result |= Limaki.Presenter.UI.ModifierKeys.Alt;
            }
            if ((native & System.Windows.Input.ModifierKeys.Control) != 0) {
                result |= Limaki.Presenter.UI.ModifierKeys.Control;
            }
            if ((native & System.Windows.Input.ModifierKeys.Shift) != 0) {
                result |= Limaki.Presenter.UI.ModifierKeys.Shift;
            }
            if ((native & System.Windows.Input.ModifierKeys.Windows) != 0) {
                result |= Limaki.Presenter.UI.ModifierKeys.Windows;
            }
            return result;
        }

        public static KeyActionEventArgs Convert(KeyEventArgs e,UIElement relativeTo, Point location) {
           return new KeyActionEventArgs(Convert(e.Key),Convert(Keyboard.Modifiers),new Limaki.Drawing.PointI((int)location.X,(int)location.Y));
        }

    }
}