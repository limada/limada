using System.Windows.Input;

using System.Windows;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.View;

namespace Limaki.View.WPF {
    public interface IWPFControl : IControl {
        WPFSurface Surface { get; }
        Cursor Cursor { get; set; }
    }
}