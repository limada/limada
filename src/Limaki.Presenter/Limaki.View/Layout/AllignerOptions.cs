using Limaki.Drawing.Shapes;
using Xwt;
using Limaki.Drawing;

namespace Limaki.View.Layout {

    public class AllignerOptions {
        public Alignment AlignX { get; set; }
        public Alignment AlignY { get; set; }
        public PointOrder PointOrder { get; set; }
        public Dimension Dimension { get; set; }
    }

    public enum Dimension {
        X,Y
    }
}