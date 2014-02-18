using Xwt;

namespace Limaki.View.UI {

    public class HeadlessSystemInformation : IUISystemInformation {
        public Size DragSize {
            get { return new Size (1, 1); }
        }

        public int DoubleClickTime {
            get { return 2; }
        }

        public int VerticalScrollBarWidth {
            get { return 10; }
        }

        public int HorizontalScrollBarHeight {
            get { return 10; }
        }
    }
}