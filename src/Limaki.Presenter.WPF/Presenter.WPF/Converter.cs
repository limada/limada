using System.Windows;
using System.Windows.Input;
using System;
using Limaki.Presenter.UI;
using Xwt;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Point = System.Windows.Point;

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

        public static Xwt.Key Convert(System.Windows.Input.Key native) {
            // Attention! silverlight and wpf-keys are different!
            return Xwt.WPFBackend.KeyboardUtil.TranslateToXwtKey(native);
        }

        public static Xwt.ModifierKeys Convert(System.Windows.Input.ModifierKeys native) {
            var result = Xwt.ModifierKeys.None;
            if ((native & System.Windows.Input.ModifierKeys.Alt) !=0) {
                result |= Xwt.ModifierKeys.Alt;
            }
            if ((native & System.Windows.Input.ModifierKeys.Control) != 0) {
                result |= Xwt.ModifierKeys.Control;
            }
            if ((native & System.Windows.Input.ModifierKeys.Shift) != 0) {
                result |= Xwt.ModifierKeys.Shift;
            }
            if ((native & System.Windows.Input.ModifierKeys.Windows) != 0) {
                result |= Xwt.ModifierKeys.Command;
            }
            return result;
        }

        public static KeyActionEventArgs Convert(KeyEventArgs e,UIElement relativeTo, Point location) {
           return new KeyActionEventArgs(Convert(e.Key),Convert(Keyboard.Modifiers),new Xwt.Point((int)location.X,(int)location.Y));
        }

    }
}