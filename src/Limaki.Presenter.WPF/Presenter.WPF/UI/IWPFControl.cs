using System.Windows.Input;

using System.Windows;
using Limaki.Drawing;
using Limaki.Drawing.WPF;
using Limaki.Presenter;

namespace Limaki.Presenter.WPF {
    public interface IWPFControl : IControl {
        WPFSurface Surface { get; }
        Cursor Cursor { get; set; }
    }
}