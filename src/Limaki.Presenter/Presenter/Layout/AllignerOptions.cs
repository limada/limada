using Limaki.Drawing;
using Limaki.Drawing.Shapes;

namespace Limaki.Presenter.Layout {
    public class AllignerOptions {
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public VerticalAlignment VerticalAlignment { get; set; }
        public PointOrder PointOrder { get; set; }
        public Distribution Distribution { get; set; }
    }

    public enum Distribution {
        Horizontal,
        Vertical
    }
}