using System.Drawing;
using Limaki.View.Display;
using Limaki.View.Gdi.UI;
using Limaki.View.Swf.Display;

namespace Limaki.View.Swf {

    public class ImageBackendComposer : SwfBackendComposer<Image> {

        public override void Factor(Display<Image> display) {
            base.Factor(display);
            
            this.DataLayer = new GdiImageLayer();
        }
    }
}