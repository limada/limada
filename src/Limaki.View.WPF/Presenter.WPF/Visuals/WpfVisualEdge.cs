using System.Windows;
using System.Windows.Controls;
using Limaki.Visuals;
using Limaki.Visuals.WPF;

namespace Limaki.Visuals.WPF {
    public class WpfVisualEdge<T> : VisualEdge<T>, IWPFVisual {
        public WpfVisualEdge(T data) : base(data) { }
        public WpfVisualEdge(T data, IVisual root, IVisual leaf) : base(data, root, leaf) { }
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