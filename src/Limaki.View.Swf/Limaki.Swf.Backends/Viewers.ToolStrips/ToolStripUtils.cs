using System.Windows.Forms;
using Limaki.Common;
using Limaki.View.UI;

namespace Limaki.Swf.Backends.Viewers.ToolStrips {
    public class ToolStripUtils {
        static IUISystemInformation _systemInformation = null;
        public static IUISystemInformation SystemInformation {
            get { return _systemInformation ?? (_systemInformation = Registry.Pool.TryGetCreate<IUISystemInformation>()); }
        }

        public static int DropdownWidth = SystemInformation.VerticalScrollBarWidth / 3*2;

        public static void SetCommand(IToolStripCommandItem item, ref ToolStripCommand _command, ToolStripCommand value) {
            var toolStripItem = item as ToolStripItem;
            if (_command != value) {
                try {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.SuspendLayout();
                    if (_command != null)
                        _command.DeAttach(item);
                    _command = value;
                    _command.Attach(item);
                } finally {
                    if (toolStripItem.Owner != null)
                        toolStripItem.Owner.ResumeLayout(true);
                }
            }
        }
    }
}