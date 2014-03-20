using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Xwt;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf {

    public class SwfSystemInformation : IUISystemInformation {
        public Size DragSize {
            get {
                return SystemInformation.DragSize.ToXwt ();
            }
        }
        public int DoubleClickTime {
            get {
                return SystemInformation.DoubleClickTime;
            }
        }
        public int VerticalScrollBarWidth {
            get {
                return SystemInformation.VerticalScrollBarWidth;
            }
        }
        public int HorizontalScrollBarHeight {
            get {
                return SystemInformation.HorizontalScrollBarHeight;
            }
        }
    }

}