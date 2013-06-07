using System.Drawing;
using Limaki.View.Visualizers;
using Limaki.View.Swf.Visualizers;
using Limaki.View.Gdi.UI;

namespace Limaki.View.Swf.Visualizers {

    public class ImageDisplayBackend:DisplayBackend<Image>,IImageDisplayBackend {
       
        public override DisplayFactory<Image> CreateDisplayFactory(DisplayBackend<Image> backend) {

            var result = new ImageDisplayFactory();
            
            var backendComposer = new ImageDisplayBackendComposer();
            backendComposer.Backend = backend;

            result.BackendComposer = backendComposer;
            result.DisplayComposer = new ImageDisplayComposer ();

            return result;
        }

    }

    public class ImageDisplayBackendComposer : SwfBackendComposer<Image> {

        public override void Factor (Display<Image> display) {
            base.Factor(display);

            this.DataLayer = new GdiImageLayer();
        }
    }
}