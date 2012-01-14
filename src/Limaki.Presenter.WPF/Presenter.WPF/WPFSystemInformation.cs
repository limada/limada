using Limaki.Drawing;
using Limaki.Presenter.UI;
using System.Windows;


namespace Limaki.Presenter.WPF {
    public class WPFSystemInformation : IUISystemInformation {
        public SizeI DragSize {
            get {
                return new SizeI(12,12);
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