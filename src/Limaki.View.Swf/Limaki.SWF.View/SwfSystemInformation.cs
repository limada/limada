using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.View.UI;
using Xwt;


namespace Limaki.View.Swf {
    public class SwfSystemInformation : IUISystemInformation {
        public Size DragSize {
            get {
                return GDIConverter.Convert(SystemInformation.DragSize);
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