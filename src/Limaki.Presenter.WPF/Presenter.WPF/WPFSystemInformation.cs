using Limaki.Drawing;
using System.Windows;
using Limaki.View.UI;
using Xwt;
using Size = Xwt.Size;


namespace Limaki.View.WPF {
    public class WPFSystemInformation : IUISystemInformation {
        public Size DragSize {
            get {
                return new Size(12,12);
            }
        }
        public int DoubleClickTime {
            get {
                return 12;
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