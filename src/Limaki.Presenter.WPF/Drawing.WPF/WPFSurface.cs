using System.Windows;
using Limaki.Drawing;
using System.Windows.Input;
using System.Windows.Controls;

namespace Limaki.Drawing.WPF {
    public class WPFSurface : ISurface {
        public WPFSurface(Panel graphics) {
            this.Graphics = graphics;
        }

        public RectangleI Clip { get; set; }
        public Panel Graphics { get; set; }

        public MouseButtonEventHandler MouseButtonDown = null;
        public MouseButtonEventHandler MouseButtonUp = null;
    }
}