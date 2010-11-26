using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.Drawing.GDI;
using Limaki.Drawing.UI;

namespace Limaki.Winform {
    public class WinformSystemInformation : IUISystemInformation {
        public SizeI DragSize {
            get {
                return GDIConverter.Convert(SystemInformation.DragSize);
            }
        }
        public int DoubleClickTime {
            get {
                return SystemInformation.DoubleClickTime;
            }
        }
    }

}