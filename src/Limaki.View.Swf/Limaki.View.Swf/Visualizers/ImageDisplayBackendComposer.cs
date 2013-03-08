using System.Drawing;
using Limaki.View.Visualizers;
using Limaki.View.Gdi.UI;
using Limaki.View.Swf.Visualizers;

namespace Limaki.View.Swf.Visualizers {

    public class ImageDisplayBackendComposer : SwfBackendComposer<Image> {

        public override void Factor(Display<Image> display) {
            base.Factor(display);
            
            this.DataLayer = new GdiImageLayer();
        }
    }
}