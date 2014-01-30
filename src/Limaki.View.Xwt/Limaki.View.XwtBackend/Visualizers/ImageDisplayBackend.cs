using Limaki.View.Visualizers;
using Xwt.Drawing;

namespace Limaki.View.XwtBackend {
    public class ImageDisplayBackend : DisplayBackend<Image>, IImageDisplayBackend {

        public override DisplayFactory<Image> CreateDisplayFactory (DisplayBackend<Image> backend) {

            return new ImageDisplayFactory {
                BackendComposer = new ImageDisplayBackendComposer { Backend = backend },
                DisplayComposer = new ImageDisplayComposer(),
            };
        }
    }

    public class ImageDisplayBackendComposer : DisplayBackendComposer<Image> {

        public override void Factor (Display<Image> display) {
            base.Factor(display);
            this.DataLayer = new ImageLayer();
        }
    }

}