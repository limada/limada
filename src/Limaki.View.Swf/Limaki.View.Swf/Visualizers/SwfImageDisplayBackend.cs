using System.Drawing;
using Limaki.View.Visualizers;
using Limaki.View.Swf.Visualizers;

namespace Limaki.View.Swf.Visualizers {

    public class SwfImageDisplayBackend:DisplayBackend<Image>,IImageDisplayBackend {
       
        public override DisplayFactory<Image> CreateDisplayFactory(DisplayBackend<Image> backend) {

            var result = new ImageDisplayFactory();
            
            var backendComposer = new ImageDisplayBackendComposer();
            backendComposer.Backend = backend;

            result.BackendComposer = backendComposer;
            result.DisplayComposer = new ImageDisplayComposer ();

            return result;
        }

    }
}