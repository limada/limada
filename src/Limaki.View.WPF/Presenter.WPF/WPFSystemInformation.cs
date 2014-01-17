using Limaki.Drawing;
using System.Windows;
using Limaki.View.UI;
using Xwt;
using Size = Xwt.Size;
using System.Runtime.InteropServices;


namespace Limaki.View.WPF {
    public class WPFSystemInformation : IUISystemInformation {
        public Size DragSize {
            get {
                return new Size(SystemParameters.MinimumHorizontalDragDistance, SystemParameters.MinimumHorizontalDragDistance);
            }
        }

        [DllImport("user32.dll")]
        static extern uint GetDoubleClickTime ();
        public int DoubleClickTime {
            get {
                return (int)GetDoubleClickTime(); 
            }
        }
        public int VerticalScrollBarWidth {
            get {
                return (int)SystemParameters.VerticalScrollBarWidth;
            }
        }
        public int HorizontalScrollBarHeight {
            get {
                return (int)SystemParameters.HorizontalScrollBarHeight;
            }
        }
    }

}