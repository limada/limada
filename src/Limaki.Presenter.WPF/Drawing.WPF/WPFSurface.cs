using System.Windows;
using Limaki.Drawing;
using System.Windows.Input;
using Xwt;
using Panel = System.Windows.Controls.Panel;

namespace Limaki.Drawing.WPF {
    public class WPFSurface : ISurface {
        public WPFSurface(Panel graphics) {
            this.Graphics = graphics;
        }

        public RectangleD Clip { get; set; }
        public Panel Graphics { get; set; }

        public MouseButtonEventHandler MouseButtonDown = null;
        public MouseButtonEventHandler MouseButtonUp = null;
    }
}