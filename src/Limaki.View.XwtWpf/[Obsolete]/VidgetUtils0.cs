using System;
using System.Windows;
using Limaki.View.WpfBackend;

namespace Limaki.View.Vidgets {

    public static class VidgetUtils0 {

        public static void SetCommand (IToolStripCommandToggle0 item, ref ToolStripCommand0 _command, ToolStripCommand0 value) {
            if (_command != value) {
                try {
                    if (_command != null)
                        _command.DeAttach (item);
                    _command = value;
                    _command.Attach (item);
                } finally { }
            }
        }

        [Obsolete]
        public static void AddItems (this ToolStripBackend toolstripbackend, params UIElement[] items) {
            foreach (var item in items)
                toolstripbackend.Control.Items.Add (item);

        }
    }


}