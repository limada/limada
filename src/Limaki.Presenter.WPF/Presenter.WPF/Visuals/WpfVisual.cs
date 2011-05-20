using Limaki.Visuals;
using System.Windows;
using System.Windows.Controls;
using Limaki.Visuals.WPF;
using System.Windows.Input;

namespace Limaki.Visuals.WPF {
    public class WpfVisual<T> : Visual<T>, IWPFVisual {
        public WpfVisual() : base() { }
        public WpfVisual(T data) : base(data) { }

        private UIElement _dataElement = null;
        public UIElement DataElement {
            get {
                if (_dataElement ==null) {
                    _dataElement = new TextBlock ();
                }
                ( (TextBlock) _dataElement ).Text = this.Data.ToString ();
                return _dataElement;
            }
        }

    }
}