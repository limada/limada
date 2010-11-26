using System.Windows;
using System.Windows.Controls;
using Limaki.Widgets;
using Limaki.Widgets.WPF;

namespace Limaki.Widgets.WPF {
    public class WPFEdgeWidget<T> : EdgeWidget<T>, IWPFWidget {
        public WPFEdgeWidget(T data) : base(data) { }
        public WPFEdgeWidget(T data, IWidget root, IWidget leaf) : base(data, root, leaf) { }
        private UIElement _dataElement = null;
        public UIElement DataElement {
            get {
                if (_dataElement == null) {
                    _dataElement = new TextBlock();
                }
                ((TextBlock)_dataElement).Text = this.Data.ToString();
                return _dataElement;
            }
        }
    }
}