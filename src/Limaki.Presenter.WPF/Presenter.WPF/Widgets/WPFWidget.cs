using Limaki.Widgets;
using System.Windows;
using System.Windows.Controls;
using Limaki.Widgets.WPF;
using System.Windows.Input;

namespace Limaki.Widgets.WPF {
    public class WPFWidget<T> : Widget<T>, IWPFWidget {
        public WPFWidget() : base() { }
        public WPFWidget(T data) : base(data) { }

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