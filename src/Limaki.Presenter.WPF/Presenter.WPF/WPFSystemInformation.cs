using Limaki.Drawing;
using Limaki.Presenter.UI;


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
    }

}